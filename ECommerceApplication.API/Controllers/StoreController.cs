using ECommerceApplication.Data;
using ECommerceApplication.Models;
using ECommerceApplication.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceApplication.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StoreController : ControllerBase
{
    private IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    // GET: api/<StoreController>
    [HttpGet]
    public async Task<List<ProductList>> Get()
    {
        var limit = 5;
        var url = $"https://fakestoreapi.com/products";
        var parameters = "?limit={limit}";
        List<ProductList> productList = new List<ProductList>();

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(url);

        HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            productList = JsonConvert.DeserializeObject<List<ProductList>>(jsonString) ?? new List<ProductList>();
        }

        return productList;
    }

    [HttpGet]
    [Route("/products/top-pairs")]
    public IEnumerable<PairAnalysis> Toppairs(int? topN)
    {
        var productPairs = new ProductPairs();

        foreach (var order in Db.Orders)
        {
            order.Products.SelectMany(x => order.Products, (x, y) => new { product1 = x.ProductName, product2 = y.ProductName })
                .Where(pair => string.Compare(pair.product1, pair.product2) < 0)
                .ToList().ForEach(l => productPairs.Add(l.product1, l.product2));
        }

        var p = productPairs.pairList.OrderByDescending(p => p.Value)
            .ThenBy(p => p.Key.product1)
            .ThenBy(p => p.Key.product2)
            .Take(topN ?? productPairs.pairList.Count())
            .Select(p => new PairAnalysis(p.Key.product1, p.Key.product2, p.Value));

        return p;
    }


    [HttpGet]
    [Route("/sales/analyze")]
    public IEnumerable<SalesAnalysis> Salesanalysis()
    {
        return Db.MonthlySales.GroupBy(g => g.ProductName)
            .Select(p => new SalesAnalysis
            {
                name = p.Key,
                consistentHighSales = p.Min(m => m.QuantitySold) > 50
            });
    }

    [HttpGet]
    [Route("/customers/segment")]
    public IEnumerable<CustomerSegment> Customerssegment()
    {
        return Db.Purchases.GroupBy(g => g.CustomerId)
            .Select(p => new CustomerSegment
            {
                customerId = p.Key,
                amount = p.Sum(a => a.Amount)
            });
    }

    [HttpGet]
    [Route("/products/unique")]
    public IEnumerable<UniqueProducts> UniqueProducts(int? year)
    {
        year = year ?? Db.Orders.Max(o => o.PurchaseTime.Year);
        
        var productList = Db.Orders.Where(o => o.PurchaseTime.Year == year)
            .SelectMany(p => p.Products);

        return productList.Select(p => new UniqueProducts
        {
            productId = p.ProductId,
            productName = p.ProductName,
            category = p.Category
        }).Distinct(new UniqueProductsComparer());
    }
}
