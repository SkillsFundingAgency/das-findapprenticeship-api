using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearches;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Users;

[TestFixture]
public class WhenDeletingSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_The_UserReference_Is_Passed_To_The_Command_Handler_And_NoContent_Returned(
        Guid userReference,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        var actual = await sut.DeleteSavedSearches(userReference, default) as NoContentResult;

        actual.Should().NotBeNull();
        mediator.Verify(
            x => x.Send(It.Is<DeleteSavedSearchesCommand>(c => c.UserReference.Equals(userReference)), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_InternalServer_Error_Returned(
        Guid userReference,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        mediator
            .Setup(x => x.Send(It.Is<DeleteSavedSearchesCommand>(c => c.UserReference.Equals(userReference)), It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        var actual = await sut.DeleteSavedSearches(userReference, default) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}