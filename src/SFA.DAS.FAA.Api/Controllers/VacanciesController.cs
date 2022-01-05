using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using SFA.DAS.FAA.Domain.Models;

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

            if (result.ApprenticeshipVacancy == null)
            {
                return NotFound();
            }

            var apiResponse = (GetApprenticeshipVacancyResponse) result.ApprenticeshipVacancy;
            
            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search(
            int pageNumber = 1,
            int pageSize = 10,
            int? ukprn = null,
            string accountPublicHashedId = null,
            string accountLegalEntityPublicHashedId = null,
            int? standardLarsCode = null,
            bool nationWideOnly = false,
            double? lat = null,
            double? lon = null,
            uint? distanceInMiles = null,
            string route = null,
            uint? postedInLastNumberOfDays = null,
            VacancySort sort = VacancySort.AgeDesc)
        {
            var result = await _mediator.Send(new SearchApprenticeshipVacanciesQuery
            {
                PageNumber = pageNumber, 
                PageSize = pageSize,
                Ukprn = ukprn,
                AccountPublicHashedId = accountPublicHashedId,
                AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId,
                Route = route,
                Lat = lat,
                Lon = lon,
                DistanceInMiles = distanceInMiles,
                NationWideOnly = nationWideOnly,
                StandardLarsCode = standardLarsCode,
                PostedInLastNumberOfDays = postedInLastNumberOfDays,
                VacancySort = sort
            });

            var apiResponse = (GetSearchApprenticeshipVacanciesResponse) result;
            
            return Ok(apiResponse);
        }
    }
}