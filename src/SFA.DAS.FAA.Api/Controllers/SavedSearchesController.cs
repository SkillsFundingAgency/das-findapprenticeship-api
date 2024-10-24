using System;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FAA.Application.SavedSearches.GetSavedSearches;

namespace SFA.DAS.FAA.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("/api/savedSearches/")]
    public class SavedSearchesController(IMediator mediator, ILogger<SavedSearchesController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery] DateTime lastRunDateFilter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            try
            {
                var result = await mediator.Send(new GetSavedSearchesQuery(lastRunDateFilter, pageNumber, pageSize));
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get Saved Searches : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}