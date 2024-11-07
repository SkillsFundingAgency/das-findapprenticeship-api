using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Users;

public class WhenDeletingSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_Id_Is_Passed_To_The_Command_Handler_And_NoContent_Returned(
        Guid userReference,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        var actual = await sut.DeleteSavedSearch(userReference, id, default) as NoContentResult;

        actual.Should().NotBeNull();
        mediator.Verify(
            x => x.Send(It.Is<DeleteSavedSearchCommand>(c => c.Id.Equals(id) && c.UserReference.Equals(userReference)), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_InteralServer_Error_Returned(
        Guid userReference,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        mediator
            .Setup(x => x.Send(It.Is<DeleteSavedSearchCommand>(c => c.Id.Equals(id)), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        var actual = await sut.DeleteSavedSearch(userReference, id, default) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}