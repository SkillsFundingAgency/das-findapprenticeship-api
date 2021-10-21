using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FAA.Data.ElasticSearch;
using SFA.DAS.FAA.Data.Extensions;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.Repository
{
    public class ApprenticeshipVacancySearchRepository : IVacancySearchRepository
    {
        private readonly IElasticLowLevelClient _client;
        private readonly ElasticEnvironment _environment;
        private readonly IElasticSearchQueries _elasticQueries;
        private readonly ILogger<ApprenticeshipVacancySearchRepository> _logger;
        private const string IndexName = "-faa-apprenticeships";

        public ApprenticeshipVacancySearchRepository(
            IElasticLowLevelClient client, 
            ElasticEnvironment environment, 
            IElasticSearchQueries elasticQueries, 
            ILogger<ApprenticeshipVacancySearchRepository> logger)
        {
            _client = client;
            _environment = environment;
            _elasticQueries = elasticQueries;
            _logger = logger;
        }

        private string ApprenticeshipVacanciesIndex => _environment.Prefix + IndexName;

        public async Task<bool> PingAsync()
        {
            var pingResponse = await _client.CountAsync<StringResponse>(
                ApprenticeshipVacanciesIndex, 
                PostData.String(""), 
                new CountRequestParameters(), 
                CancellationToken.None);

            if (!pingResponse.Success)
            {
                _logger.LogDebug($"Elastic search ping failed: {pingResponse.DebugInformation ?? "no information available"}");
            }

            return pingResponse.Success;
        }

        public async Task<ApprenticeshipSearchItem> Get(string vacancyReference)
        {
            _logger.LogInformation($"Starting get vacancy [{vacancyReference}]");
            
            var query = _elasticQueries.GetVacancyQuery.Replace("{vacancyReference}", vacancyReference);
            var jsonResponse = await _client.SearchAsync<StringResponse>(ApprenticeshipVacanciesIndex, PostData.String(query));
            var responseBody = JsonConvert.DeserializeObject<ElasticResponse<ApprenticeshipSearchItem>>(jsonResponse.Body);
            
            _logger.LogInformation($"Found [{responseBody.hits.total.value}] hits for vacancy [{vacancyReference}]");
            
            return responseBody.Items.SingleOrDefault();
        }

        public async Task<ApprenticeshipSearchResponse> Find(string searchTerm, int pageNumber, int pageSize, int? ukprn = null)
        {
            _logger.LogInformation("Starting vacancy search");

            var startingDocumentIndex = StartingDocumentIndex(pageNumber, pageSize);

            var elasticSearchResult = await GetSearchResult(
                searchTerm, pageSize, startingDocumentIndex, null);

            if (elasticSearchResult == null)
            {
                _logger.LogWarning("Searching failed. Elastic search response could not be de-serialised");
                return new ApprenticeshipSearchResponse();
            }

            _logger.LogDebug("Searching complete, returning search results");

            var totalRecordCount = await GetTotal();
            
            var searchResult =  new ApprenticeshipSearchResponse
            {
               ApprenticeshipVacancies = elasticSearchResult.Items,
               TotalFound = elasticSearchResult.hits.total.value,
               Total = totalRecordCount
            };

            return searchResult;
        }

        private static int StartingDocumentIndex(int pageNumber, int pageSize)
        {
            return pageNumber < 2 ? 0 : (pageNumber - 1) * pageSize;
        }

        private async Task<ElasticResponse<ApprenticeshipSearchItem>> GetSearchResult(
            string searchTerm, int pageSize,
            int startingDocumentIndex, int? ukprn)
        {
            var parameters = new Dictionary<string, object>
            {
                {nameof(pageSize), pageSize},
                {nameof(startingDocumentIndex), startingDocumentIndex},
                {nameof(searchTerm), searchTerm}
            };
            //if (ukprn.HasValue) parameters.Add(nameof(ukprn), ukprn.Value);
            
            var request = string.IsNullOrEmpty(searchTerm) ?
                _elasticQueries.GetAllVacanciesQuery.ReplaceParameters(parameters) :
                _elasticQueries.FindVacanciesQuery.ReplaceParameters(parameters);

            _logger.LogDebug($"Searching with search term: {searchTerm}");

            var jsonResponse =
                await _client.SearchAsync<StringResponse>(ApprenticeshipVacanciesIndex, PostData.String(request));

            var searchResult = JsonConvert.DeserializeObject<ElasticResponse<ApprenticeshipSearchItem>>(jsonResponse.Body);

            return searchResult;
        }

        private async Task<int> GetTotal()
        {
            var jsonResponse = await _client.CountAsync<StringResponse>(
                ApprenticeshipVacanciesIndex,
                PostData.String(_elasticQueries.GetVacanciesCountQuery));

            var result = JsonConvert.DeserializeObject<ElasticCountResponse>(jsonResponse.Body);

            return result.count;
        }
    }
}
