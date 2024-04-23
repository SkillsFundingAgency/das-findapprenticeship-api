using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQueryHandler : IRequestHandler<GetApprenticeshipVacancyCountQuery, int>
    {
        private readonly IVacancySearchRepository _vacancySearchRepository;
        private readonly IAcsVacancySearchRepository _acsVacancySearchRepository;

        public GetApprenticeshipVacancyCountQueryHandler(IVacancySearchRepository vacancySearchRepository, IAcsVacancySearchRepository acsVacancySearchRepository)
        {
            _vacancySearchRepository = vacancySearchRepository;
            _acsVacancySearchRepository = acsVacancySearchRepository;
        }
        public async Task<int> Handle(GetApprenticeshipVacancyCountQuery request, CancellationToken cancellationToken)
        {
            if (request.Source == SearchSource.Elastic)
            {
                return await _vacancySearchRepository.Count();
            }
            else
            {
                return await _acsVacancySearchRepository.Count(request.AdditionalDataSources);
            }
        }
    }
}