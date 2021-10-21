namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IElasticSearchQueryBuilder
    {
        string BuildFindVacanciesQuery(int pageNumber, int pageSize, int? ukprn);
        string BuildGetVacanciesCountQuery();
        string BuildGetVacancyQuery(string vacancyReference);
    }
}