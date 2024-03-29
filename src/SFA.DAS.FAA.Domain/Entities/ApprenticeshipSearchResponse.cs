﻿using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class ApprenticeshipSearchResponse
    {
        public List<ApprenticeshipSearchItem> ApprenticeshipVacancies { get; set; } = new List<ApprenticeshipSearchItem>();
        public int TotalFound { get; set; }
        public int Total { get; set; }
    }
}
