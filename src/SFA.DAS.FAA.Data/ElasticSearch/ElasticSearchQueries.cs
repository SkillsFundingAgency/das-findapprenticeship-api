using System.IO;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.ElasticSearch
{
    public class ElasticSearchQueries : IElasticSearchQueries
    {
        public string FindVacanciesQuery { get; }
        public string GetAllVacanciesQuery { get; }
        public string GetVacancyCountQuery { get; }

        public ElasticSearchQueries()
        {
            FindVacanciesQuery = File.ReadAllText("ElasticSearch/FindVacanciesQuery.json");
            GetAllVacanciesQuery = File.ReadAllText("ElasticSearch/GetAllVacanciesQuery.json");
            GetVacancyCountQuery = File.ReadAllText("ElasticSearch/GetVacanciesCountQuery.json");
        }
    }
}
