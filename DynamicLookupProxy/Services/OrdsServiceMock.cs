using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DynamicLookupProxy.Services
{
    public class OrdsServiceMock : IOrdsService
    {

        HttpClient client = new HttpClient();

        public string Lookup(string query)
        {
            Task<HttpResponseMessage> responseTask =
                client.GetAsync("http://localhost:6000/api/lookup/ords/" + query);
            HttpResponseMessage responseMessage = responseTask.Result;
            string responseContent = responseMessage.Content.ReadAsStringAsync().Result;
            return responseContent;
        }

        public string GetEmployeeDetails(string apiPath, string q)
        {
            string requestUri = "http://localhost:6000/api/lookup/ords/" + apiPath + "?" + q;
            Console.WriteLine("requestUri: " + requestUri);
            Task<HttpResponseMessage> responseTask =
                client.GetAsync(requestUri);
            HttpResponseMessage responseMessage = responseTask.Result;
            string responseContent = responseMessage.Content.ReadAsStringAsync().Result;
            return responseContent;
        }
    }
}