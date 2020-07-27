using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace DynamicLookupProxy.Services
{
    public class OrdsService : IOrdsService
    {
        RestClient client = new RestClient("https://149.155.63.154/ords/xxopa/opa");

        string accessToken = string.Empty;
        
        public string Lookup(string query)
        {
            throw new System.NotImplementedException();
        }

        public string GetEmployeeDetails(string apiPath, string q)
        {
            GetAccessToken();
            
//            string requestUri = "http://localhost:6000/api/lookup/ords/" + apiPath + "?" + q;
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest("empdetail", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("q", q);
            var response = client.Execute(request);
            return response.Content;
        }
        
        async Task GetAccessToken()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
 
            using (var httpClient = new HttpClient(handler))
            {
                //using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://oca.dba5.uksbs.co.uk/ords/xxopa/opa/oauth/token"))
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://149.155.63.154/ords/xxopa/opa/oauth/token"))
                {
                    var clientId = "XTBm-_jQCsXidPHZDBUkUg..";
                    var clientSecret = "sqQk0P35192e5hOXy0QfvQ..";
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
 
                    request.Content = new StringContent("grant_type=client_credentials");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
 
                    var response = await httpClient.SendAsync(request);
                    var result = response.Content.ReadAsStringAsync().Result;
 
                    var json = JsonConvert.DeserializeObject<dynamic>(result);
                    accessToken = json["access_token"];
                    Console.WriteLine($"Access Token={accessToken}");
                }
            }
        }

    }
}