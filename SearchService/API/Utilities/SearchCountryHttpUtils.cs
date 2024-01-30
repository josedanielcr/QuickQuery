﻿using API.Contracts;
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
            var url = $"{configuration["ServicesUrl:DataGateway"]}/api/country?name={request.Name}";
            var response = await httpUtils.ExecuteHttpGetAsync(url, headers);

            return response.IsSuccess
                ? await HandleHttpResponse(response.Value)
                : Result.Failure<CountrySearchResult>(new Error("DataGateway.HttpError", "Error occurred during HTTP request."));
        }

        private async Task<Result<CountrySearchResult>> HandleHttpResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<CountrySearchResult>(new Error("DataGateway.NotFound", "DataGateway returned no data."));
            }

            var data = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(data)
                ? Result.Failure<CountrySearchResult>(new Error("DataGateway.EmptyResponse", "DataGateway returned an empty response."))
                : httpUtils.DeserializeResponseContent<CountrySearchResult>(data);
        }
    }
}
