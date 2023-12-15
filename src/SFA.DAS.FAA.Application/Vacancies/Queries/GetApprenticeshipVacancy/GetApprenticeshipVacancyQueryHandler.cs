using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyResult>
    {
        private readonly IVacancySearchRepository _vacancySearchRepository;
        private readonly IAcsVacancySearchRespository _acsVacancySearchRepository;

        public GetApprenticeshipVacancyQueryHandler(IVacancySearchRepository vacancySearchRepository,IAcsVacancySearchRespository acsVacancySearchRepository)
        {
            _vacancySearchRepository = vacancySearchRepository;
            _acsVacancySearchRepository = acsVacancySearchRepository;
        }
        
        public async Task<GetApprenticeshipVacancyResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancy = request.Source == SearchSource.Elastic
                ? await _vacancySearchRepository.Get(request.VacancyReference) 
                : await _acsVacancySearchRepository.Get(request.VacancyReference);

            return new GetApprenticeshipVacancyResult
            {
                ApprenticeshipVacancy = vacancy
            };
        }
    }
}