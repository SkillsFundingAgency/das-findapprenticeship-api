using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;

namespace SFA.DAS.FAA.Api.ApiResponses;

public class GetSearchApprenticeshipVacanciesDetailsResponse
{
    public int Total { get; set; }
    public int TotalFound { get; set; }
    public IEnumerable<GetApprenticeshipVacancyDetailResponse> ApprenticeshipVacancies { get; set; }

    public static implicit operator GetSearchApprenticeshipVacanciesDetailsResponse(SearchApprenticeshipVacanciesResult source)
    {
        return new GetSearchApprenticeshipVacanciesDetailsResponse
        {
            ApprenticeshipVacancies = source.ApprenticeshipVacanciesWithDetails.Select(GetApprenticeshipVacancyDetailResponse.From),
            TotalFound = source.TotalFound,
            Total = source.Total
        };
    }
}