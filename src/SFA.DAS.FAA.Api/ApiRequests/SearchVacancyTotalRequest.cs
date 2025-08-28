using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.Extensions;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.ApiRequests;

public class SearchVacancyTotalRequest
{
    [FromQuery]
    public string? SearchTerm { get; set; }
    [FromQuery]
    public bool? ExcludeNational { get; set; } = null;
    [FromQuery]
    public double? Lat { get; set; } = null;
    [FromQuery]
    public double? Lon { get; set; } = null;
    [FromQuery]
    public uint? DistanceInMiles { get; set; } = null;
    [FromQuery]
    [ModelBinder(typeof(NonNullListStringModelBinder))]
    public List<string> Categories { get; set; } = null;
    [FromQuery]
    public List<int> RouteIds { get; set; } = null;
    [FromQuery]
    [ModelBinder(typeof(NonNullListStringModelBinder))]
    public List<string> Levels { get; set; } = null;
    [FromQuery]
    public WageType? WageType { get; set; } = null;
    [FromQuery]
    public bool DisabilityConfident { get; set; }
    [FromQuery]
    public List<DataSource> DataSources { get; set; } = null;
    public List<ApprenticeshipTypes> ApprenticeshipTypes { get; set; } = null;
}