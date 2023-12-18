using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IRequestHandler<SearchApprenticeshipVacanciesQuery, SearchApprenticeshipVacanciesResult>
    {
        private readonly IVacancySearchRepository _vacancySearchRepository;
        private readonly IAcsVacancySearchRespository _acsVacancySearchRepository;

        public SearchApprenticeshipVacanciesQueryHandler(IVacancySearchRepository vacancySearchRepository,
            IAcsVacancySearchRespository acsVacancySearchRepository)
        {
            _vacancySearchRepository = vacancySearchRepository;
            _acsVacancySearchRepository = acsVacancySearchRepository;
        }
        
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
                VacancySort = request.VacancySort
            };

            ApprenticeshipSearchResponse searchResult;
            if (request.Source == "Elastic")
            {
                searchResult = await _vacancySearchRepository.Find(model);
            }
            else
            {
                searchResult = await _acsVacancySearchRepository.Find(model);
            }

            return new SearchApprenticeshipVacanciesResult
            {
                ApprenticeshipVacancies = searchResult.ApprenticeshipVacancies,
                TotalFound = searchResult.TotalFound,
                Total = searchResult.Total
            };
        }
    }
}