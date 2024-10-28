using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;

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
    public async Task<IActionResult> SaveSearch([FromBody] SaveSearchRequest request)
    {
        var result = await mediator.Send(new SaveSearchCommand(
            request.UserReference,
            request.SearchParameters
        ));

        return Ok(PostSaveSearchResponse.From(result));
    }
}