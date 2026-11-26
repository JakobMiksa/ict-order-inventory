using System.Net.Http.Json;
using ICT.Model;

string apiUrl = "http://localhost:5003/";
int productId = 4;
int antalKunder = 10;
int levering = 0;
if (args.Length > 0)
{
    levering = int.Parse(args[0]);
}

HttpClient client = new HttpClient();

List<Product> varerFoer = (await client.GetFromJsonAsync<List<Product>>(apiUrl + "api/products"))!;
int lagerFoer = varerFoer.First(p => p.Id == productId).StockQuantity;

List<Task<HttpResponseMessage>> bestillinger = new List<Task<HttpResponseMessage>>();
for (int i = 0; i < antalKunder; i++)
{
    Order order = new Order();
    order.MemberId = 1;
    order.Lines.Add(new OrderLine { ProductId = productId, Quantity = 1 });
    bestillinger.Add(client.PostAsJsonAsync(apiUrl + "api/orders", order));
}

List<Task> alleKald = new List<Task>(bestillinger);
if (levering > 0)
{
    alleKald.Add(client.PostAsJsonAsync(apiUrl + $"api/products/{productId}/stock", levering));
}

await Task.WhenAll(alleKald);

int lykkedes = bestillinger.Count(b => b.Result.IsSuccessStatusCode);
Console.WriteLine($"{antalKunder} kunder bestiller vare {productId} på samme tid. Der var {lagerFoer} på lager.");
if (levering > 0)
{
    Console.WriteLine($"Imens lægger administratoren {levering} på lager.");
}
Console.WriteLine($"{lykkedes} bestillinger lykkedes, {antalKunder - lykkedes} blev afvist.");

List<Product> varer = (await client.GetFromJsonAsync<List<Product>>(apiUrl + "api/products"))!;
Product vare = varer.First(p => p.Id == productId);
Console.WriteLine($"{vare.Name} har nu {vare.StockQuantity} på lager. Det skal være {lagerFoer} + {levering} - {lykkedes} = {lagerFoer + levering - lykkedes}.");
