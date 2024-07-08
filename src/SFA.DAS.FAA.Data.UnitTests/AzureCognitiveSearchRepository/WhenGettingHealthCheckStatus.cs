using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.FAA.Data.AzureSearch;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Data.UnitTests.AzureCognitiveSearchRepository
{
    [TestFixture]
    public class WhenGettingHealthCheckStatus
    {
        [Test]
        [MoqInlineAutoData(0, HealthCheckResult.Healthy)]
        [MoqInlineAutoData(-30, HealthCheckResult.Healthy)]
        [MoqInlineAutoData(-59, HealthCheckResult.Healthy)]
        [MoqInlineAutoData(-180, HealthCheckResult.UnHealthy)]
        [MoqInlineAutoData(-61, HealthCheckResult.UnHealthy)]
        public async Task Then_Returns_HealthCheckResult(
            double minutes,
            HealthCheckResult expectedResult,
            [Frozen] Mock<ILogger<AcsVacancySearchRepository>> logger,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            AcsVacancySearchRepository sut)
        {
            var mockResponse = $"{AzureSearchIndex.IndexName}-{DateTime.UtcNow.AddMinutes(minutes):yyyy-MM-dd-HH-mm}";
            azureSearchHelper.Setup(x => x.GetIndexName(CancellationToken.None))
                .ReturnsAsync(mockResponse);

            var actual = await sut.GetHealthCheckStatus(CancellationToken.None);

            using (new AssertionScope())
            {
                actual.Should().Be(expectedResult);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Exception_HealthCheckResult_As_Unhealthy(
            [Frozen] Mock<ILogger<AcsVacancySearchRepository>> logger,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            AcsVacancySearchRepository sut)
        {
            azureSearchHelper.Setup(x => x.GetIndexName(CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await sut.GetHealthCheckStatus(CancellationToken.None);

            using (new AssertionScope())
            {
                actual.Should().Be(HealthCheckResult.UnHealthy);
            }
        }
    }
}