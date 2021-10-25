using System.Collections.Generic;
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
        
        public string BuildFindVacanciesQuery(int pageNumber, int pageSize, int? ukprn, string accountPublicHashedId = null, string accountLegalEntityPublicHashedId = null)
        {
            var startingDocumentIndex = pageNumber < 2 ? 0 : (pageNumber - 1) * pageSize;
            var mustConditions = BuildMustConditions(ukprn, accountPublicHashedId, accountLegalEntityPublicHashedId);
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
            var parameters = new Dictionary<string, object>
            {
                {nameof(vacancyReference), vacancyReference}
            };
            return _elasticSearchQueries.GetVacancyQuery.ReplaceParameters(parameters);
        }
        
        private string BuildMustConditions(int? ukprn, string accountPublicHashedId, string accountLegalEntityPublicHashedId)
        {
            var filters = string.Empty;
            if (ukprn.HasValue)
            {
                filters += @$"{{ ""term"": {{ ""{nameof(ukprn)}"": ""{ukprn}"" }}}}";
            }
            if (!string.IsNullOrEmpty(accountPublicHashedId))
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    filters += ", ";
                }
                filters += @$"{{ ""term"": {{ ""{nameof(accountPublicHashedId)}"": ""{accountPublicHashedId}"" }}}}";
            }
            if (!string.IsNullOrEmpty(accountLegalEntityPublicHashedId))
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    filters += ", ";
                }
                filters += @$"{{ ""term"": {{ ""{nameof(accountLegalEntityPublicHashedId)}"": ""{accountLegalEntityPublicHashedId}"" }}}}";
            }
            return filters;
        }
    }
}