using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAcsVacancySearchRepository
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
    Task<ApprenticeshipVacancyItem> Get(string vacancyReference);
    Task<int> Count(List<AdditionalDataSource> additionalDataSources);
}
