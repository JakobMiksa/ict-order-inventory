using System.Net.Http.Json;
using ICT.Model;

namespace ICT.Desktop;

public partial class StockForm : Form
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:5003/") };

    public StockForm()
    {
        InitializeComponent();
    }

    private async void StockForm_Load(object sender, EventArgs e)
    {
        await LoadProducts();
    }

    private async Task LoadProducts()
    {
        try
        {
            List<Product> products = (await _client.GetFromJsonAsync<List<Product>>("api/products"))!;
            productsGrid.DataSource = products;
            productsGrid.Columns["Name"]!.HeaderText = "Vare";
            productsGrid.Columns["Price"]!.HeaderText = "Pris";
            productsGrid.Columns["StockQuantity"]!.HeaderText = "På lager";
        }
        catch (HttpRequestException)
        {
            messageLabel.Text = "Kan ikke få forbindelse til serveren.";
        }
    }

    private async void addStockButton_Click(object sender, EventArgs e)
    {
        Product? product = productsGrid.CurrentRow?.DataBoundItem as Product;
        if (product == null)
        {
            messageLabel.Text = "Vælg en vare.";
            return;
        }

        int amount = (int)amountUpDown.Value;

        try
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"api/products/{product.Id}/stock", amount);
            if (response.IsSuccessStatusCode)
            {
                messageLabel.Text = $"{amount} stk. {product.Name} er lagt på lager.";
                await LoadProducts();
            }
            else
            {
                messageLabel.Text = await response.Content.ReadAsStringAsync();
            }
        }
        catch (HttpRequestException)
        {
            messageLabel.Text = "Kan ikke få forbindelse til serveren.";
        }
    }
}
