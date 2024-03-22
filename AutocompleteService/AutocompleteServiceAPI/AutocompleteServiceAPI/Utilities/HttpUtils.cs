using QuickquerySearchAPI.Shared;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Text;
using QuickquerySearchAPI.Resources.Http;
using QuickquerySearchAPI.Resources.Internal;
using Newtonsoft.Json.Linq;

public class HttpUtils
{
    private readonly IHttpClientFactory httpClientFactory;

    public HttpUtils(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<Result<HttpResponseMessage>> ExecuteHttpGetAsync(string url,
        string token)
    {
        try
        {
            var client = httpClientFactory.CreateClient("BypassSslValidation");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer "+token);
            var response = await client.SendAsync(request);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<HttpResponseMessage>(
                new Error(HttpCodeMessages.Http_GetError,
                string.Format(HttpMessages.HttpGetError, url, ex.Message)));
        }
    }

    public async Task<Result<HttpResponseMessage>> ExecuteHttpPostAsync(string url, object data)
    {
        try
        {
            var client = httpClientFactory.CreateClient("BypassSslValidation");
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddRequestContentData(data, request);
            var response = await client.SendAsync(request);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<HttpResponseMessage>(new Error(
                HttpCodeMessages.Http_PostError,
                string.Format(HttpMessages.HttpPostError, url, ex.Message)));
        }
    }

    private static void AddRequestContentData(object data, HttpRequestMessage request)
    {
        request.Content = new StringContent(JsonConvert.SerializeObject(data),
            Encoding.UTF8, "application/json");
    }

    public Result<T> DeserializeResponseContent<T>(string data)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(data);
            return result != null
                ? Result.Success(result)
                : Result.Failure<T>(new Error(
                    InternalCodeMessages.DeserializationError,
                    string.Format(InternalMessages.Deserialization_Error, "")));
        }
        catch (JsonException ex)
        {
            return Result.Failure<T>(new Error(
                InternalCodeMessages.DeserializationError,
                string.Format(InternalMessages.Deserialization_Error, ex.Message)));
        }
    }
}