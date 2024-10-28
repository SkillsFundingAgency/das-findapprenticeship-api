using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.SavedSearches;

public class WhenPostingSavedSearch
{
    [Test, MoqAutoData]
    public async Task If_Valid_Submission_Then_Ok_Is_Returned(
        SaveSearchCommandResult saveSearchCommandResult,
        SaveSearchRequest saveSearchRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController controller)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.Is<SaveSearchCommand>(c => 
                c.UserReference == saveSearchRequest.UserReference 
                && c.SearchParameters == saveSearchRequest.SearchParameters), CancellationToken.None))
            .ReturnsAsync(saveSearchCommandResult);
        
        // act
        var response = await controller.SaveSearch(saveSearchRequest) as OkObjectResult;
        var payload = response?.Value as PostSaveSearchResponse;
        
        // assert
        using (new AssertionScope())
        {
            response?.StatusCode.Should().Be(200);
            payload.Should().BeEquivalentTo(saveSearchCommandResult);
        }
    }
}