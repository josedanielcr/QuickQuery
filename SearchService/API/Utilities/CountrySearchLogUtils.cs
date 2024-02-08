using API.Contracts;
using API.Shared;
using static API.Features.Search.SearchByCountryName;

namespace API.Utilities
{
    public class CountrySearchLogUtils
    {
        private readonly IConfiguration configuration;
        private readonly HttpUtils httpUtils;
        private readonly JwtUtils jwtUtils;

        public CountrySearchLogUtils(IConfiguration configuration,
            HttpUtils httpUtils,
            JwtUtils jwtUtils)
        {
            this.configuration = configuration;
            this.httpUtils = httpUtils;
            this.jwtUtils = jwtUtils;
        }

        public async Task<Result> LogCountrySearch(Query request, Result<CountrySearchResult> result)
        {
            var url = $"{configuration["ServicesUrl:DataGateway"]}/api/country/search-log";
            var token = request.Headers["Authorization"].ToString().Split(" ")[1];
            var userSid = jwtUtils.GetSidFromToken(token);
            Guid userSidGuid = Guid.Parse(userSid!);

            if (userSid == null)
            {
                return Result.Failure(new Error("UserSid.NotFound", "The User sid was not found in the" +
                    "request token"));
            }

            return await ExecuteCountryLogRequest(request, result, url, userSidGuid);
        }

        private async Task<Result> ExecuteCountryLogRequest(Query request,
            Result<CountrySearchResult> result, string url, Guid userSid)
        {
            var log = new
            {
                UserId = userSid,
                CountryId = result.Value.Id
            };

            return await httpUtils.ExecuteHttpPostAsync(url, log, request.Headers);
        }
    }
}