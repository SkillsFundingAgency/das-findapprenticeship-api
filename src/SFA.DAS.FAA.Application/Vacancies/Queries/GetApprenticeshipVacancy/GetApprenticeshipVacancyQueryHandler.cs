using System.Linq;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler(IAcsVacancySearchRepository acsVacancySearchRepository)
        : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyResult>
    {
        public async Task<GetApprenticeshipVacancyResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancy = await acsVacancySearchRepository.Get(request.VacancyReference);

            if (vacancy is not null)
                return new GetApprenticeshipVacancyResult
                {
                    ApprenticeshipVacancy = vacancy
                };
            
            var results = await acsVacancySearchRepository.GetAll([request.VacancyReference]);
            if (results is {Count: > 0})
            {
                return new GetApprenticeshipVacancyResult
                {
                    ApprenticeshipVacancy = ApprenticeshipVacancyItem.FromApprenticeshipSearchItem(results.FirstOrDefault())
                };
            }

            return new GetApprenticeshipVacancyResult
            {
                ApprenticeshipVacancy = null
            };
        }
    }
}