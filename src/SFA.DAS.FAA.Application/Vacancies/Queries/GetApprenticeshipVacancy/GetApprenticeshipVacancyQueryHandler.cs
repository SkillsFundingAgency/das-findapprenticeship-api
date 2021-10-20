using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyResult>
    {
        private readonly IVacancySearchRepository _vacancySearchRepository;

        public GetApprenticeshipVacancyQueryHandler(IVacancySearchRepository vacancySearchRepository)
        {
            _vacancySearchRepository = vacancySearchRepository;
        }
        
        public async Task<GetApprenticeshipVacancyResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancy = await _vacancySearchRepository.Get(request.VacancyReference);

            return new GetApprenticeshipVacancyResult
            {
                ApprenticeshipVacancy = vacancy
            };
        }
    }
}