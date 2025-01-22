using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Models
{
    public class FindVacanciesCountModel : FindFilteredVacanciesModel
    {
        public string? SearchTerm { get; init; }
        public int? Ukprn { get; init; }
        public List<string> Categories { get; set; }
        public List<int> RouteIds { get; set; }
        public List<string> Levels { get; init; }
        public bool DisabilityConfident { get; set; }
        public WageType? WageType { get; set; }
        public List<AdditionalDataSource> AdditionalDataSources { get; set; }
    }
}