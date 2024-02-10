namespace QuickqueryDataGatewayAPI.Contracts
{
    public class CreateCountrySearchLogRequest
    {
        public Guid UserId { get; set; }
        public Guid CountryId { get; set; }
    }
}
