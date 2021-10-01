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
    public class VacancyIndexRepository : IVacancyIndexRepository
    {
        private readonly IElasticLowLevelClient _client;
        private readonly FindApprenticeshipsApiEnvironment _environment;
        private readonly IElasticSearchQueries _elasticQueries;
        private readonly ILogger<VacancyIndexRepository> _logger;

        public VacancyIndexRepository(IElasticLowLevelClient client, FindApprenticeshipsApiEnvironment environment, IElasticSearchQueries elasticQueries, ILogger<VacancyIndexRepository> logger)
        {
            _client = client;
            _environment = environment;
            _elasticQueries = elasticQueries;
            _logger = logger;
        }

        public async Task<IndexedVacancySearchResult> Find(
            long providerId, string searchTerm, ushort pageNumber, ushort pageItemCount)
        {
            _logger.LogInformation("Starting reservation search");

            var reservationIndex = await GetCurrentReservationIndex();

            if (string.IsNullOrWhiteSpace(reservationIndex?.Name))
            {
                _logger.LogWarning("Searching failed. Latest Reservation index does not have a name value");

                return new IndexedVacancySearchResult();
            }

            var startingDocumentIndex = (ushort) (pageNumber < 2 ? 0 : (pageNumber - 1) * pageItemCount);

            var elasticSearchResult = await GetSearchResult(
                providerId, searchTerm, pageItemCount, startingDocumentIndex, reservationIndex.Name);

            if (elasticSearchResult == null)
            {
                _logger.LogWarning("Searching failed. Elastic search response could not be de-serialised");
                return new IndexedVacancySearchResult();
            }

            _logger.LogDebug("Searching complete, returning search results");

            var totalRecordCount = await GetSearchResultCount(reservationIndex.Name, providerId);
            
            var searchResult =  new IndexedVacancySearchResult
            {
               Reservations = elasticSearchResult.Items,
               TotalReservations = (uint) elasticSearchResult.hits.total.value
            };

            return searchResult;
        }

        private async Task<ElasticResponse<VacancyIndex>> GetSearchResult(
            long providerId, string searchTerm, ushort pageItemCount,
            ushort startingDocumentIndex, string reservationIndexName)
        {
            var request = string.IsNullOrEmpty(searchTerm) ?
                GetReservationsSearchString(startingDocumentIndex, pageItemCount, providerId) :
                GetReservationsSearchString(startingDocumentIndex, pageItemCount, providerId, searchTerm);

            _logger.LogDebug($"Searching with search term: {searchTerm}");

            var jsonResponse =
                await _client.SearchAsync<StringResponse>(reservationIndexName, PostData.String(request));

            var searchResult = JsonConvert.DeserializeObject<ElasticResponse<VacancyIndex>>(jsonResponse.Body);

            return searchResult;
        }

        public async Task<bool> PingAsync()
        {
            var index = await GetCurrentReservationIndex();

            var pingResponse = await _client.CountAsync<StringResponse>(index.Name, PostData.String(""), new CountRequestParameters(), CancellationToken.None);

            if (!pingResponse.Success)
            {
                _logger.LogDebug($"Elastic search ping failed: {pingResponse.DebugInformation ?? "no information available"}");
            }

            return pingResponse.Success;
        }

        public async Task<IndexRegistryEntry> GetCurrentReservationIndex()
        {
            var data = PostData.String(_elasticQueries.LastIndexSearchQuery);

            _logger.LogDebug("Getting latest reservation index name");

            var response = await _client.SearchAsync<StringResponse>(
                _environment.EnvironmentName + _elasticQueries.VacancyIndexLookupName, data);

            var elasticResponse = JsonConvert.DeserializeObject<ElasticResponse<IndexRegistryEntry>>(response.Body);

            if (elasticResponse?.Items != null && elasticResponse.Items.Any())
            {
                return elasticResponse.Items.First();
            }

            _logger.LogWarning("Searching failed. Could not find any reservation index names to search");

            return null;
        }

        private string GetReservationCountForProviderQuery(long providerId)
        {
            return _elasticQueries.GetVacancyCountQuery.Replace("{providerId}", providerId.ToString());
        }

        private string GetReservationsSearchString(
            ushort startingDocumentIndex, ushort pageItemCount, long providerId)
        {
            var query = _elasticQueries.GetAllVacanciesQuery.Replace("{startingDocumentIndex}", startingDocumentIndex.ToString());
            query = query.Replace("{providerId}", providerId.ToString());
            query = query.Replace("{pageItemCount}", pageItemCount.ToString());

            return query;
        }

        private string GetReservationsSearchString(
            ushort startingDocumentIndex, ushort pageItemCount, long providerId, string searchTerm)
        {
            var query = _elasticQueries.FindVacanciesQuery.Replace("{startingDocumentIndex}", startingDocumentIndex.ToString());
            query = query.Replace("{providerId}", providerId.ToString());
            query = query.Replace("{pageItemCount}", pageItemCount.ToString());
            query = query.Replace("{searchTerm}", searchTerm);

            return query;
        }

        private async Task<int> GetSearchResultCount(string reservationIndexName, long providerId)
        {
            var query = GetReservationCountForProviderQuery(providerId);

            var jsonResponse = await _client.CountAsync<StringResponse>(reservationIndexName,
                PostData.String(query));

            var result = JsonConvert.DeserializeObject<ElasticCountResponse>(jsonResponse.Body);

            return result.count;
        }
    }
}
