﻿using System.Collections.Generic;
using SFA.DAS.FAA.Data.Extensions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.ElasticSearch
{
    public class ElasticSearchQueryBuilder : IElasticSearchQueryBuilder
    {
        private readonly IElasticSearchQueries _elasticSearchQueries;

        public ElasticSearchQueryBuilder(IElasticSearchQueries elasticSearchQueries)
        {
            _elasticSearchQueries = elasticSearchQueries;
        }
        
        public string BuildFindVacanciesQuery(int pageNumber, int pageSize, int? ukprn, string accountPublicHashedId = null)
        {
            var startingDocumentIndex = pageNumber < 2 ? 0 : (pageNumber - 1) * pageSize;
            var mustConditions = BuildMustConditions(ukprn, accountPublicHashedId);
            var parameters = new Dictionary<string, object>
            {
                {nameof(pageSize), pageSize},
                {nameof(startingDocumentIndex), startingDocumentIndex},
                {nameof(mustConditions), mustConditions}
            };
            
            var query = _elasticSearchQueries.FindVacanciesQuery.ReplaceParameters(parameters);

            return query;
        }

        public string BuildGetVacanciesCountQuery()
        {
            return _elasticSearchQueries.GetVacanciesCountQuery;
        }

        public string BuildGetVacancyQuery(string vacancyReference)
        {
            return _elasticSearchQueries.GetVacancyQuery;
        }
        
        private string BuildMustConditions(int? ukprn, string accountPublicHashedId)
        {
            var filters = string.Empty;
            if (ukprn.HasValue)
                filters += @$"{{ ""term"": {{ ""{nameof(ukprn)}"": ""{ukprn}"" }}";
            if (!string.IsNullOrEmpty(accountPublicHashedId))
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    filters += ", ";
                }
                filters += @$"{{ ""term"": {{ ""{nameof(accountPublicHashedId)}"": ""{accountPublicHashedId}"" }}";
            }
            return filters;
        }
    }
}