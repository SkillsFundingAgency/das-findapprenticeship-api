using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.SavedSearches;

public class WhenDeletingSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_Id_Is_Passed_To_Command_And_No_Content_Result_Returned(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        var actual = await savedSearchesController.DeleteSavedSearch(id) as NoContentResult;

        actual.Should().NotBeNull();
        mediator.Verify(x=>x.Send(It.Is<DeleteSavedSearchCommand>(c=>c.Id==id), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Exception_InternalServerError_Response_Returned(
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        mediator.Setup(x => x.Send(
                It.Is<DeleteSavedSearchCommand>(command =>
                    command.Id == id),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception());
        
        var actual = await savedSearchesController.DeleteSavedSearch(id) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Multiple_Saved_Searches_Can_Be_Deleted(
        List<Guid> ids,
        [Frozen] Mock<ISavedSearchRepository> repository,
        [Greedy] SavedSearchesController sut)
    {
        // act
        var result = await sut.DeleteManyAsync(repository.Object, ids, CancellationToken.None) as NoContentResult;

        // assert
        result.Should().NotBeNull();
        repository.Verify(x => x.DeleteManyAsync(ids, It.IsAny<CancellationToken>()), Times.Once);
    }
}