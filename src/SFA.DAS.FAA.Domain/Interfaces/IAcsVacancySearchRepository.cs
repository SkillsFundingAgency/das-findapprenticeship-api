using System.Collections.Generic;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Common.Domain.Models;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAcsVacancySearchRepository
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
    Task<ApprenticeshipVacancyItem> Get(VacancyReference vacancyReference);
    Task<List<ApprenticeshipSearchItem>> GetAll(List<VacancyReference> vacancyReferences);
    Task<int> Count(FindVacanciesCountModel findVacanciesCountModel);
    Task<HealthCheckResult> GetHealthCheckStatus(CancellationToken cancellationToken);
}
