using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IRequestHandler<SearchApprenticeshipVacanciesQuery, SearchApprenticeshipVacanciesResult>
    {
        private readonly IVacancyIndexRepository _vacancyIndexRepository;

        public SearchApprenticeshipVacanciesQueryHandler(IVacancyIndexRepository vacancyIndexRepository)
        {
            _vacancyIndexRepository = vacancyIndexRepository;
        }
        
        public async Task<SearchApprenticeshipVacanciesResult> Handle(SearchApprenticeshipVacanciesQuery request, CancellationToken cancellationToken)
        {
            var searchResult = await _vacancyIndexRepository.Find(request.SearchTerm, request.PageNumber, request.PageSize);

            return new SearchApprenticeshipVacanciesResult
            {
                ApprenticeshipVacancies = searchResult.ApprenticeshipVacancies,
                TotalFound = searchResult.TotalApprenticeshipVacancies
            };
        }
    }
}