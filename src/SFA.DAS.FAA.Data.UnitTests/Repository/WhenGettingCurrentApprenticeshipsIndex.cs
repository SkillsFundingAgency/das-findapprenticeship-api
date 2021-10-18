using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Data.Repository;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.UnitTests.Repository
{
    public class WhenGettingCurrentApprenticeshipsIndex
    {
        [Test]
        public void Then_Returns_Alias_Based_On_Environment()
        {
            //arrange
            var apiEnvironment = new ElasticEnvironment("test");
            var repository = new ApprenticeshipVacancySearchRepository(
                Mock.Of<IElasticLowLevelClient>(), 
                apiEnvironment, 
                Mock.Of<IElasticSearchQueries>(), 
                Mock.Of<ILogger<ApprenticeshipVacancySearchRepository>>());
            
            //Act
            var index = repository.GetCurrentApprenticeshipVacanciesIndex();

            //Assert
            index.Should().Be(apiEnvironment.Prefix + "-faa-apprenticeships");
        }
    }
}
