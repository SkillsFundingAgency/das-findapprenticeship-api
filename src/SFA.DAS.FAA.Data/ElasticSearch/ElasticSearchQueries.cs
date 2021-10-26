using System.IO;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.ElasticSearch
{
    public class ElasticSearchQueries : IElasticSearchQueries
    {
        public string FindVacanciesQuery { get; }
        public string GetVacanciesCountQuery { get; }
        public string GetVacancyQuery { get; }

        public ElasticSearchQueries()
        {
            FindVacanciesQuery = File.ReadAllText("ElasticSearch/FindVacanciesQuery.json");
            GetVacanciesCountQuery = File.ReadAllText("ElasticSearch/GetVacanciesCountQuery.json");
            GetVacancyQuery = File.ReadAllText("ElasticSearch/GetVacancyQuery.json");
        }
    }
}
