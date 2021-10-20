using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;

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
        [Route("{vacancyReference}")]
        public async Task<IActionResult> Get(string vacancyReference)
        {
            var result = await _mediator.Send(new GetApprenticeshipVacancyQuery
            {
                VacancyReference = vacancyReference
            });

            var apiResponse = (GetApprenticeshipVacancyResponse) result.ApprenticeshipVacancy;
            
            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _mediator.Send(new SearchApprenticeshipVacanciesQuery
            {
                PageNumber = pageNumber, 
                PageSize = pageSize
            });

            var apiResponse = (GetSearchApprenticeshipVacanciesResponse) result;
            
            return Ok(apiResponse);
        }
    }
}