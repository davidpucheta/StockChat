using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StockChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private const string StockEndpoint = "https://stooq.com/q/l/?f=sd2t2ohlcv&h&e=csv&s=";
        private readonly IHttpClientFactory _httpClientFactory;

        public StockController(IHttpClientFactory httpClientFactory)
        {
           _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string stockCode)
        {
            var stockUrl = StockEndpoint + stockCode;
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(stockUrl);

            var result = client.GetStringAsync(stockUrl).Result;

            var stockPrice = result.Split(',')[13];

            if (!string.IsNullOrEmpty(stockPrice))
            {
                return Ok(stockPrice);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
