using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler(IAcsVacancySearchRepository acsVacancySearchRepository)
        : IRequestHandler<SearchApprenticeshipVacanciesQuery, SearchApprenticeshipVacanciesResult>
    {
        public async Task<SearchApprenticeshipVacanciesResult> Handle(SearchApprenticeshipVacanciesQuery request, CancellationToken cancellationToken)
        {
            var model = new FindVacanciesModel
            {
                AccountLegalEntityPublicHashedId = request.AccountLegalEntityPublicHashedId,
                AccountPublicHashedId = request.AccountPublicHashedId,
                ApprenticeshipTypes = request.ApprenticeshipTypes,
                EmployerName = request.EmployerName,
                AdditionalDataSources = request.AdditionalDataSources,
                Categories = request.Categories,
                DisabilityConfident = request.DisabilityConfident,
                DistanceInMiles = request.DistanceInMiles,
                Lat = request.Lat,
                Levels = request.Levels,
                Lon = request.Lon,
                ExcludeNational = request.ExcludeNational,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                PostedInLastNumberOfDays = request.PostedInLastNumberOfDays,
                RouteIds = request.RouteIds,
                SearchTerm = request.SearchTerm,
                SkipWageType = request.SkipWageType,
                StandardLarsCode = request.StandardLarsCode,
                Ukprn = request.Ukprn,
                VacancySort = request.VacancySort,
            };

            var searchResult = await acsVacancySearchRepository.Find(model);

            return new SearchApprenticeshipVacanciesResult
            {
                ApprenticeshipVacancies = searchResult.ApprenticeshipVacancies,
                TotalFound = searchResult.TotalFound,
                Total = searchResult.Total
            };
        }
    }
}