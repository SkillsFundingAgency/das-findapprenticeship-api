using System.Collections.Generic;
using MediatR;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQuery : IRequest<SearchApprenticeshipVacanciesResult>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int? Ukprn { get; init; }
        public string AccountPublicHashedId { get; init; }
        public string AccountLegalEntityPublicHashedId { get ; init ; }
        public List<int> StandardLarsCode { get ; init ; }
        public bool? NationWideOnly { get ; init ; }
        public uint? DistanceInMiles { get ; init ; }
        public uint? PostedInLastNumberOfDays { get ; init ; }
        public List<string> Categories { get ; init ; }
        public List<string> Levels { get; init; }
        public double? Lat { get ; init ; }
        public double? Lon { get ; init ; }
        public VacancySort VacancySort { get ; init ; }
        public WageType? WageType { get ; init ; }
        public bool DisabilityConfident { get; set; }
        public List<AdditionalDataSource> AdditionalDataSources { get; set; }
    }
}