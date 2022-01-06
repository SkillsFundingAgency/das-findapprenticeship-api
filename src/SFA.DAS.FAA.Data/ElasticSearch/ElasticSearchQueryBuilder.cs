using System.Collections.Generic;
using SFA.DAS.FAA.Data.Extensions;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.ElasticSearch
{
    public class ElasticSearchQueryBuilder : IElasticSearchQueryBuilder
    {
        private readonly IElasticSearchQueries _elasticSearchQueries;

        public ElasticSearchQueryBuilder(IElasticSearchQueries elasticSearchQueries)
        {
            _elasticSearchQueries = elasticSearchQueries;
        }
        
        public string BuildFindVacanciesQuery(FindVacanciesModel findVacanciesModel)
        {
            var startingDocumentIndex = findVacanciesModel.PageNumber < 2 ? 0 : (findVacanciesModel.PageNumber - 1) * findVacanciesModel.PageSize;
            var mustConditions = BuildMustConditions(findVacanciesModel);
            var sort = BuildSort(findVacanciesModel);
            var distanceFilter = BuildDistanceFilter(findVacanciesModel);
            var parameters = new Dictionary<string, object>
            {
                {"pageSize", findVacanciesModel.PageSize},
                {nameof(startingDocumentIndex), startingDocumentIndex},
                {nameof(mustConditions), mustConditions},
                {nameof(sort), sort},
                {nameof(distanceFilter), distanceFilter}
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
        
        private string BuildMustConditions(FindVacanciesModel findVacanciesModel)
        {
            var filters = string.Empty;
            if (findVacanciesModel.Ukprn.HasValue)
            {
                filters += @$"{{ ""term"": {{ ""{nameof(findVacanciesModel.Ukprn)}"": ""{findVacanciesModel.Ukprn}"" }}}}";
            }
            if (!string.IsNullOrEmpty(findVacanciesModel.AccountPublicHashedId))
            {
                filters += @$"{AddFilterSeparator(filters)}{{ ""term"": {{ ""{nameof(findVacanciesModel.AccountPublicHashedId)}"": ""{findVacanciesModel.AccountPublicHashedId}"" }}}}";
            }
            if (!string.IsNullOrEmpty(findVacanciesModel.AccountLegalEntityPublicHashedId))
            {
                filters += @$"{AddFilterSeparator(filters)}{{ ""term"": {{ ""{nameof(findVacanciesModel.AccountLegalEntityPublicHashedId)}"": ""{findVacanciesModel.AccountLegalEntityPublicHashedId}"" }}}}";
            }
            if (findVacanciesModel.StandardLarsCode.HasValue)
            {
                filters += @$"{AddFilterSeparator(filters)}{{ ""term"": {{ ""{nameof(findVacanciesModel.StandardLarsCode)}"": ""{findVacanciesModel.StandardLarsCode}"" }}}}";
            }
            if (!string.IsNullOrEmpty(findVacanciesModel.Route))
            {
                filters += @$"{AddFilterSeparator(filters)}{{ ""term"": {{ ""{nameof(findVacanciesModel.Route)}"": ""{findVacanciesModel.Route}"" }}}}";
            }
            return filters;
        }

        private static string AddFilterSeparator(string filters)
        {
            if (!string.IsNullOrEmpty(filters))
            {
                return ", ";
            }

            return "";
        }

        private string BuildSort(FindVacanciesModel model)
        {
            switch (model.VacancySort)
            {
                case VacancySort.AgeAsc:
                    return @" { ""postedDate"" : { ""order"" : ""asc"" } }";
                case VacancySort.AgeDesc:
                    return @" { ""postedDate"" : { ""order"" : ""desc"" } }";
                case VacancySort.ExpectedStartDateAsc:
                    return @" { ""startDate"" : { ""order"" : ""asc"" } }";
                case VacancySort.ExpectedStartDateDesc:
                    return @" { ""startDate"" : { ""order"" : ""desc"" } }";
                case VacancySort.DistanceAsc:
                    return !model.Lat.HasValue || !model.Lon.HasValue ? "" : @$" {{ ""_geo_distance"" : {{ ""location"" : {{ ""lat"" : {model.Lat}, ""lon"" : {model.Lon} }}, ""order"" : ""asc"", ""unit"" :""mi"" }} }}";
                case VacancySort.DistanceDesc:
                    return !model.Lat.HasValue || !model.Lon.HasValue ? "" : @$" {{ ""_geo_distance"" : {{ ""location"" : {{ ""lat"" : {model.Lat}, ""lon"" : {model.Lon} }}, ""order"" : ""desc"", ""unit"" :""mi"" }} }}";
            }
            
            return "";
        }

        private string BuildDistanceFilter(FindVacanciesModel findVacanciesModel)
        {
            if (!findVacanciesModel.Lat.HasValue || !findVacanciesModel.Lon.HasValue ||
                !findVacanciesModel.DistanceInMiles.HasValue)
            {
                return "";
            }
            
            return $@",""filter"": {{ ""geo_distance"": {{ ""distance"": ""{findVacanciesModel.DistanceInMiles}miles"", ""location"": {{ ""lat"": {findVacanciesModel.Lat}, ""lon"": {findVacanciesModel.Lon} }} }} }}";
        }
    }
}