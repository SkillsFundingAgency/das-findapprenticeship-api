using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IVacancyIndexRepository
    {
        Task<bool> PingAsync();
        string GetCurrentApprenticeshipVacanciesIndex();
        Task<ApprenticeshipSearchResponse> Find(
            string searchTerm, 
            int pageNumber, 
            int pageSize);
    }
}
