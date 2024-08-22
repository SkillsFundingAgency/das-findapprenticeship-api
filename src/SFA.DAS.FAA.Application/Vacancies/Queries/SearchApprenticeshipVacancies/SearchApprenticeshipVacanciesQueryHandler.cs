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
                SearchTerm = request.SearchTerm,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Ukprn = request.Ukprn,
                AccountPublicHashedId = request.AccountPublicHashedId,
                AccountLegalEntityPublicHashedId = request.AccountLegalEntityPublicHashedId,
                StandardLarsCode = request.StandardLarsCode,
                Categories = request.Categories,
                Lat = request.Lat,
                Lon = request.Lon,
                DistanceInMiles = request.DistanceInMiles,
                NationWideOnly = request.NationWideOnly,
                PostedInLastNumberOfDays = request.PostedInLastNumberOfDays,
                VacancySort = request.VacancySort,
                Levels = request.Levels,
                DisabilityConfident = request.DisabilityConfident,
                AdditionalDataSources = request.AdditionalDataSources
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