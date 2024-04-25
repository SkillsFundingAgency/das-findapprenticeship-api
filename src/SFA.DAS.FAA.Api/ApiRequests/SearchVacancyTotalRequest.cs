using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.FAA.Api.ApiRequests;

public class SearchVacancyTotalRequest
{
    [FromQuery]
    public List<AdditionalDataSource> AdditionalDataSources { get; set; } = null;
}