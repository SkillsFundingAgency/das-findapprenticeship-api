using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Commands.PatchSavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.SavedSearches;

[TestFixture]
public class WhenPostUpdateSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_Patch_Result_From_Mediator(
        Guid id,
        JsonPatchDocument<PatchSavedSearch> request,
        PatchSavedSearchCommandResponse response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SavedSearchesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<PatchSavedSearchCommand>(command =>
                    command.Id == id &&
                    command.Patch == request),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await controller.PatchSavedSearch(id, request) as NoContentResult;
        result!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Test, MoqAutoData]
    public async Task Then_Patch_Not_Found_Returns_Status_Code_Result_NotFound(
        Guid id,
        JsonPatchDocument<PatchSavedSearch> request,
        PatchSavedSearchCommandResponse response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SavedSearchesController controller)
    {
        response.SavedSearch = null;

        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<PatchSavedSearchCommand>(command =>
                    command.Id == id &&
                    command.Patch == request),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await controller.PatchSavedSearch(id, request) as StatusCodeResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task Then_Post_Error_Returns_Status_Code_Result_InternalServerError(
        Guid id,
        JsonPatchDocument<PatchSavedSearch> request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SavedSearchesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<PatchSavedSearchCommand>(command =>
                    command.Id == id &&
                    command.Patch == request),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var result = await controller.PatchSavedSearch(id, request) as StatusCodeResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}