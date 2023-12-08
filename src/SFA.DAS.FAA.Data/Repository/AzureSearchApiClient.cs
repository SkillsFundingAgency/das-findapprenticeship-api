using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FAA.Domain.Configuration;

namespace SFA.DAS.FAA.Data.Repository;
public class AzureSearchApiClient : ApiClientBase
{
    private readonly FindApprenticeshipsApiConfiguration _configuration;
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;

    public AzureSearchApiClient(IOptions<FindApprenticeshipsApiConfiguration> configuration, 
        IAzureClientCredentialHelper azureClientCredentialHelper,
        HttpClient httpClient) : base(httpClient)
    {
        _configuration = configuration.Value;
        _azureClientCredentialHelper = azureClientCredentialHelper;
        httpClient.BaseAddress = new Uri(configuration.Value.AzureSearchBaseUrl);
    }

    protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        var token = await _azureClientCredentialHelper.GetAccessTokenAsync(_configuration.AzureSearchResource);
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpRequestMessage.Headers.Add("api-key", _configuration.AzureSearchKey);
    }
}
