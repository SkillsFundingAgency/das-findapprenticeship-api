using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAzureSearchHelper
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
    Task<ApprenticeshipVacancyItem> Get(string vacancyReference);
    Task<int> Count(List<AdditionalDataSource> additionalDataSources);
    Task<List<ApprenticeshipSearchItem>> Get(List<string> vacancyReferences);
    Task<string> GetIndexName(CancellationToken cancellationToken);
}
