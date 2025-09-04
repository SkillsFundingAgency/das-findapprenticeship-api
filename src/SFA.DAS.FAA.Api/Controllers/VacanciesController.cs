using Asp.Versioning;
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

            var apiResponse = GetApprenticeshipVacancyDetailResponse.From(result.ApprenticeshipVacancy);
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
                    AccountLegalEntityPublicHashedId = request.AccountLegalEntityPublicHashedId,
                    AccountPublicHashedId = request.AccountPublicHashedId,
                    AdditionalDataSources = request.AdditionalDataSources,
                    ApprenticeshipTypes = request.ApprenticeshipTypes,
                    Categories = request.Categories,
                    DisabilityConfident = request.DisabilityConfident,
                    DistanceInMiles = request.DistanceInMiles,
                    EmployerName = request.EmployerName,
                    ExcludeNational = request.ExcludeNational,
                    Lat = request.Lat,
                    Levels = request.Levels,
                    Lon = request.Lon,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    PostedInLastNumberOfDays = request.PostedInLastNumberOfDays,
                    RouteIds = request.RouteIds,
                    SearchTerm = request.SearchTerm,
                    SkipWageType = request.SkipWageType,
                    StandardLarsCode = request.StandardLarsCode,
                    Ukprn = request.Ukprn,
                    VacancySort = request.Sort ?? VacancySort.AgeDesc,
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
                    ApprenticeshipTypes = request.ApprenticeshipTypes,
                    Categories = request.Categories,
                    DataSources = request.DataSources,
                    DisabilityConfident = request.DisabilityConfident,
                    DistanceInMiles = request.DistanceInMiles,
                    ExcludeNational = request.ExcludeNational,
                    Lat = request.Lat,
                    Levels = request.Levels,
                    Lon = request.Lon,
                    RouteIds = request.RouteIds,
                    SearchTerm = request.SearchTerm,
                    WageType = request.WageType,
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