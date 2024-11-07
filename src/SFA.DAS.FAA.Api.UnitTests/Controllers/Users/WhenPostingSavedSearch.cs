using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Users;

public class WhenPostingSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_Search_Is_Saved(
        Guid userReference,
        Guid id,
        UpsertSaveSearchCommandResult upsertSaveSearchCommandResult,
        SaveSearchRequest saveSearchRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        // arrange
        UpsertSaveSearchCommand passedCommand = null;
        mediator
            .Setup(x => x.Send(It.IsAny<UpsertSaveSearchCommand>(), CancellationToken.None))
            .Callback<IRequest<UpsertSaveSearchCommandResult>, CancellationToken>((x, _) => passedCommand = x as UpsertSaveSearchCommand)
            .ReturnsAsync(upsertSaveSearchCommandResult);
        
        // act
        var response = await sut.SaveSearch(userReference, id, saveSearchRequest) as OkObjectResult;
        var payload = response?.Value as PutSaveSearchResponse;
        
        // assert
        response?.StatusCode.Should().Be(200);
        payload.Should().BeEquivalentTo(upsertSaveSearchCommandResult);
        passedCommand.Should().NotBeNull();
        passedCommand.UserReference.Should().Be(userReference);
        passedCommand.Id.Should().Be(id);
        passedCommand.UnSubscribeToken.Should().Be(saveSearchRequest.UnSubscribeToken);
        passedCommand.SearchParameters.Should().BeEquivalentTo(saveSearchRequest.SearchParameters);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Exceptions_Are_Handled(
        UpsertSaveSearchCommandResult upsertSaveSearchCommandResult,
        SaveSearchRequest saveSearchRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<UpsertSaveSearchCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        
        // act
        var response = await sut.SaveSearch(Guid.NewGuid(), Guid.NewGuid(), saveSearchRequest) as StatusCodeResult;
        
        // assert
        response?.StatusCode.Should().Be(500);
    }
}