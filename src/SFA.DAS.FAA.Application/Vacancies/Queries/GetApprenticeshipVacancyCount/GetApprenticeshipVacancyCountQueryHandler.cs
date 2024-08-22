using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQueryHandler(IAcsVacancySearchRepository acsVacancySearchRepository)
        : IRequestHandler<GetApprenticeshipVacancyCountQuery, int>
    {
        public async Task<int> Handle(GetApprenticeshipVacancyCountQuery request, CancellationToken cancellationToken)
        {
            return await acsVacancySearchRepository.Count(request.AdditionalDataSources);
        }
    }
}