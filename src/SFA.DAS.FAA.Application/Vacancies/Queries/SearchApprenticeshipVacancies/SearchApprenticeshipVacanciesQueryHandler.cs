using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IRequestHandler<SearchApprenticeshipVacanciesQuery, SearchApprenticeshipVacanciesResult>
    {
        private readonly IVacancySearchRepository _vacancySearchRepository;

        public SearchApprenticeshipVacanciesQueryHandler(IVacancySearchRepository vacancySearchRepository)
        {
            _vacancySearchRepository = vacancySearchRepository;
        }
        
        public async Task<SearchApprenticeshipVacanciesResult> Handle(SearchApprenticeshipVacanciesQuery request, CancellationToken cancellationToken)
        {
            var searchResult = await _vacancySearchRepository.Find(new FindVacanciesModel
            {
                PageNumber = request.PageNumber, 
                PageSize = request.PageSize, 
                Ukprn = request.Ukprn, 
                AccountPublicHashedId = request.AccountPublicHashedId, 
                AccountLegalEntityPublicHashedId =request.AccountLegalEntityPublicHashedId,
                LarsCode = request.LarsCode,
                Categories = request.Categories,
                Lat = request.Lat,
                Lon =request.Lon,
                DistanceInMiles = request.DistanceInMiles,
                NationWideOnly = request.NationWideOnly,
                PostedInLastNumberOfDays = request.PostedInLastNumberOfDays,
                VacancySort = request.VacancySort
            });

            return new SearchApprenticeshipVacanciesResult
            {
                ApprenticeshipVacancies = searchResult.ApprenticeshipVacancies,
                TotalFound = searchResult.TotalFound,
                Total = searchResult.Total
            };
        }
    }
}