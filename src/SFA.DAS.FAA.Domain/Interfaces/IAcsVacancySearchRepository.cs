﻿using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IAcsVacancySearchRepository
{
    Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel);
    Task<ApprenticeshipVacancyItem> Get(string vacancyReference);
    Task<int> Count(FindVacanciesCountModel findVacanciesCountModel);
    Task<HealthCheckResult> GetHealthCheckStatus(CancellationToken cancellationToken);
}
