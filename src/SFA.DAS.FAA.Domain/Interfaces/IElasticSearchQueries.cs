﻿namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IElasticSearchQueries
    {
        string VacancyIndexLookupName { get; }
        string FindVacanciesQuery { get; }
        string GetAllVacanciesQuery { get; }
        string GetVacancyCountQuery { get; }
    }
}