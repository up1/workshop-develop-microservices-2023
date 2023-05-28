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

    public CatalogController(ILogger<CatalogController> logger, HttpClient httpClient,
        IProductService productService, IMapper mapper)
    {
        _logger = logger;
        _httpClient = httpClient;
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public Task<string> Get()
    {
        _logger.LogInformation(2001, "TRACING DEMO: Call stock service");
        return _httpClient.GetStringAsync("http://stock:3000/");
    }

    [HttpGet("/products")]
    public IActionResult GetProducts()
    {
        _logger.LogInformation(2000, "TRACING DEMO: Get all products from database");
        var products = _productService.GetAll();
        _logger.LogInformation(2001, "TRACING DEMO: Call stock service");
        _httpClient.GetStringAsync("http://stock:3000/");
        _logger.LogInformation(2002, "TRACING DEMO: Call pricing service");
        _httpClient.GetStringAsync("http://pricing:3000/");
        return Ok(products);
    }
}

