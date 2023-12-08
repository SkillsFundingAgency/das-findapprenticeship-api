using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.ApRequests;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.Controllers;

[ApiVersion("2.0")]
[ApiController]
[Route("/api/[controller]/")]
public class VacanciesV2Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public VacanciesV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Search([FromQuery] SearchVacancyRequest request)
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
            VacancySort = request.Sort ?? VacancySort.AgeDesc,
            Source = "ACS"
        });

        var apiResponse = (GetSearchApprenticeshipVacanciesResponse)result;

        return Ok(apiResponse);
    }
}
