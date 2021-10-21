using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Data.Repository;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.UnitTests.Repository
{
    public class WhenPingingElasticSearchServer
    {
        private const string IndexName = "test-faa-apprenticeships";
        private Mock<IElasticLowLevelClient> _mockClient;
        private ElasticEnvironment _apiEnvironment;
        private ApprenticeshipVacancySearchRepository _repository;
        
        [SetUp]
        public void Init()
        {
            _mockClient = new Mock<IElasticLowLevelClient>();
            _apiEnvironment = new ElasticEnvironment("test");
            _repository = new ApprenticeshipVacancySearchRepository(
                _mockClient.Object, 
                _apiEnvironment, 
                Mock.Of<IElasticSearchQueryBuilder>(), 
                Mock.Of<ILogger<ApprenticeshipVacancySearchRepository>>());
        }

        [Test]
        public async Task Then_Returns_True_If_Api_Call_Successful()
        {
            //Arrange
            var apiCallMock = new Mock<IApiCallDetails>();
            apiCallMock
                .Setup(api => api.Success)
                .Returns(true);

            _mockClient
                .Setup(c => c.CountAsync<StringResponse>(IndexName, It.IsAny<PostData>(), It.IsAny<CountRequestParameters>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse {ApiCall = apiCallMock.Object});

            //Act
            var result = await _repository.PingAsync();

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Then_Returns_False_If_Api_Call_Fails()
        {
            //Arrange
            var apiCallMock = new Mock<IApiCallDetails>();
            apiCallMock
                .Setup(api => api.Success)
                .Returns(false);

            _mockClient
                .Setup(c => c.CountAsync<StringResponse>(IndexName, It.IsAny<PostData>(), It.IsAny<CountRequestParameters>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse {ApiCall = apiCallMock.Object});

            //Act
            var result = await _repository.PingAsync();

            //Assert
            Assert.IsFalse(result);
        }
    }
}
