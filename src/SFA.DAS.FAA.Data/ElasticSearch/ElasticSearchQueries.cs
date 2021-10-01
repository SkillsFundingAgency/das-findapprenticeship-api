using System.IO;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.ElasticSearch
{
    public class ElasticSearchQueries : IElasticSearchQueries
    {
        public string VacancyIndexLookupName => "-vacancies-index-registry";
        public string LastIndexSearchQuery { get; }
        public string FindVacanciesQuery { get; }
        public string GetAllVacanciesQuery { get; }
        public string GetVacancyCountQuery { get; }

        public ElasticSearchQueries()
        {
            LastIndexSearchQuery = File.ReadAllText("ElasticSearch/LatestIndexSearchQuery.json");
            FindVacanciesQuery = File.ReadAllText("ElasticSearch/FindVacanciesQuery.json");
            GetAllVacanciesQuery = File.ReadAllText("ElasticSearch/GetAllVacanciesQuery.json");
            GetVacancyCountQuery = File.ReadAllText("ElasticSearch/GetVacancyCountQuery.json");
        }
    }
}
