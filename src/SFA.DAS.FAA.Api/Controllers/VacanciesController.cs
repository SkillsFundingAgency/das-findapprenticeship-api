using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipsVacanciesByIdList;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using SFA.DAS.FAA.Domain.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;

namespace SFA.DAS.FAA.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("/api/Vacancies/")]
    public class VacanciesController(IMediator mediator) : ControllerBase
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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> GetByVacancyReferences([FromBody] GetVacanciesByReferenceRequest request)
        {
            var result = await mediator.Send(new GetApprenticeshipVacanciesByReferenceQuery
            {
                VacancyReferences = request.VacancyReferences
            });

            var apiResponse = (GetApprenticeshipVacanciesByReferenceApiResponse)result;
            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Search([FromQuery] SearchVacancyRequest request)
        {
            try
            {
                var result = await mediator.Send(new SearchApprenticeshipVacanciesQuery
                {
                    SearchTerm = request.SearchTerm,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    Ukprn = request.Ukprn,
                    AccountPublicHashedId = request.AccountPublicHashedId,
                    AccountLegalEntityPublicHashedId = request.AccountLegalEntityPublicHashedId,
                    EmployerName = request.EmployerName,
                    Categories = request.Categories,
                    RouteIds = request.RouteIds,
                    Levels = request.Levels,
                    Lat = request.Lat,
                    Lon = request.Lon,
                    DistanceInMiles = request.DistanceInMiles,
                    ExcludeNational = request.ExcludeNational,
                    StandardLarsCode = request.StandardLarsCode,
                    PostedInLastNumberOfDays = request.PostedInLastNumberOfDays,
                    VacancySort = request.Sort ?? VacancySort.AgeDesc,
                    SkipWageType = request.SkipWageType,
                    DisabilityConfident = request.DisabilityConfident,
                    AdditionalDataSources = request.AdditionalDataSources
                });

                var apiResponse = (GetSearchApprenticeshipVacanciesResponse)result;

                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetVacancyCount([FromQuery] SearchVacancyTotalRequest request)
        {
            try
            {
                var result = await mediator.Send(new GetApprenticeshipVacancyCountQuery
                {
                    SearchTerm = request.SearchTerm,
                    Categories = request.Categories,
                    RouteIds = request.RouteIds,
                    Levels = request.Levels,
                    Lat = request.Lat,
                    Lon = request.Lon,
                    DistanceInMiles = request.DistanceInMiles,
                    ExcludeNational = request.ExcludeNational,
                    WageType = request.WageType,
                    DisabilityConfident = request.DisabilityConfident,
                    DataSources = request.DataSources
                });
                return Ok(new GetCountApprenticeshipVacanciesResponse { TotalVacancies = result });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}