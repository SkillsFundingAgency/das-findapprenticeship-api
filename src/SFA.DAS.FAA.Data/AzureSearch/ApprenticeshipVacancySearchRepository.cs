﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FAA.Data.ElasticSearch;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.AzureSearch
{
    public class ApprenticeshipVacancySearchRepository : IVacancySearchRepository
    {
        private readonly IElasticLowLevelClient _client;
        private readonly ElasticEnvironment _environment;
        private readonly IElasticSearchQueryBuilder _queryBuilder;
        private readonly ILogger<ApprenticeshipVacancySearchRepository> _logger;
        private const string IndexName = "-faa-apprenticeships";

        public ApprenticeshipVacancySearchRepository(
            IElasticLowLevelClient client, 
            ElasticEnvironment environment, 
            IElasticSearchQueryBuilder queryBuilder, 
            ILogger<ApprenticeshipVacancySearchRepository> logger)
        {
            _client = client;
            _environment = environment;
            _queryBuilder = queryBuilder;
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

        public async Task<ApprenticeshipVacancyItem> Get(string vacancyReference)
        {
            _logger.LogInformation($"Starting get vacancy [{vacancyReference}]");
            
            var query = _queryBuilder.BuildGetVacancyQuery(vacancyReference);
            var jsonResponse = await _client.SearchAsync<StringResponse>(ApprenticeshipVacanciesIndex, PostData.String(query));
            var responseBody = JsonConvert.DeserializeObject<ElasticResponse<ApprenticeshipVacancyItem>>(jsonResponse.Body);

			if (responseBody.hits is { total: not null })
				_logger.LogInformation(
					$"Found [{responseBody.hits.total.value}] hits for vacancy [{vacancyReference}]");

			var apprenticeshipVacancyItem = responseBody.Items.SingleOrDefault()?._source;
            return apprenticeshipVacancyItem;
        }

        public async Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel)
        {
            _logger.LogInformation("Starting vacancy search");
            
            var query = _queryBuilder.BuildFindVacanciesQuery(findVacanciesModel);
            var jsonResponse = await _client.SearchAsync<StringResponse>(ApprenticeshipVacanciesIndex, PostData.String(query));
            var responseBody = JsonConvert.DeserializeObject<ElasticResponse<ApprenticeshipSearchItem>>(jsonResponse.Body);

            if (responseBody == null)
            {
                _logger.LogWarning("Searching failed. Elastic search response could not be de-serialised");
                return new ApprenticeshipSearchResponse();
            }

            _logger.LogDebug("Searching complete, returning search results");

            var totalRecordCount = await GetTotal();

            var apprenticeshipSearchItems = responseBody.Items;

            var searchItems = new List<ApprenticeshipSearchItem>();

            var isDistanceSort = findVacanciesModel.VacancySort == VacancySort.DistanceAsc ||
                                 findVacanciesModel.VacancySort == VacancySort.DistanceDesc;
            
            if (findVacanciesModel.Lat == null || findVacanciesModel.Lon == null 
                                               || findVacanciesModel.DistanceInMiles == null || !isDistanceSort)
            {
                searchItems = apprenticeshipSearchItems.Select(c=>c._source).ToList();
            }
            else
            {
                foreach (var apprenticeshipSearchItem in apprenticeshipSearchItems)
                {
                    var searchItem = apprenticeshipSearchItem._source;
                    searchItem.Distance = apprenticeshipSearchItem.sort?.FirstOrDefault();  
                    searchItems.Add(searchItem);
                }    
            }
            
            
            var searchResult =  new ApprenticeshipSearchResponse
            {
               ApprenticeshipVacancies = searchItems,
               TotalFound = responseBody.hits?.total?.value ?? 0,
               Total = totalRecordCount
            };

            return searchResult;
        }

        public Task<int> Count()
        {
            return GetTotal();
        }

        private async Task<int> GetTotal()
        {
            var query = _queryBuilder.BuildGetVacanciesCountQuery();
            var jsonResponse = await _client.CountAsync<StringResponse>(ApprenticeshipVacanciesIndex, PostData.String(query));
            var result = JsonConvert.DeserializeObject<ElasticCountResponse>(jsonResponse.Body);

            return result.count;
        }
    }
}
