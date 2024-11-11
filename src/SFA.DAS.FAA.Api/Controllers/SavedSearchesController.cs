using System;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearch;
using SFA.DAS.FAA.Application.SavedSearches.Commands.PatchSavedSearch;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearch;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearches;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
[Route("/api/[controller]/")]
public class SavedSearchesController(IMediator mediator, ILogger<SavedSearchesController> logger) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType<GetSavedSearchesResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] DateTime lastRunDateFilter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
    {
        try
        {
            var result = await mediator.Send(new GetSavedSearchesQuery(lastRunDateFilter, pageNumber, pageSize));
            return Ok(GetSavedSearchesResponse.From(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get Saved Searches : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType<GetSavedSearchResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var result = await mediator.Send(new GetSavedSearchQuery(id));

            if (result.SavedSearch == null)
            {
                return NotFound();
            }
            
            return Ok(new GetSavedSearchResponse{
                SavedSearch = SavedSearchDto.From(result.SavedSearch)
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get Saved Searches : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PatchSavedSearch([FromRoute] Guid id, [FromBody] JsonPatchDocument<PatchSavedSearch> savedSearchRequest)
    {
        try
        {
            var result = await mediator.Send(new PatchSavedSearchCommand(id, savedSearchRequest));

            if (result.SavedSearch is null)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Patch Saved Search : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteSavedSearch([FromRoute] Guid id)
    {
        try
        {
            await mediator.Send(new DeleteSavedSearchCommand
            {
                Id = id
            });
            return NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Delete Saved Search : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
    
}
