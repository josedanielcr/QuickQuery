using API.Shared;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public class HttpUtils
{
    private readonly IHttpClientFactory httpClientFactory;

    public HttpUtils(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<Result<HttpResponseMessage>> ExecuteHttpGetAsync(string url,
        IDictionary<string, StringValues> headers)
    {
        try
        {
            var client = httpClientFactory.CreateClient("BypassSslValidation");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeadersToRequest(request, headers);
            var response = await client.SendAsync(request);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<HttpResponseMessage>(new Error("HttpGetError", ex.Message));
        }
    }

    private void AddHeadersToRequest(HttpRequestMessage request, IDictionary<string, StringValues> headers)
    {
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value.ToString());
        }
    }

    public Result<T> DeserializeResponseContent<T>(string data)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(data);
            return result != null 
                ? Result.Success(result) 
                : Result.Failure<T>(new Error("DeserializationError", 
                    "Failed to deserialize the response."));
        }
        catch (JsonException ex)
        {
            return Result.Failure<T>(new Error("JsonDeserializationError", ex.Message));
        }
    }
}
