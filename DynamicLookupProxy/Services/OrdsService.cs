using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace DynamicLookupProxy.Services
{
    public class OrdsService : IOrdsService
    {
        private readonly RestClient _client = new RestClient("https://149.155.63.154/ords/xxopa/opa");

        private string _accessToken = string.Empty;

        private DateTime _accessTokenRenewAt = DateTime.MinValue;
        
        public string Lookup(string query)
        {
//            Task<HttpResponseMessage> responseTask =
//                client.GetAsync("http://localhost:6000/api/lookup/ords/" + query);
//            HttpResponseMessage responseMessage = responseTask.Result;
//            string responseContent = responseMessage.Content.ReadAsStringAsync().Result;
//            return responseContent;
            string accessToken = AcquireAccessToken();
            
//            string requestUri = "http://localhost:6000/api/lookup/ords/" + apiPath + "?" + q;
            query = query.Replace("&", "%26");
            _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest(query, Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
//            request.AddParameter("q", q);
            var response = _client.Execute(request);
            string responseContent = response.Content;
            Console.WriteLine($"ResponseContent: {responseContent}");
            return responseContent;
        }

        public string GetEmployeeDetails(string apiPath, string q)
        {
            string accessToken = AcquireAccessToken();

//            string requestUri = "http://localhost:6000/api/lookup/ords/" + apiPath + "?" + q;
            _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest("empdetail", Method.GET);
            q = q.Replace("&", "%26");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("q", q);
            var response = _client.Execute(request);
            return response.Content;
        }

        private string AcquireAccessToken()
        {
            if (string.IsNullOrEmpty(_accessToken) || DateTime.Compare(_accessTokenRenewAt, DateTime.Now) < 0)
            {
                Task task = GetAccessToken();
                task.Wait();
            }

            return _accessToken;
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
                    _accessToken = json["access_token"];
                    string expiresIn = json["expires_in"];
                    if (!string.IsNullOrEmpty(expiresIn))
                    {
                        _accessTokenRenewAt = DateTime.Now.AddSeconds(int.Parse(expiresIn));
                    }
                    else
                    {
                        _accessTokenRenewAt = DateTime.MinValue;
                    }
                    Console.WriteLine($"Access Token={_accessToken}");
                }
            }
        }

    }
}