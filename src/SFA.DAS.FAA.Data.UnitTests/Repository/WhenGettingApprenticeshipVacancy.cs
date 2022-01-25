using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Elasticsearch.Net;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FAA.Data.ElasticSearch;
using SFA.DAS.FAA.Data.Repository;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Data.UnitTests.Repository
{
    public class WhenGettingApprenticeshipVacancy
    {
        private const string IndexName = "-faa-apprenticeships";
        
        [Test, MoqAutoData]
        public async Task And_Found_Then_Returns_Vacancy(
            string vacancyReference,
            [Frozen] ElasticEnvironment environment,
            [Frozen] Mock<IElasticLowLevelClient> mockElasticClient,
            ApprenticeshipVacancySearchRepository repository)
        {
            var expectedVacancy = JsonConvert
                .DeserializeObject<ElasticResponse<ApprenticeshipSearchItem>>(FakeElasticResponses.SingleHitResponse)
                .Items.First();
            
            mockElasticClient
                .Setup(client => client.SearchAsync<StringResponse>(
                    $"{environment.Prefix}{IndexName}",
                    It.IsAny<PostData>(),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(FakeElasticResponses.SingleHitResponse));
            
            var vacancy = await repository.Get(vacancyReference);

            vacancy.Should().BeEquivalentTo(expectedVacancy._source);
        }
        
        [Test, MoqAutoData]
        public async Task And_Not_Found_Then_Returns_Default(
            string vacancyReference,
            [Frozen] ElasticEnvironment environment,
            [Frozen] Mock<IElasticLowLevelClient> mockElasticClient,
            ApprenticeshipVacancySearchRepository repository)
        {
            mockElasticClient
                .Setup(client => client.SearchAsync<StringResponse>(
                    $"{environment.Prefix}{IndexName}",
                    It.IsAny<PostData>(),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(FakeElasticResponses.NoHitsResponse));
            
            var vacancy = await repository.Get(vacancyReference);

            vacancy.Should().BeNull();
        }
        
        [Test, MoqAutoData]
        public async Task And_More_Than_One_Hit_Then_Throws_Exception(
            string vacancyReference,
            [Frozen] ElasticEnvironment environment,
            [Frozen] Mock<IElasticLowLevelClient> mockElasticClient,
            ApprenticeshipVacancySearchRepository repository)
        {
            mockElasticClient
                .Setup(client => client.SearchAsync<StringResponse>(
                    $"{environment.Prefix}{IndexName}",
                    It.IsAny<PostData>(),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(FakeElasticResponses.MoreThanOneHitResponse));
            
            Func<Task> act = async () => await repository.Get(vacancyReference);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}