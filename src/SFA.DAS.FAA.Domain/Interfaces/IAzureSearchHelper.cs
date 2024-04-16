using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAzureSearchHelper
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
    Task<ApprenticeshipVacancyItem> Get(string vacancyReference);
    Task<int> Count();
    Task<List<ApprenticeshipSearchItem>> Get(List<string> vacancyReferences);
}
