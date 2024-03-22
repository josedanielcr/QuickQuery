using AutocompleteServiceAPI.Contracts;
using AutocompleteServiceAPI.DataStructures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickquerySearchAPI.Resources.Http;
using QuickquerySearchAPI.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutocompleteServiceAPI.Features
{
    public class TrieInitialization
    {
        private readonly HttpUtils httpUtils;
        private readonly IConfiguration configuration;
        public Trie countriesTrie;

        public TrieInitialization(HttpUtils httpUtils, IConfiguration configuration)
        {
            this.httpUtils = httpUtils;
            this.configuration = configuration;
            countriesTrie = new Trie();
        }

        public async void InitializeTrie()
        {
            Result<HttpResponseMessage> result = await AuthenticateService();
            ManagePostServiceExecution(result);
            string token = ExtractTokenFromAuthentication(result);
            await ExecuteGetCountriesRequest(token);
        }

        private async Task ExecuteGetCountriesRequest(string token)
        {
            var countries = await httpUtils.ExecuteHttpGetAsync(
                            configuration.GetSection("ServicesUrl:DataGateway").Value +
                            "/api/countries", token);
            ManagePostServiceExecution(countries);
            var processedCountries = await ProcessCountries(countries);
            BuildTrie(processedCountries);
        }

        private async Task<List<CountryResult>> ProcessCountries(Result<HttpResponseMessage> countries)
        {
            var httpResponse = await countries.Value.Content.ReadAsStringAsync();
            List<CountryResult> processedCountries
                    = JsonConvert.DeserializeObject<List<CountryResult>>(httpResponse)!;
            return processedCountries;

        }

        private static string ExtractTokenFromAuthentication(Result<HttpResponseMessage> result)
        {
            return JObject.Parse(result!.Value.Content.ReadAsStringAsync().Result)["token"]!.ToString();
        }

        private static void ManagePostServiceExecution(Result<HttpResponseMessage> result)
        {
            if (result.IsFailure || result == null)
            {
                throw new Exception("Failed to authenticate service");
            }
        }

        private async Task<Result<HttpResponseMessage>> AuthenticateService()
        {
            return await httpUtils.ExecuteHttpPostAsync(
                configuration.GetSection("ServicesUrl:AuthenticationService").Value +
                "/api/login", new
                {
                    email = configuration.GetSection("Credentials:Username").Value,
                    password = configuration.GetSection("Credentials:Password").Value
                });
        }

        private void BuildTrie(List<CountryResult> countries)
        {
            foreach (var item in countries)
            {
                countriesTrie.Insert(item.Name.ToLower());
            }
        }
    }
}