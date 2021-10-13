using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;

namespace SFA.DAS.FAA.Api.ApiResponses
{
    public class GetSearchApprenticeshipVacanciesResponse
    {
        public int Total { get; set; }
        public int TotalFound { get; set; }
        public IEnumerable<GetApprenticeshipVacancyResponse> ApprenticeshipVacancies { get; set; }

        public static implicit operator GetSearchApprenticeshipVacanciesResponse(SearchApprenticeshipVacanciesResult source)
        {
            return new GetSearchApprenticeshipVacanciesResponse
            {
                ApprenticeshipVacancies = source.ApprenticeshipVacancies.Select(item => (GetApprenticeshipVacancyResponse)item),
                TotalFound = source.TotalFound,
                Total = source.Total
            };
        }
    }
}