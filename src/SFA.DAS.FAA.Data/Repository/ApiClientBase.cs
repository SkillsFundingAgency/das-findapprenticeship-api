using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.Repository;
public abstract class ApiClientBase
{
    private readonly HttpClient _httpClient;

    protected ApiClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
        await AddAuthenticationHeader(requestMessage);

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    protected abstract Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage);

    private static async Task<ApiResponse<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var errorContent = "";
        var responseBody = (TResponse)default;

        if (!response.IsSuccessStatusCode)
        {
            errorContent = json;
        }
        else
        {
            responseBody = JsonSerializer.Deserialize<TResponse>
                (json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        var apiResponse = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

        return apiResponse;
    }
}
