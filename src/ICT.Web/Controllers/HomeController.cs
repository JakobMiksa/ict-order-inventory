using System.Diagnostics;
using ICT.Model;
using ICT.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ICT.Web.Controllers;

public class HomeController : Controller
{
    private const int CurrentMemberId = 1;
    private readonly string _apiUrl;

    public HomeController(IConfiguration configuration)
    {
        _apiUrl = configuration["ApiUrl"]!;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> products = new List<Product>();
        ViewBag.Error = TempData["Error"];

        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_apiUrl + "api/products");

            if (response.IsSuccessStatusCode)
            {
                products = (await response.Content.ReadFromJsonAsync<List<Product>>())!;
            }
            else
            {
                ViewBag.Error = "Kunne ikke hente varerne.";
            }
        }
        catch (HttpRequestException)
        {
            ViewBag.Error = "Kunne ikke få forbindelse til serveren.";
        }

        return View(products);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Order(List<int> productIds, List<int> quantities)
    {
        Order order = new Order();
        order.MemberId = CurrentMemberId;
        for (int i = 0; i < productIds.Count && i < quantities.Count; i++)
        {
            OrderLine line = new OrderLine();
            line.ProductId = productIds[i];
            line.Quantity = quantities[i];
            order.Lines.Add(line);
        }

        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(_apiUrl + "api/orders", order);

            if (response.IsSuccessStatusCode)
            {
                int orderId = await response.Content.ReadFromJsonAsync<int>();
                return View("Receipt", orderId);
            }

            TempData["Error"] = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = "Kunne ikke få forbindelse til serveren.";
        }

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
