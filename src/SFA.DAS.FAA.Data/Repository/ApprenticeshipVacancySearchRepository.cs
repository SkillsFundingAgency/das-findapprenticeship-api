using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FAA.Data.ElasticSearch;
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

        public string GetCurrentApprenticeshipVacanciesIndex() => _environment.Prefix + IndexName;
        
        public async Task<bool> PingAsync()
        {
            var index = GetCurrentApprenticeshipVacanciesIndex();

            var pingResponse = await _client.CountAsync<StringResponse>(
                index, 
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
            throw new NotImplementedException();
        }

        public async Task<ApprenticeshipSearchResponse> Find(string searchTerm, int pageNumber, int pageSize)
        {
            _logger.LogInformation("Starting vacancy search");

            var vacanciesIndex = GetCurrentApprenticeshipVacanciesIndex();

            if (string.IsNullOrWhiteSpace(vacanciesIndex))
            {
                _logger.LogWarning("Searching failed. Latest Apprenticeship Vacancy index does not have a name value");

                return new ApprenticeshipSearchResponse();
            }

            var startingDocumentIndex = StartingDocumentIndex(pageNumber, pageSize);

            var elasticSearchResult = await GetSearchResult(
                searchTerm, pageSize, startingDocumentIndex, vacanciesIndex);

            if (elasticSearchResult == null)
            {
                _logger.LogWarning("Searching failed. Elastic search response could not be de-serialised");
                return new ApprenticeshipSearchResponse();
            }

            _logger.LogDebug("Searching complete, returning search results");

            var totalRecordCount = await GetTotal(vacanciesIndex);
            
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
            int startingDocumentIndex, string vacancyIndexName)
        {
            var request = string.IsNullOrEmpty(searchTerm) ?
                GetSearchString(startingDocumentIndex, pageSize) :
                GetSearchString(startingDocumentIndex, pageSize, searchTerm);

            _logger.LogDebug($"Searching with search term: {searchTerm}");

            var jsonResponse =
                await _client.SearchAsync<StringResponse>(vacancyIndexName, PostData.String(request));

            var searchResult = JsonConvert.DeserializeObject<ElasticResponse<ApprenticeshipSearchItem>>(jsonResponse.Body);

            return searchResult;
        }

        private string GetSearchString(
            int startingDocumentIndex, int pageSize)
        {
            var query = _elasticQueries.GetAllVacanciesQuery.Replace("{startingDocumentIndex}", startingDocumentIndex.ToString());
            query = query.Replace("{pageSize}", pageSize.ToString());

            return query;
        }

        private string GetSearchString(
            int startingDocumentIndex, int pageSize, string searchTerm)
        {
            var query = _elasticQueries.FindVacanciesQuery.Replace("{startingDocumentIndex}", startingDocumentIndex.ToString());
            query = query.Replace("{pageSize}", pageSize.ToString());
            query = query.Replace("{searchTerm}", searchTerm);

            return query;
        }

        private async Task<int> GetTotal(string indexName)
        {
            var jsonResponse = await _client.CountAsync<StringResponse>(indexName,
                PostData.String(_elasticQueries.GetVacanciesCountQuery));

            var result = JsonConvert.DeserializeObject<ElasticCountResponse>(jsonResponse.Body);

            return result.count;
        }
    }
}
