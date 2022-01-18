﻿using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IVacancySearchRepository
    {
        Task<bool> PingAsync();
        Task<ApprenticeshipSearchItem> Get(string vacancyReference);
        Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
    }
}
