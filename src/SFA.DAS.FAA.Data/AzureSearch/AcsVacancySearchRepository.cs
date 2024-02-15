using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

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

    public async Task<int> Count()
    {
        return await _searchHelper.Count();
    }
}
