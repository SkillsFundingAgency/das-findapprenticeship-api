using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler(IAcsVacancySearchRepository acsVacancySearchRepository)
        : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyResult>
    {
        public async Task<GetApprenticeshipVacancyResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancy = await acsVacancySearchRepository.Get(request.VacancyReference);

            return new GetApprenticeshipVacancyResult
            {
                ApprenticeshipVacancy = vacancy
            };
        }
    }
}