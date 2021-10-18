namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IElasticSearchQueries
    {
        string FindVacanciesQuery { get; }
        string GetAllVacanciesQuery { get; }
        string GetVacanciesCountQuery { get; }
    }
}