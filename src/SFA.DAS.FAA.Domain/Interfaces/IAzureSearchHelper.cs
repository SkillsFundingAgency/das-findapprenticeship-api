using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAzureSearchHelper
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
    Task<ApprenticeshipVacancyItem> Get(VacancyReference vacancyReference);
    Task<int> Count(FindVacanciesCountModel countModel);
    Task<List<ApprenticeshipSearchItem>> Get(List<VacancyReference> vacancyReferences);
    Task<string> GetIndexName(CancellationToken cancellationToken);
}
