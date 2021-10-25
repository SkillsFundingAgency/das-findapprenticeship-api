namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IElasticSearchQueryBuilder
    {
        string BuildFindVacanciesQuery(int pageNumber, int pageSize, int? ukprn, string accountPublicHashedId = null, string accountLegalEntityPublicHashedId = null);
        string BuildGetVacanciesCountQuery();
        string BuildGetVacancyQuery(string vacancyReference);
    }
}