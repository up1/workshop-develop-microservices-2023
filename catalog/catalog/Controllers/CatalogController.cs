using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace catalog.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ILogger<CatalogController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [HttpGet]
    public Task<string> Get()
    {
        _logger.LogInformation(2001, "TRACING DEMO: Call stock service");
        return _httpClient.GetStringAsync("http://stock:3000/");
    }
}

