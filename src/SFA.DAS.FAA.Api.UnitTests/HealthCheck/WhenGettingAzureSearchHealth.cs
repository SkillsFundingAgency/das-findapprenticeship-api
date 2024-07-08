using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Api.HealthCheck;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.UnitTests.HealthCheck
{
    public class WhenGettingAzureSearchHealth
    {
        [Test, MoqAutoData]
        public async Task Then_Healthy_Returned_If_Returns_Healthy_From_Repository(
            HealthCheckContext context,
            [Frozen] Mock<IAcsVacancySearchRepository> acsVacancySearchRepository,
            AzureSearchHealthCheck healthCheck)
        {
            acsVacancySearchRepository.Setup(x => x.GetHealthCheckStatus(CancellationToken.None))
                .ReturnsAsync(Domain.Models.HealthCheckResult.Healthy);

            var actual = await healthCheck.CheckHealthAsync(context);

            actual.Status.Should().Be(HealthStatus.Healthy);
        }
        [Test, MoqAutoData]
        public async Task Then_Degraded_Returned_If_Returns_Degraded_From_Repository(
            HealthCheckContext context,
            [Frozen] Mock<IAcsVacancySearchRepository> acsVacancySearchRepository,
            AzureSearchHealthCheck healthCheck)
        {
            acsVacancySearchRepository.Setup(x => x.GetHealthCheckStatus(CancellationToken.None))
                .ReturnsAsync(Domain.Models.HealthCheckResult.Degraded);

            var actual = await healthCheck.CheckHealthAsync(context);

            actual.Status.Should().Be(HealthStatus.Degraded);
        }
    }
}