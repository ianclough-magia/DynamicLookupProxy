using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DynamicLookupProxy.Controllers
{
    [ApiController]
    public class OrdsProxyController : Controller
    {
        HttpClient client = new HttpClient();
        
        [HttpGet("/ordsproxy/{ordsQuery}")]
        public IActionResult ProxyLookup(string ordsQuery)
        {
            Console.WriteLine("ordsproxy: " + ordsQuery);
            Task<HttpResponseMessage> responseTask =
                client.GetAsync("http://localhost:6000/api/lookup/ords/" + ordsQuery);
            HttpResponseMessage responseMessage = responseTask.Result;
            string responseContent = responseMessage.Content.ReadAsStringAsync().Result;
            Console.WriteLine("ordsproxy response: " + responseContent);
            return Ok(responseContent);
        }
    }
}