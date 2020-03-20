using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace DynamicLookupProxy.Controllers
{
    [ApiController]
    [Route("api/lookup/ords")]
    public class OrdsStubController : ControllerBase
    {

        [HttpGet("organisations")]
        public ResponseItems Organisations()
        {
            ResponseItems responseItems = new ResponseItems(new List<object>()
            {
                new OrdsOrganisation("1000", "UKGV 100"),
                new OrdsOrganisation("1001", "UKSBS 101")
            });
            return responseItems;
        }
        
        [HttpGet("organisations/{organisation}/costcentres")]
        public ResponseItems CostCentres(string organisation)
        {
            ResponseItems responseItems = new ResponseItems(new List<object>()
            {
                new OrdsCostCentre("1000", "UKGV 100", "107988"),
                new OrdsCostCentre("1000", "UKGV 100", "105739")
            });
            return responseItems;
        }
        
        [HttpGet("organisations/{organisation}/costcentres/{costcentre}/positions")]
        public ResponseItems Positions(string organisation, string costcentre)
        {
            ResponseItems responseItems = new ResponseItems(new List<object>()
            {
                new OrdsPosition("1000", "UKGV 100", "107988", "BIS-1.1.2.107988"),
                new OrdsPosition("1000", "UKGV 100", "107988", "SIP-1.2.5.107988")
            });
            return responseItems;
        }
        
    }

    public class ResponseItems
    {
        public List<object> Items { get; }

        public ResponseItems(List<object> items)
        {
            Items = items;
        }
    }

    public class OrdsOrganisation
    {
        public string Org_id { get; }
        public string Org_name { get; }

        public OrdsOrganisation(string org_id, string org_name)
        {
            Org_id = org_id;
            Org_name = org_name;
        }
    }

    public class OrdsCostCentre
    {
        public string Org_id { get; }
        public string Org_name { get; }
        public string Cost_centre { get; }
        public OrdsCostCentre(string org_id, string org_name, string cost_centre)
        {
            Org_id = org_id;
            Org_name = org_name;
            Cost_centre = cost_centre;
        }
    }
    
    public class OrdsPosition
    {
        public string Org_id { get; }
        public string Org_name { get; }
        public string Cost_centre { get; }
        public string Position { get; }
        public OrdsPosition(string org_id, string org_name, string cost_centre, string position)
        {
            Org_id = org_id;
            Org_name = org_name;
            Cost_centre = cost_centre;
            Position = position;
        }
    }
}