﻿using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IVacancySearchRepository
    {
        Task<bool> PingAsync();
        Task<ApprenticeshipSearchItem> Get(string vacancyReference);
        Task<ApprenticeshipSearchResponse> Find(
            string searchTerm,
            int pageNumber, 
            int pageSize, 
            int? ukprn = null);
    }
}
