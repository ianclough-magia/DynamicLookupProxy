using System;
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
        public IActionResult BusinesUnits(string q, int limit)
        {
            QueryParams queryParams = q == null ? new QueryParams() : (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            string businessUnitsJson = System.IO.File.ReadAllText(@"MockData\BusinessUnits.json");
            return Ok(businessUnitsJson);
        }
        
        [HttpGet("costcentres")]
        public IActionResult CostCentres(string q, int limit)
        {
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            string costCentresJson = System.IO.File.ReadAllText(@"MockData\CostCentres.json");
            return Ok(costCentresJson);
//            ResponseItems responseItems = new ResponseItems(new List<object>()
//            {
//                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = "107988"},
//                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = "105739"}
//            });
//            return responseItems;
        }
        
        [HttpGet("positions")]
        public IActionResult Positions(string q, int limit)
        {
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            string positionsJson = System.IO.File.ReadAllText(@"MockData\Positions.json");
            return Ok(positionsJson);
//            ResponseItems responseItems = new ResponseItems(new List<object>()
//            {
//                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = queryParams.cost_centre, Position = "BIS-1.1.2.107988"},
//                new ResponseItem {Business_unit = queryParams.business_unit, Cost_centre = queryParams.cost_centre, Position = "SIP-1.2.5.107988"}
//            });
//            return responseItems;
        }
        
        [HttpGet("programmes")]
        public IActionResult Programmes(string q, int limit)
        {
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            string programmesJson = System.IO.File.ReadAllText(@"MockData\Programmes.json");
            return Ok(programmesJson);
        }
        
        [HttpGet("empdetail")]
        public ResponseItems EmployeeDetail(string q, int limit)
        {
            Console.WriteLine("OrdeStubController.EmployeeDetail q=" + q + " limit=" + limit);
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            if (string.IsNullOrEmpty(queryParams.employee_number))
            {
                ResponseItems responseItems = new ResponseItems(new List<object>()
                {
                });
                return responseItems;
            }
            else
            {
                string employeesJson = System.IO.File.ReadAllText("c:\\SBS\\ords_employee.data");
                EmployeeDetails employeeDetails = (EmployeeDetails) JsonSerializer.Deserialize(employeesJson, typeof(EmployeeDetails));
                EmployeeDetail selectedEmployeeDetail = null;
                foreach (EmployeeDetail employeeDetail in employeeDetails.items)
                {
                    if (employeeDetail.employee_number == queryParams.employee_number)
                    {
                        selectedEmployeeDetail = employeeDetail;
                    }
                }
                ResponseItems responseItems = new ResponseItems(new List<object>()
                {
                    selectedEmployeeDetail
                });
                return responseItems;
            }
        }

        [HttpGet("emphier")]
        public IActionResult EmployeeHeirarchy(string q, int limit)
        {
            Console.WriteLine("OrdsStubControoller.EmployeeHierarchy");
            QueryParams queryParams = (QueryParams) JsonSerializer.Deserialize(q, typeof(QueryParams));
            string employees = System.IO.File.ReadAllText(@"MockData\Employees.json");
            return Ok(employees);
            
//            ResponseItems responseItems = new ResponseItems(new List<object>()
//            {
//                new EmployeeSummary {Employee_number = queryParams.employee_number},
//                new EmployeeSummary {Employee_number = queryParams.employee_number}
//            });
//            return responseItems;
        }

    }
    
    public class QueryParams
    {
        public string business_unit { get; set; }
        public string cost_centre { get; set; }
        public string position { get; set; }
        public string programme { get; set; }
        public string employee_number { get; set; }
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
        public string Business_unit_name { get; set; }
        public string Cost_centre { get; set; }
        public string Position { get; set; }
        public string Programme { get; set; }
    }

    public class EmployeeDetail
    {
        public string employee_number { get; set; }
        public string full_name { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_names { get; set; }
        public string known_as { get; set; }
        public string national_identifier { get; set; }
        public string email_address { get; set; }
        public string assignment_number { get; set; }
        public string effective_start_date { get; set; }
        public string client { get; set; }
    }

    public class EmployeeDetails
    {
        public List<EmployeeDetail> items { get; set; }
    }
    

    public class EmployeeSummary
    {
        public string Employee_number { get; set; }
        public string Employee_full_name { get; set; }
        public string Supervisor_emp_number { get; set; }
        public string Supervisor_full_name { get; set; }
        public string Person_id { get; set; }
        public string Supervisor_id { get; set; }
    }

}