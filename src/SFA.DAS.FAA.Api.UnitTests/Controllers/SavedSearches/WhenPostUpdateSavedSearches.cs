using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Commands.PostUpdateSavedSearches;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.SavedSearches;

[TestFixture]
public class WhenPostUpdateSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_Post_Result_From_Mediator(
        PostUpdateSavedSearchesRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SavedSearchesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<PostUpdateSavedSearchesCommand>(command =>
                    command.SavedSearchGuids == request.SavedSearchGuids),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Unit());

        var result = await controller.PostUpdate(request) as NoContentResult;
        result!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Test, MoqAutoData]
    public async Task Then_Post_Error_Returns_Status_Code_Result_InternalServerError(
        PostUpdateSavedSearchesRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SavedSearchesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<PostUpdateSavedSearchesCommand>(command =>
                    command.SavedSearchGuids == request.SavedSearchGuids),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var result = await controller.PostUpdate(request) as StatusCodeResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}