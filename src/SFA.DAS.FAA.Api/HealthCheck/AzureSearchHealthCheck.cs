using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Api.HealthCheck
{
    public class AzureSearchHealthCheck(IAcsVacancySearchRepository acsVacancySearchRepository) : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Azure search re-indexing health";

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await acsVacancySearchRepository.GetHealthCheckStatus(cancellationToken);

            return result == Domain.Models.HealthCheckResult.Healthy 
                ? HealthCheckResult.Healthy(HealthCheckResultDescription) 
                : HealthCheckResult.Degraded(HealthCheckResultDescription);
        }
    }
}