using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;

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
            var searchResult = await _vacancySearchRepository.Find(request.PageNumber, request.PageSize);

            return new SearchApprenticeshipVacanciesResult
            {
                ApprenticeshipVacancies = searchResult.ApprenticeshipVacancies,
                TotalFound = searchResult.TotalFound,
                Total = searchResult.Total
            };
        }
    }
}