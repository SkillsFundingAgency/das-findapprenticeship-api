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
    public class ApprenticeshipVacancySearchRepository : IVacancyIndexRepository
    {
        private readonly IElasticLowLevelClient _client;
        private readonly FindApprenticeshipsApiEnvironment _environment;
        private readonly IElasticSearchQueries _elasticQueries;
        private readonly ILogger<ApprenticeshipVacancySearchRepository> _logger;
        private const string IndexName = "-faa-apprenticeships";

        public ApprenticeshipVacancySearchRepository(IElasticLowLevelClient client, FindApprenticeshipsApiEnvironment environment, IElasticSearchQueries elasticQueries, ILogger<ApprenticeshipVacancySearchRepository> logger)
        {
            _client = client;
            _environment = environment;
            _elasticQueries = elasticQueries;
            _logger = logger;
        }

        public string GetCurrentApprenticeshipVacanciesIndex() => _environment.EnvironmentName + IndexName;
        
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

        public async Task<IndexedVacancySearchResult> Find(
            long providerId, string searchTerm, ushort pageNumber, ushort pageItemCount)
        {
            _logger.LogInformation("Starting reservation search");

            var reservationIndex = GetCurrentApprenticeshipVacanciesIndex();

            if (string.IsNullOrWhiteSpace(reservationIndex))
            {
                _logger.LogWarning("Searching failed. Latest Reservation index does not have a name value");

                return new IndexedVacancySearchResult();
            }

            var startingDocumentIndex = (ushort) (pageNumber < 2 ? 0 : (pageNumber - 1) * pageItemCount);

            var elasticSearchResult = await GetSearchResult(
                providerId, searchTerm, pageItemCount, startingDocumentIndex, reservationIndex);

            if (elasticSearchResult == null)
            {
                _logger.LogWarning("Searching failed. Elastic search response could not be de-serialised");
                return new IndexedVacancySearchResult();
            }

            _logger.LogDebug("Searching complete, returning search results");

            var totalRecordCount = await GetSearchResultCount(reservationIndex, providerId);
            
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
