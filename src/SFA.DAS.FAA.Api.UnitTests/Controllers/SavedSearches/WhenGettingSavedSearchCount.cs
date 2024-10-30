using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchCount;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.SavedSearches;

public class WhenGettingSavedSearchCount
{
    [Test, MoqAutoData]
    public async Task If_Valid_Submission_Then_Ok_Is_Returned(
        int count,
        GetSavedSearchCountQuery query,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController controller)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.Is<GetSavedSearchCountQuery>(c => c.UserReference == query.UserReference), default))
            .ReturnsAsync(count);
        
        // act
        var response = await controller.GetSavedSearchCount(query.UserReference) as OkObjectResult;
        var payload = response?.Value as GetSavedSearchCountResponse;
        
        // assert
        using (new AssertionScope())
        {
            response?.StatusCode.Should().Be(200);
            payload!.UserReference.Should().Be(query.UserReference);
            payload.SavedSearchesCount.Should().Be(count);
        }
    }
}