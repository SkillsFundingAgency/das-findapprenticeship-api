using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.Extensions;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.FAA.Api.ApiRequests;

public class SearchVacancyTotalRequest
{
    [FromQuery]
    public string? SearchTerm { get; set; }
    [FromQuery]
    public bool? NationWideOnly { get; set; } = null;
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
    [ModelBinder(typeof(NonNullListStringModelBinder))]
    public List<string> Levels { get; set; } = null;
    [FromQuery]
    public WageType? WageType { get; set; } = null;
    [FromQuery]
    public bool DisabilityConfident { get; set; }
    [FromQuery]
    public List<AdditionalDataSource> AdditionalDataSources { get; set; } = null;
}