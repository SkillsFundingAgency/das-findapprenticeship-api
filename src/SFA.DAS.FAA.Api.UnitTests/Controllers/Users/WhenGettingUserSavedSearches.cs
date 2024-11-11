using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Users;

public class WhenGettingUserSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        Guid userReference,
        GetSavedSearchesByUserReferenceQueryResult getSavedSearchesByUserReferenceQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        // arrange
        GetSavedSearchesByUserReferenceQuery passedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchesByUserReferenceQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetSavedSearchesByUserReferenceQueryResult>, CancellationToken>((x, _) => passedQuery = x as GetSavedSearchesByUserReferenceQuery)
            .ReturnsAsync(getSavedSearchesByUserReferenceQueryResult);

        // act
        var response = await sut.GetByUserReference(userReference) as OkObjectResult;
        var model = response?.Value as GetSavedSearchesByUserReferenceResponse;

        // assert
        passedQuery.UserReference.Should().Be(userReference);
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        model.Should().NotBeNull();
        model!.SavedSearches.Should().BeEquivalentTo(getSavedSearchesByUserReferenceQueryResult.SavedSearches);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Exceptions_Are_Handled(
        Guid userReference,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchesByUserReferenceQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        // act
        var response = await sut.GetByUserReference(userReference) as StatusCodeResult;

        // assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}