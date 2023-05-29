using System.Net.Http;
using System.Threading;
using AutoMapper;
using catalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace catalog.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private IProductService _productService;
    private IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public CatalogController(ILogger<CatalogController> logger, HttpClient httpClient,
        IProductService productService, IMapper mapper, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClient;
        _productService = productService;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public Task<string> Get()
    {
        _logger.LogInformation(2001, "TRACING DEMO: Call stock service");
        return _httpClient.GetStringAsync("http://stock:3000/");
    }

    [HttpGet("/products")]
    public async Task<IActionResult> GetProducts()
    {
        var stockClient = _httpClientFactory.CreateClient("Stock-Service");
        var pricingClient = _httpClientFactory.CreateClient("Pricing-Service");

        _logger.LogInformation(2000, "TRACING DEMO: Get all products from database");
        var products = _productService.GetAll();

        foreach (var p in products)
        {
            _logger.LogInformation(2001, "TRACING DEMO: Call stock service");
            var stock = await stockClient.GetFromJsonAsync<StockResponse>("/product/" + p.Id);
            _logger.LogInformation(2001, "TRACING DEMO: Call product service");
            var price = await pricingClient.GetFromJsonAsync<PriceResponse>("/product/" + +p.Id);
            p.Stock = stock!.stock;
            p.Price = price!.price;
        }

        return Ok(products);
    }
}

