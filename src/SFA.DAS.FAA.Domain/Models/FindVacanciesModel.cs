using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Models
{
    public class FindVacanciesModel : FindFilteredVacanciesModel
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; set; }
        public int PageSize { get; init; }
        public int? Ukprn { get; init; }
        public string AccountPublicHashedId { get; init; }
        public string AccountLegalEntityPublicHashedId { get; init; }
        public string EmployerName { get; init; }
        public List<int> StandardLarsCode { get; set; }
        public List<string> Categories { get; set; }
        public List<int> RouteIds { get; set; }
        public List<string> Levels { get; init; }
        public uint? PostedInLastNumberOfDays { get; set; }
        public VacancySort VacancySort { get; set; }
        public bool DisabilityConfident { get; set; }
        public WageType? SkipWageType { get; set; }
        public List<DataSource> AdditionalDataSources { get; set; }
    }

    public class FindFilteredVacanciesModel
    {
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public uint? DistanceInMiles { get; set; }
        public bool? ExcludeNational { get; init; }
    }
}