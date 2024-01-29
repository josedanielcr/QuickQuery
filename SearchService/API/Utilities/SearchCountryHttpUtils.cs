using API.Contracts;
using API.Shared;
using static API.Features.Search.SearchByCountryName;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Utilities
{
    public class SearchCountryHttpUtils
    {
        private readonly IConfiguration configuration;
        private readonly HttpUtils httpUtils;

        public SearchCountryHttpUtils(IConfiguration configuration, HttpUtils httpUtils)
        {
            this.configuration = configuration;
            this.httpUtils = httpUtils;
        }

        public async Task<Result<CountrySearchResult>> GetCountryDataFromDataGatewayService(Query request, IDictionary<string, StringValues> headers)
        {
            var url = BuildDataGatewayUrl() + $"country?name={request.Name}";
            var response = await httpUtils.ExecuteHttpGetAsync(url, headers);

            if (!response.IsSuccess)
            {
                return Result.Failure<CountrySearchResult>(new Error("DataGateway.HttpError",
                    "Error occurred during HTTP request."));
            }

            return await HandleHttpCountryRetrieval(response.Value);
        }

        private string BuildDataGatewayUrl()
        {
            return $"{configuration.GetSection("ServicesUrl:DataGateway").Value}/api/";
        }

        private async Task<Result<CountrySearchResult>> HandleHttpCountryRetrieval(HttpResponseMessage response)
        {
            using (response)
            {
                return response.IsSuccessStatusCode
                    ? await HandleSuccessCountryResponse(response)
                    : Result.Failure<CountrySearchResult>(new Error("DataGateway.NotFound", "DataGateway returned no data."));
            }
        }

        private async Task<Result<CountrySearchResult>> HandleSuccessCountryResponse(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(data))
            {
                return Result.Failure<CountrySearchResult>(new Error("DataGateway.EmptyResponse", "DataGateway returned an empty response."));
            }

            return httpUtils.DeserializeResponseContent<CountrySearchResult>(data);
        }
    }
}
