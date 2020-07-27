namespace DynamicLookupProxy.Services
{
    public interface IOrdsService
    {
        string Lookup(string query);

        string GetEmployeeDetails(string apiPath, string q);
    }
}