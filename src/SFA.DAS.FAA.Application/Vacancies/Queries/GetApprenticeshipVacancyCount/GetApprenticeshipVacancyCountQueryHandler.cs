using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQueryHandler : IRequestHandler<GetApprenticeshipVacancyCountQuery, int>
    {
        private readonly IVacancySearchRepository _vacancySearchRepository;

        public GetApprenticeshipVacancyCountQueryHandler(IVacancySearchRepository vacancySearchRepository)
        {
            _vacancySearchRepository = vacancySearchRepository;
        }
        public async Task<int> Handle(GetApprenticeshipVacancyCountQuery request, CancellationToken cancellationToken)
        {
            return await _vacancySearchRepository.Count();
        }
    }
}