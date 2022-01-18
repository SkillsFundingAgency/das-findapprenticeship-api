using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IElasticSearchQueryBuilder
    {
        string BuildFindVacanciesQuery(FindVacanciesModel findVacanciesModel);
        string BuildGetVacanciesCountQuery();
        string BuildGetVacancyQuery(string vacancyReference);
    }
}