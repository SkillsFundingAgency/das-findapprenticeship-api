using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Elasticsearch.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FAA.Data.ElasticSearch;
using SFA.DAS.FAA.Data.Repository;
using SFA.DAS.FAA.Data.UnitTests.Extensions;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.UnitTests.Repository
{
    public class WhenFindingApprenticeshipVacancies
    {
        private const string ExpectedEnvironmentName = "test";
        private const string IndexName = "-faa-apprenticeships";

        private Mock<IElasticLowLevelClient> _mockClient;
        private ElasticEnvironment _apiEnvironment;
        private ApprenticeshipVacancySearchRepository _repository;
        private Mock<IElasticSearchQueryBuilder> _mockQueryBuilder;

        [SetUp]
        public void Init()
        {
            _mockClient = new Mock<IElasticLowLevelClient>();
            _mockQueryBuilder = new Mock<IElasticSearchQueryBuilder>();
            _apiEnvironment = new ElasticEnvironment(ExpectedEnvironmentName);
            _repository = new ApprenticeshipVacancySearchRepository(_mockClient.Object, _apiEnvironment, _mockQueryBuilder.Object, Mock.Of<ILogger<ApprenticeshipVacancySearchRepository>>());

            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        $"{_apiEnvironment.Prefix}{IndexName}",
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(FakeElasticResponses.MoreThanOneHitResponse));

            _mockClient.Setup(c =>
                    c.CountAsync<StringResponse>(
                        $"{_apiEnvironment.Prefix}{IndexName}",
                        It.IsAny<PostData>(),
                        It.IsAny<CountRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(@"{""count"":10}"));

            _mockQueryBuilder
                .Setup(x => x.BuildFindVacanciesQuery(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>()))
                .Returns(string.Empty);
            _mockQueryBuilder.Setup(x => x.BuildGetVacanciesCountQuery()).Returns(string.Empty);
        }

        [Test, AutoData]
        public async Task Then_Will_Lookup_Total_ApprenticeshipVacancies(
            int pageNumber, 
            int pageSize, 
            string countQuery)
        {
            //Arrange
            _mockQueryBuilder
                .Setup(x => x.BuildGetVacanciesCountQuery())
                .Returns(countQuery);

            //Act
            await _repository.Find(pageNumber, pageSize);

            //Assert
            _mockClient.Verify(c =>
                c.CountAsync<StringResponse>(
                    $"{_apiEnvironment.Prefix}{IndexName}",
                    It.Is<PostData>(pd => 
                        pd.GetRequestString().Equals(countQuery)),
                    It.IsAny<CountRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Will_Search_Latest_ApprenticeshipVacanciesIndex(
            int pageNumber, 
            int pageSize, 
            string query)
        {
            //Arrange
    
            _mockQueryBuilder
                .Setup(x => x.BuildFindVacanciesQuery(pageNumber, pageSize, null))
                .Returns(query);

            //Act
            await _repository.Find(pageNumber, pageSize);

            //Assert
            _mockClient.Verify(c =>
                c.SearchAsync<StringResponse>(
                    $"{_apiEnvironment.Prefix}{IndexName}",
                    It.Is<PostData>(pd => 
                        pd.GetRequestString().Equals(query)),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Then_Will_Return_ApprenticeshipVacancies_Found()
        {
            //arrange
            var expectedVacancy = JsonConvert
                .DeserializeObject<ElasticResponse<ApprenticeshipSearchItem>>(FakeElasticResponses.MoreThanOneHitResponse)
                .Items.First();
            
            //Act
            var results = await _repository.Find(1, 1);

            //Assert
            results.Total.Should().Be(10);
            results.TotalFound.Should().Be(2);
            results.ApprenticeshipVacancies.Count().Should().Be(2);
            var vacancy = results.ApprenticeshipVacancies.First();
            vacancy.Should().BeEquivalentTo(expectedVacancy);
        }

        [Test]
        public async Task Then_Will_Return_Empty_Result_If_ApprenticeshipVacanciesIndex_Request_Returns_Invalid_Response()
        {
            //Arrange
            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        $"{_apiEnvironment.Prefix}{IndexName}",
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(""));

            //Act
            var result = await _repository.Find(1, 10);

            //Assert
            Assert.IsNotNull(result?.ApprenticeshipVacancies);
            Assert.IsEmpty(result.ApprenticeshipVacancies);
            Assert.AreEqual(0, result.TotalFound);
        }

        [Test]
        public async Task Then_Will_Return_Empty_Result_If_ApprenticeshipVacanciesIndex_Request_Returns_Failed_Response()
        {
            //Arrange
            var response =  @"{""took"":0,""timed_out"":false,""_shards"":{""total"":1,""successful"":0,""skipped"":0,""failed"":1},""hits"":{""total"":
            {""value"":0,""relation"":""eq""},""max_score"":null,""hits"":[]}}";

            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        $"{_apiEnvironment.Prefix}{IndexName}",
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(response));

            //Act
            var result = await _repository.Find(1, 10);

            //Assert
            Assert.IsNotNull(result?.ApprenticeshipVacancies);
            Assert.IsEmpty(result.ApprenticeshipVacancies);
            Assert.AreEqual(0, result.TotalFound);
        }
    }
}
