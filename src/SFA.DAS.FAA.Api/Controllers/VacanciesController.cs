using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Core;
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

    public abstract class VacanciesControllerBase(IMediator mediator, string searchSource) : ControllerBase
    {
        [HttpGet]
        [Route("{vacancyReference}")]
        public async Task<IActionResult> Get(string vacancyReference)
        {
            var result = await mediator.Send(new GetApprenticeshipVacancyQuery
            {
                VacancyReference = vacancyReference
            });

            if (result.ApprenticeshipVacancy == null)
            {
                return NotFound();
            }

            var apiResponse = (GetApprenticeshipVacancyDetailResponse)result.ApprenticeshipVacancy;

            return Ok(apiResponse);
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search([FromQuery] SearchVacancyRequest request)
        {
            var result = await mediator.Send(new SearchApprenticeshipVacanciesQuery
            {
                SearchTerm = request.SearchTerm,
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
                VacancySort = request.Sort ?? VacancySort.AgeDesc,
                Source = searchSource
            });

            var apiResponse = (GetSearchApprenticeshipVacanciesResponse)result;

            return Ok(apiResponse);
        }
        
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetVacancyCount([FromQuery] SearchVacancyTotalRequest request)
        {
            try
            {
                var result = await mediator.Send(new GetApprenticeshipVacancyCountQuery
                {
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

                });
                return Ok(new GetCountApprenticeshipVacanciesResponse{TotalVacancies = result});
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
    
    
    
    
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/Vacancies/")]
    public class VacanciesController(IMediator mediator) : VacanciesControllerBase(mediator, "Elastic");
    
    [ApiVersion("2.0")]
    [ApiController]
    [Route("/api/Vacancies/")]
    public class VacanciesV2Controller(IMediator mediator) : VacanciesControllerBase(mediator, "ACS");
}