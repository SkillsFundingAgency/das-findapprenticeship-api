using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class ApprenticeshipSearchResponse
    {
        public IEnumerable<ApprenticeshipSearchItem> ApprenticeshipVacancies { get; set; } = new List<ApprenticeshipSearchItem>();
        public int TotalApprenticeshipVacancies { get; set; }
    }
}
