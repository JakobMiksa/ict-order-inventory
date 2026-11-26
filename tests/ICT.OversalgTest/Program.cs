using System.Net.Http.Json;
using ICT.Model;

string apiUrl = "http://localhost:5003/";
int productId = 4;
int antalKunder = 10;

HttpClient client = new HttpClient();

List<Task<HttpResponseMessage>> bestillinger = new List<Task<HttpResponseMessage>>();
for (int i = 0; i < antalKunder; i++)
{
    Order order = new Order();
    order.MemberId = 1;
    order.Lines.Add(new OrderLine { ProductId = productId, Quantity = 1 });
    bestillinger.Add(client.PostAsJsonAsync(apiUrl + "api/orders", order));
}

HttpResponseMessage[] svar = await Task.WhenAll(bestillinger);

int lykkedes = svar.Count(s => s.IsSuccessStatusCode);
Console.WriteLine($"{antalKunder} kunder bestiller vare {productId} på samme tid.");
Console.WriteLine($"{lykkedes} bestillinger lykkedes, {antalKunder - lykkedes} blev afvist.");

List<Product> varer = (await client.GetFromJsonAsync<List<Product>>(apiUrl + "api/products"))!;
Product vare = varer.First(p => p.Id == productId);
Console.WriteLine($"{vare.Name} har nu {vare.StockQuantity} på lager.");
