using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Elasticsearch.Net;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Data.AzureSearch;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Data.UnitTests.ElasticSearchRepository
{
    public class WhenGettingApprenticeshipVacancyCount
    {
        private const string IndexName = "-faa-apprenticeships";
        [Test, MoqAutoData]
        public async Task Then_Will_Return_ApprenticeshipVacancies_Count(
            FindVacanciesModel model,
            [Frozen] ElasticEnvironment environment,
            [Frozen] Mock<IElasticSearchQueryBuilder> mockQueryBuilder,
            [Frozen] Mock<IElasticLowLevelClient> mockElasticClient,
            ApprenticeshipVacancySearchRepository repository)
        {
            //arrange
            mockElasticClient.Setup(c =>
                    c.CountAsync<StringResponse>(
                        $"{environment.Prefix}{IndexName}",
                        It.IsAny<PostData>(),
                        It.IsAny<CountRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(@"{""count"":10}"));

            //Act
            var results = await repository.Count();

            //Assert
            results.Should().Be(10);
        }
    }
}