using System;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;
using SFA.DAS.FAA.Application.SavedSearches.Queries.GetSavedSearchCount;

namespace SFA.DAS.FAA.Api.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
[Route("/api/SavedSearches/")]
public class SavedSearchesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("")]
    [ProducesResponseType<PostSaveSearchResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType<GetSavedSearchCountResponse>((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SaveSearch([FromBody] SaveSearchRequest request)
    {
        var result = await mediator.Send(new SaveSearchCommand(
            request.UserReference,
            request.SearchParameters
        ));

        return Ok(PostSaveSearchResponse.From(result));
    }

    [HttpGet]
    [Route("count")]
    [ProducesResponseType<GetSavedSearchCountResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType<GetSavedSearchCountResponse>((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetSavedSearchCount([FromQuery] Guid candidateId)
    {
        var result = await mediator.Send(new GetSavedSearchCountQuery
        {
            UserReference = candidateId
        });

        return Ok(new GetSavedSearchCountResponse(candidateId, result));
    }
}