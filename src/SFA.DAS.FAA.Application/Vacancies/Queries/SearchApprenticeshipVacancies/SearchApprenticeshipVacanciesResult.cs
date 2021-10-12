using System.Collections.Generic;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesResult
    {
        public IEnumerable<ApprenticeshipSearchItem> ApprenticeshipVacancies { get; set; }
        public int TotalFound { get; set; }
        public int Total { get; set; }
    }
}