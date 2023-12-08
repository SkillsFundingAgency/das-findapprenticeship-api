using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAzureSearchHelper
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
}
