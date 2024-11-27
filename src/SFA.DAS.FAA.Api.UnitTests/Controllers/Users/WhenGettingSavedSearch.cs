using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetUserSavedSearch;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Users;

public class WhenGettingSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_Result_Is_Returned(
        Guid userReference,
        Guid id,
        GetUserSavedSearchQueryResult getSavedSearchQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        // arrange
        GetUserSavedSearchQuery passedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetUserSavedSearchQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetUserSavedSearchQueryResult>, CancellationToken>((x, _) => passedQuery = x as GetUserSavedSearchQuery)
            .ReturnsAsync(getSavedSearchQueryResult);

        // act
        var response = await sut.Get(userReference, id) as OkObjectResult;
        var model = response?.Value as GetSavedSearchResponse;

        // assert
        passedQuery.UserReference.Should().Be(userReference);
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        model.Should().NotBeNull();
        model!.SavedSearch.Should().BeEquivalentTo(getSavedSearchQueryResult.SavedSearch);
    }
    
    [Test, MoqAutoData]
    public async Task Then_NotFound_Is_Returned(
        Guid userReference,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetUserSavedSearchQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetUserSavedSearchQueryResult)null);

        // act
        var response = await sut.Get(userReference, id) as NotFoundResult;

        // assert
        response.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_Exceptions_Are_Handled(
        Guid userReference,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetUserSavedSearchQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        // act
        var response = await sut.Get(userReference, id) as StatusCodeResult;

        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}