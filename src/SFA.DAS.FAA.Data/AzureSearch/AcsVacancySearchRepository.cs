using Microsoft.Extensions.Logging;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Data.AzureSearch;
public class AcsVacancySearchRepository : IAcsVacancySearchRepository
{
    private readonly ILogger<AcsVacancySearchRepository> _logger;
    private readonly IAzureSearchHelper _searchHelper;

    public AcsVacancySearchRepository(ILogger<AcsVacancySearchRepository> logger, IAzureSearchHelper searchHelper)
    {
        _logger = logger;
        _searchHelper = searchHelper;
    }

    public async Task<ApprenticeshipSearchResponse> Find(FindVacanciesModel findVacanciesModel)
    {
        _logger.LogInformation("Starting vacancy search");
        return await _searchHelper.Find(findVacanciesModel);
    }

    public async Task<ApprenticeshipVacancyItem> Get(string vacancyReference)
    {
        return await _searchHelper.Get(vacancyReference);
    }

    public async Task<int> Count(List<AdditionalDataSource> additionalDataSources)
    {
        return await _searchHelper.Count(additionalDataSources);
    }

    public async Task<HealthCheckResult> GetHealthCheckStatus(CancellationToken cancellationToken)
    {
        try
        {
            var indexName = await _searchHelper.GetIndexName(cancellationToken);
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
            _logger.LogError("Unable to communicate with Azure search. Details: {details}", ex.Message);
            return HealthCheckResult.Degraded;
        }

        return HealthCheckResult.Healthy;
    }
}