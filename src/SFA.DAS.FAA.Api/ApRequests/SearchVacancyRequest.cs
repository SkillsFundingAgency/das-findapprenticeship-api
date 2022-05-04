using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.ApRequests
{
    public class SearchVacancyRequest
    {
        [FromQuery]
        public int PageNumber { get; set; } = 1;
        [FromQuery]
        public int PageSize { get; set; } = 10;
        [FromQuery]
        public int? Ukprn { get; set; } = null;
        [FromQuery]
        public string AccountPublicHashedId  { get; set; } = null;
        [FromQuery]
        public string AccountLegalEntityPublicHashedId  { get; set; } = null;
        [FromQuery]
        public List<int> LarsCode  { get; set; } = null;
        [FromQuery]
        public bool? NationWideOnly  { get; set; } = null;
        [FromQuery]
        public double? Lat  { get; set; } = null;
        [FromQuery]
        public double? Lon  { get; set; } = null;
        [FromQuery]
        public uint? DistanceInMiles  { get; set; } = null;
        [FromQuery]
        public List<string> Categories  { get; set; } = null;
        [FromQuery]
        public uint? PostedInLastNumberOfDays  { get; set; } = null;
        [FromQuery]
        public VacancySort? Sort  { get; set; } = VacancySort.AgeDesc;
    }
}