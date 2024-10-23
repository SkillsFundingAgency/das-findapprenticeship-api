using System;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetSavedSearches;

namespace SFA.DAS.FAA.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("/api/savedSearch/")]
    public class SavedSearchController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery] DateTime lastRunDateFilter)
        {
            try
            {
                var result = await mediator.Send(new GetSavedSearchesQuery(lastRunDateFilter));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
