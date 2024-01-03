using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SFA.DAS.FAA.Api.ApiRequests;

public class SearchVacancyTotalRequest
{
    [FromQuery]
    public int? Ukprn { get; set; } = null;
    [FromQuery]
    public string AccountPublicHashedId { get; set; } = null;
    [FromQuery]
    public string AccountLegalEntityPublicHashedId { get; set; } = null;
    [FromQuery]
    public List<int> StandardLarsCode { get; set; } = null;
    [FromQuery]
    public bool? NationWideOnly { get; set; } = null;
    [FromQuery]
    public double? Lat { get; set; } = null;
    [FromQuery]
    public double? Lon { get; set; } = null;
    [FromQuery]
    public uint? DistanceInMiles { get; set; } = null;
    [FromQuery]
    public List<string> Categories { get; set; } = null;
    [FromQuery]
    public uint? PostedInLastNumberOfDays { get; set; } = null;
}