using API.Contracts;
using API.Shared;
using static API.Features.Search.SearchByCountryName;
using Newtonsoft.Json;

namespace API.Utilities
{
    public class SearchCountryHttpUtils
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public SearchCountryHttpUtils(HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<Result<CountrySearchResult>> GetCountryDataFromDataGatewayService(Query request)
        {
            HttpResponseMessage response
                = await httpClient.GetAsync(
                    configuration.GetSection("ServicesUrl:DataGateway").Value
                    + $"/api/country");
            return HandleHttpCountryRetrieval(response);
        }

        private static Result<CountrySearchResult> HandleHttpCountryRetrieval(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode
                ? HandleSuccessCountryResponse(response)
                : Result.Failure<CountrySearchResult>(
                              new Error("DataGateway.NotFound", "DataGateway returned no data."));
        }

        private static Result<CountrySearchResult> HandleSuccessCountryResponse(HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            if (data == null)
            {
                return Result.Failure<CountrySearchResult>(
                                               new Error("DataGateway.NotFound", "DataGateway returned no data."));
            }

            return DeserializeHttpCountryResponse(data);
        }

        private static Result<CountrySearchResult> DeserializeHttpCountryResponse(string data)
        {
            var result = JsonConvert.DeserializeObject<CountrySearchResult>(data);
            return result == null
                ? Result.Failure<CountrySearchResult>(
                        new Error("DataGateway.DeserializeObject",
                                  "Failed to deserialize the data returned by the DataGateway. The data might be corrupted, incorrectly formatted, or incompatible with the expected object structure."))
                : (Result<CountrySearchResult>)result;
        }
    }
}
