using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class IndexedVacancySearchResult
    {
        public IndexedVacancySearchResult()
        {
            Reservations = new VacancyIndex[0];
        }

        public IEnumerable<VacancyIndex> Reservations { get; set; }
        public uint TotalReservations { get; set; }
    }
}
