using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;
using SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchCount;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchesByUserReference;

namespace SFA.DAS.FAA.Api.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
[Route("/api/[controller]/")]
public class UsersController(IMediator mediator, ILogger<SavedSearchesController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{userReference:guid}/SavedSearches")]
    [ProducesResponseType<GetSavedSearchesResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetByUserReference([FromRoute] Guid userReference, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(new GetSavedSearchesByUserReferenceQuery
            {
                UserReference = userReference
            }, cancellationToken);

            return Ok(GetSavedSearchesByUserReferenceResponse.From(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get Saved Searches : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpGet]
    [Route("{userReference:guid}/SavedSearches/{id:guid}")]
    [ProducesResponseType<GetSavedSearchResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Get([FromRoute] Guid userReference, Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(new GetSavedSearchQuery(userReference, id), cancellationToken);
            return result is null
                ? NotFound()
                : Ok(new GetSavedSearchResponse(SavedSearchDto.From(result.SavedSearch)));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get Saved Search: An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpPut]
    [Route("{userReference:guid}/SavedSearches/{id:guid}")]
    [ProducesResponseType<PutSaveSearchResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SaveSearch([FromRoute] Guid userReference, [FromRoute] Guid id, [FromBody] SaveSearchRequest saveSearchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(new UpsertSaveSearchCommand(
                id,
                userReference,
                saveSearchRequest.SearchParameters
            ), cancellationToken);

            return Ok(PutSaveSearchResponse.From(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Put Saved Search : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpDelete]
    [Route("{userReference:guid}/SavedSearches/{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteSavedSearch([FromRoute] Guid userReference, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(new DeleteSavedSearchCommand(id, userReference), cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Delete Saved Search : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpGet]
    [Route("{userReference:guid}/SavedSearches/count")]
    [ProducesResponseType<GetSavedSearchCountResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetSavedSearchCount([FromRoute] Guid userReference)
    {
        try
        {
            var result = await mediator.Send(new GetSavedSearchCountQuery
            {
                UserReference = userReference
            });

            return Ok(new GetSavedSearchCountResponse(userReference, result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Count Saved Searches : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}