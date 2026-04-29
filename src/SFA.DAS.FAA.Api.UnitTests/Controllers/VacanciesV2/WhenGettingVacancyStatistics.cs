using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.VacanciesV2;

public class WhenGettingVacancyStatistics
{
    [Test, MoqAutoData]
    public async Task Then_The_Data_Is_Returned(
        Mock<IAzureSearchHelper> searchHelper,
        [Greedy] VacanciesController sut,
        CancellationToken cancellationToken)
    {
        // arrange
        var values = Enum.GetValues<DataSource>();
        ConcurrentBag<FindVacanciesCountModel> items = [];
        searchHelper
            .Setup(x => x.Count(It.IsAny<FindVacanciesCountModel>()))
            .Callback<FindVacanciesCountModel>(items.Add)
            .ReturnsAsync(3);
        
        var now = DateTime.UtcNow;
        searchHelper
            .Setup(x => x.GetIndexName(It.IsAny<CancellationToken>()))
            .ReturnsAsync($"{AzureSearchIndex.IndexName}-{now:yyyy-MM-dd-HH-mm}");
        
        // act
        var result = await sut.GetSearchIndexStatistics(searchHelper.Object, CancellationToken.None) as OkObjectResult;
        var data = result?.Value as GetSearchIndexStatisticsResponse;

        // assert
        result.Should().NotBeNull();
        data.Should().NotBeNull();
        
        data!.CreatedDate.Should().BeCloseTo(now, TimeSpan.FromMinutes(1));
        data.LastUpdatedDate.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        data.IndexStatistics.Should().AllSatisfy(x =>
        {
            x.Count.Should().Be(3);
        });
        
        searchHelper.Verify(x => x.Count(It.IsAny<FindVacanciesCountModel>()), Times.Exactly(values.Length));
        items.Should().AllSatisfy(x => values.Contains(x.DataSources.Single()));
    }
}