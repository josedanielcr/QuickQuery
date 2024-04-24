using QuickquerySearchAPI.Contracts;
using QuickquerySearchAPI.Shared;
using static QuickquerySearchAPI.Features.Search.SearchByCountryName;
using Microsoft.Extensions.Primitives;
using QuickquerySearchAPI.Resources.DataGateway;

namespace QuickquerySearchAPI.Utilities
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
            var url = $"{configuration["ServicesUrl:DataGateway"]}/api/country?name={request.Name}";
            var response = await httpUtils.ExecuteHttpGetAsync(url, headers);
            return await HandleHttpResponse(response);
        }

        private async Task<Result<CountrySearchResult>> HandleHttpResponse(Result<HttpResponseMessage> response)
        {
            return response.IsSuccess
                            ? await HandleHttpResponse(response.Value)
                            : Result.Failure<CountrySearchResult>(
                                new Error(DataGatewayCodeMessages.DataGatewayHttpError, 
                                DataGatewayMessages.DataGateway_HttpError)
                             );
        }

        public async Task<Result<CountrySearchResult>> IncreaseCountryPropularity(Result<CountrySearchResult> result,
            IDictionary<string, StringValues> headers)
        {
            var url = $"{configuration["ServicesUrl:DataGateway"]}/api/country/increase-popularity";
            var response = 
                await httpUtils.ExecuteHttpPutAsync(url,new { Id = result.Value.Id }, headers);
            return await HandleHttpResponse(response);
        }

        private async Task<Result<CountrySearchResult>> HandleHttpResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<CountrySearchResult>(
                    new Error(DataGatewayCodeMessages.DataGatewayNotFound, 
                    DataGatewayMessages.DataGateway_NotFound));
            }

            var data = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(data)
                ? Result.Failure<CountrySearchResult>(
                    new Error(DataGatewayCodeMessages.DataGatewayEmptyResponse, 
                    DataGatewayMessages.DataGateway_EmptyResponse)
                    )
                : httpUtils.DeserializeResponseContent<CountrySearchResult>(data);
        }
    }
}