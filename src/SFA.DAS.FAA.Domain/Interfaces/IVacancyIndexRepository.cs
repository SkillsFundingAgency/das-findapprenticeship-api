using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IVacancyIndexRepository
    {
        Task<bool> PingAsync();
        string GetCurrentApprenticeshipVacanciesIndex();
        Task<IndexedVacancySearchResult> Find(
            long providerId, string searchTerm, ushort pageNumber, 
            ushort pageItemCount);
    }
}
