using MediatR;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQuery : IRequest<int>
    {
        public string? SearchTerm { get; init; }
        public bool? ExcludeNational { get; init; }
        public uint? DistanceInMiles { get; init; }
        public List<string> Categories { get; init; }
        public List<int> RouteIds { get; init; }
        public List<string> Levels { get; init; }
        public double? Lat { get; init; }
        public double? Lon { get; init; }
        public WageType? WageType { get; init; }
        public bool DisabilityConfident { get; set; }
        public List<DataSource> DataSources { get; set; }
        public List<ApprenticeshipTypes> ApprenticeshipTypes { get; set; }
    }
}