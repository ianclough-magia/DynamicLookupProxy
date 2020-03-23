using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DynamicLookupProxy.Controllers
{
    [ApiController]
    [Route("api/lookup/ords")]
    public class OrdsStubController : ControllerBase
    {

        [HttpGet("businesunits")]
        public ResponseItems BusinesUnits(string q, int limit)
        {
            QueryParams queryParams = q == null ? new QueryParams() : (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            ResponseItems responseItems = new ResponseItems(new List<object>()
            {
                new ResponseItem {Business_unit = "1000"},
                new ResponseItem {Business_unit = "1001"}
            });
            return responseItems;
        }
        
        [HttpGet("costcentres")]
        public ResponseItems CostCentres(string q, int limit)
        {
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            ResponseItems responseItems = new ResponseItems(new List<object>()
            {
                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = "107988"},
                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = "105739"}
            });
            return responseItems;
        }
        
        [HttpGet("positions")]
        public ResponseItems Positions(string q, int limit)
        {
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            ResponseItems responseItems = new ResponseItems(new List<object>()
            {
                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = queryParams.cost_centre, Position = "BIS-1.1.2.107988"},
                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = queryParams.cost_centre, Position = "SIP-1.2.5.107988"}
            });
            return responseItems;
        }
        
        [HttpGet("programmes")]
        public ResponseItems Programmes(string q, int limit)
        {
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            ResponseItems responseItems = new ResponseItems(new List<object>()
            {
                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = queryParams.cost_centre, Programme = "3333333"},
                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = queryParams.cost_centre, Programme = "3434343"}
            });
            return responseItems;
        }
    }

    public class QueryParams
    {
        public string business_unit { get; set; }
        public string cost_centre { get; set; }
        public string position { get; set; }
        public string programme { get; set; }
    }
    
    public class ResponseItems
    {
        public List<object> Items { get; }

        public ResponseItems(List<object> items)
        {
            Items = items;
        }
    }

    public class ResponseItem
    {
        public string Business_unit { get; set; }
        public string Cost_centre { get; set; }
        public string Position { get; set; }
        public string Programme { get; set; }
    }

}