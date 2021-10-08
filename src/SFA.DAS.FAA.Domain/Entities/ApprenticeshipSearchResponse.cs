using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class ApprenticeshipSearchResponse
    {
        public ApprenticeshipSearchResponse()
        {
            ApprenticeshipVacancies = new ApprenticeshipSearchItem[0];
        }

        public IEnumerable<ApprenticeshipSearchItem> ApprenticeshipVacancies { get; set; }
        public uint TotalApprenticeshipVacancies { get; set; }
    }
}
