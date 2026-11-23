using System.Diagnostics;
using ICT.Model;
using ICT.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ICT.Web.Controllers;

public class HomeController : Controller
{
    private readonly string _apiUrl;

    public HomeController(IConfiguration configuration)
    {
        _apiUrl = configuration["ApiUrl"]!;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> products = new List<Product>();

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
