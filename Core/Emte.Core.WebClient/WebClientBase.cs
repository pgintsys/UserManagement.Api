using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Emte.Core.WebClient;
public class WebClientBase
{
    protected readonly string _baseUrl;
    protected readonly HttpClient _httpClient;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    public WebClientBase(
        string baseUrl,
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _baseUrl = baseUrl;
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void HandleResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) { return; }
        Console.WriteLine($"response code: {response.StatusCode}");
        switch (response.StatusCode)
        {
            case System.Net.HttpStatusCode.Unauthorized:
                throw new UnauthorizedAccessException();
            default:
                throw new WebApiRequestException("Response not in correct format");
        }
    }

    private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        HandleResponse(response);
        if (response == null || response.Content == null) { throw new WebApiRequestException("No Content Available"); }
        var data = await response.Content.ReadAsStringAsync();
        var rawData = JsonConvert.DeserializeObject<T>(data);
        return rawData;
    }
    private StringContent HandleRequest<TRequest>(TRequest request)
    {
        var requestData = JsonConvert.SerializeObject(request);
        var rawRequestData = new StringContent(requestData, Encoding.UTF8, "application/json");
        return rawRequestData;
    }

    public async Task<T?> Get<T>(string url, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        return await HandleResponse<T>(response);
    }

    public async Task<TResponse?> PostAndGet<TResponse, TRequest>(string url, TRequest request, CancellationToken cancellationToken)
    {
        var rawRequestData = HandleRequest(request);
        var response = await _httpClient.PostAsync(url, rawRequestData, cancellationToken);
        return await HandleResponse<TResponse>(response);
    }

    public async Task Post<TRequest>(string url, TRequest request, CancellationToken cancellationToken)
    {
        var rawRequestData = HandleRequest(request);
        var response = await _httpClient.PostAsync(url, rawRequestData, cancellationToken);
        HandleResponse(response);
    }

    public async Task<TResponse?> PutAndGet<TResponse, TRequest>(string url, TRequest request, CancellationToken cancellationToken)
    {
        var rawRequestData = HandleRequest(request);
        var response = await _httpClient.PutAsync(url, rawRequestData, cancellationToken);
        return await HandleResponse<TResponse>(response);
    }

    public async Task Put<TRequest>(string url, TRequest request, CancellationToken cancellationToken)
    {
        var rawRequestData = HandleRequest(request);
        var response = await _httpClient.PutAsync(url, rawRequestData, cancellationToken);
        HandleResponse(response);
    }

    public async Task Delete(string url, CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteAsync(url, cancellationToken);
        HandleResponse(response);
    }

    public void AddAuthHeader()
    {
        _httpClient.DefaultRequestHeaders.Remove("Authorization");
        _httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("Authorization", out var authToken);
        Console.WriteLine($"auth token - {authToken}");
        _httpClient.DefaultRequestHeaders.Add("Authorization", authToken.ToString());
        _httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("Authorization", out var RauthToken);
        Console.WriteLine($"AuthToken - {RauthToken}");
    }
}
