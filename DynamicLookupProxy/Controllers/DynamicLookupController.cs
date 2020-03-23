using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DynamicLookupProxy.Controllers
{
    [ApiController]
    [Route("api/lookup")]
    public class DynamicLookupController : ControllerBase
    {

        HttpClient client = new HttpClient();
        
        [HttpGet()]
        public ActionResult<List<TextValue>> GetLookup(string query, string valueBinding, string textBinding)
        {
            Console.WriteLine("GetLookup");
            Console.WriteLine("Query: " + query);
            Console.WriteLine("ValueBinding: " + valueBinding);
            Console.WriteLine("TextBinding: " + textBinding);
            
            List<TextValue> textValues = new List<TextValue>();

            try
            {
                Task<HttpResponseMessage> responseTask =
                    client.GetAsync("http://localhost:5000/api/lookup/ords/" + query);
                HttpResponseMessage responseMessage = responseTask.Result;
                string responseContent = responseMessage.Content.ReadAsStringAsync().Result;

                JObject jobject = JObject.Parse(responseContent);

                JArray jarray = (JArray) jobject["items"];
                foreach (JObject item in jarray)
                {
                    if (textBinding != null)
                    {
                        textValues.Add(new TextValue(item[textBinding].ToString(), item[valueBinding].ToString()));
                    }
                    else
                    {
                        textValues.Add(new TextValue(item[valueBinding].ToString(), item[valueBinding].ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return textValues;
        }

        private List<TextValue> GetOrganisations(string filter)
        {
            Console.WriteLine("GetOrganisations");
            List<TextValue> organisations = new List<TextValue>();
            organisations.Add(new TextValue("BEIS", "BEIS"));
            organisations.Add(new TextValue("DIT", "DIT"));
            organisations.Add(new TextValue("UKSA", "UKSA"));
            return organisations;
        }

        private List<TextValue> GetCostCentres(string organisation)
        {
            Console.WriteLine("GetCostCentres");
            List<TextValue> costCentres = new List<TextValue>();
            if (organisation == "BEIS")
            {
                costCentres.Add(new TextValue("BEIS-123", "BEIS-123"));
                costCentres.Add(new TextValue("BEIS-456", "BEIS-456"));
                costCentres.Add(new TextValue("BEIS-789", "BEIS-789"));
            } else if (organisation == "DIT")
            {
                costCentres.Add(new TextValue("DIT-111", "DIT-111"));
                costCentres.Add(new TextValue("DIT-222", "DIT-222"));
                costCentres.Add(new TextValue("DIT-333", "DIT-333"));
            }
            else if (organisation == "UKSA")
            {
                costCentres.Add(new TextValue("987", "987"));
                costCentres.Add(new TextValue("654", "654"));
                costCentres.Add(new TextValue("321", "321"));
            }

            return costCentres;
        }

        private List<TextValue> GetPositions(string costCentre)
        {
            Console.WriteLine("GetPositions");
            List<TextValue> positions = new List<TextValue>();
            if (costCentre == "BEIS-123")
            {
                positions.Add(new TextValue("123-xxx", "123-xxx"));
                positions.Add(new TextValue("123-yyy", "123-yyy"));
                positions.Add(new TextValue("123-zzz", "123-zzz"));
            }
            if (costCentre == "BEIS-456")
            {
                positions.Add(new TextValue("456-xxx", "456-xxx"));
                positions.Add(new TextValue("456-yyy", "456-yyy"));
                positions.Add(new TextValue("456-zzz", "456-zzz"));
            }
            if (costCentre == "BEIS-789")
            {
                positions.Add(new TextValue("789-xxx", "789-xxx"));
                positions.Add(new TextValue("789-yyy", "789-yyy"));
                positions.Add(new TextValue("789-zzz", "789-zzz"));
            }
            if (costCentre == "DIT-111")
            {
                positions.Add(new TextValue("111-xxx", "111-xxx"));
                positions.Add(new TextValue("111-yyy", "111-yyy"));
                positions.Add(new TextValue("111-zzz", "111-zzz"));
            }
            if (costCentre == "DIT-222")
            {
                positions.Add(new TextValue("222-xxx", "222-xxx"));
                positions.Add(new TextValue("222-yyy", "222-yyy"));
                positions.Add(new TextValue("222-zzz", "222-zzz"));
            }
            if (costCentre == "DIT-333")
            {
                positions.Add(new TextValue("333-xxx", "333-xxx"));
                positions.Add(new TextValue("333-yyy", "333-yyy"));
                positions.Add(new TextValue("333-zzz", "333-zzz"));
            }
            if (costCentre == "987")
            {
                positions.Add(new TextValue("987-xxx", "987-xxx"));
                positions.Add(new TextValue("987-yyy", "987-yyy"));
                positions.Add(new TextValue("987-zzz", "987-zzz"));
            }
            if (costCentre == "654")
            {
                positions.Add(new TextValue("654-xxx", "654-xxx"));
                positions.Add(new TextValue("654-yyy", "654-yyy"));
                positions.Add(new TextValue("654-zzz", "654-zzz"));
            }
            if (costCentre == "321")
            {
                positions.Add(new TextValue("321-xxx", "321-xxx"));
                positions.Add(new TextValue("321-yyy", "321-yyy"));
                positions.Add(new TextValue("321-zzz", "321-zzz"));
            }
            return positions;
        }
    }

    public class TextValue
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public TextValue(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}