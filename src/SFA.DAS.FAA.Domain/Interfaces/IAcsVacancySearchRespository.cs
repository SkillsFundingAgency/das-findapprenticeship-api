using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAcsVacancySearchRespository
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
}
