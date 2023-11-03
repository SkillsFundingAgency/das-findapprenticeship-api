using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.ApRequests;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount;
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

            var apiResponse = (GetApprenticeshipVacancyDetailResponse) result.ApprenticeshipVacancy;
            
            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search([FromQuery]SearchVacancyRequest request)
        {
            var result = await _mediator.Send(new SearchApprenticeshipVacanciesQuery
            {
                PageNumber = request.PageNumber, 
                PageSize = request.PageSize,
                Ukprn = request.Ukprn,
                AccountPublicHashedId = request.AccountPublicHashedId,
                AccountLegalEntityPublicHashedId = request.AccountLegalEntityPublicHashedId,
                Categories = request.Categories,
                Lat = request.Lat,
                Lon = request.Lon,
                DistanceInMiles = request.DistanceInMiles,
                NationWideOnly = request.NationWideOnly,
                StandardLarsCode = request.StandardLarsCode,
                PostedInLastNumberOfDays = request.PostedInLastNumberOfDays,
                VacancySort = request.Sort ?? VacancySort.AgeDesc
            });

            var apiResponse = (GetSearchApprenticeshipVacanciesResponse) result;
            
            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetVacancyCount([FromQuery] List<string>? routeIds, [FromQuery] string? location)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeshipVacancyCountQuery
                {
                    location = location,
                    SelectedRouteIds = routeIds,
                    NationalSearch = (location == null)
                });
                return Ok(new GetCountApprenticeshipVacanciesResponse{TotalVacancies = result});
            }
            catch (Exception)
            {
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}