using Microsoft.Extensions.Logging;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Data.AzureSearch;
public class AcsVacancySearchRepository(ILogger<AcsVacancySearchRepository> logger, IAzureSearchHelper searchHelper)
    : IAcsVacancySearchRepository
{
    public async Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel)
    {
        logger.LogInformation("Starting vacancy search");
        return await searchHelper.Find(findVacanciesModel);
    }

    public async Task<ApprenticeshipVacancyItem> Get(VacancyReference vacancyReference)
    {
        return await searchHelper.Get(vacancyReference);
    }

    public async Task<List<ApprenticeshipSearchItem>> GetAll(List<VacancyReference> vacancyReferences)
    {
        return (await searchHelper.Get(vacancyReferences)).Select(vacancyItem => (ApprenticeshipSearchItem)vacancyItem).ToList();
    }

    public async Task<int> Count(FindVacanciesCountModel findVacanciesCountModel)
    {
        return await searchHelper.Count(findVacanciesCountModel);
    }

    public async Task<HealthCheckResult> GetHealthCheckStatus(CancellationToken cancellationToken)
    {
        try
        {
            var indexName = await searchHelper.GetIndexName(cancellationToken);

            var indexCreatedDateTime = DateTime.ParseExact(
                indexName.Replace($"{AzureSearchIndex.IndexName}-", string.Empty),
                "yyyy-MM-dd-HH-mm",
                CultureInfo.InvariantCulture);

            if (indexCreatedDateTime < DateTime.UtcNow.AddHours(-1))
            {
                return HealthCheckResult.Degraded;
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to communicate with Azure search. Details: {details}", ex.Message);
            return HealthCheckResult.Degraded;
        }

        return HealthCheckResult.Healthy;
    }
}