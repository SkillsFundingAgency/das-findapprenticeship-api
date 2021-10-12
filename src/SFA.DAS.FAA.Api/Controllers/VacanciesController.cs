using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/[controller]/")]
    public class VacanciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VacanciesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            await Task.CompletedTask;
            return Ok(new {ok = "get ok"});
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search(
            string searchTerm,
            int pageNumber,
            int pageSize)
        {
            await Task.CompletedTask;
            return Ok(new {ok = "search ok"});
        }
    }
}