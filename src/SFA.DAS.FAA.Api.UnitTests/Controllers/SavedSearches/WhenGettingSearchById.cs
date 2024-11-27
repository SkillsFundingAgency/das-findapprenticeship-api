using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.SavedSearches;

public class WhenGettingSearchById
{
    [Test, MoqAutoData]
    public async Task Then_The_SavedSearch_Is_Returned_By_Id(
        Guid id,
        GetSavedSearchQueryResult getSavedSearchQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        mediator.Setup(x => x.Send(It.Is<GetSavedSearchQuery>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getSavedSearchQueryResult);
        
        var actual = await savedSearchesController.GetById(id) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual!.Value as GetSavedSearchResponse;
        actualModel!.SavedSearch.Should().BeEquivalentTo(getSavedSearchQueryResult.SavedSearch);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_SavedSearch_Query_Returns_Null_Then_Returns_NotFound_Response(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        mediator.Setup(x => x.Send(It.Is<GetSavedSearchQuery>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetSavedSearchQueryResult());
        
        var actual = await savedSearchesController.GetById(id) as NotFoundResult;

        actual.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_InternalServerError_Response_Returned(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        mediator.Setup(x => x.Send(It.Is<GetSavedSearchQuery>(c => c.Id == id), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        var actual = await savedSearchesController.GetById(id) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}