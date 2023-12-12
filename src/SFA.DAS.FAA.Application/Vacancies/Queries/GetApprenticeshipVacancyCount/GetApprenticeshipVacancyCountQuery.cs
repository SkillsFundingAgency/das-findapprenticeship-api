using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQuery : IRequest<int>
    {
        public int? Ukprn { get; set; }
        public string AccountPublicHashedId { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public List<int> StandardLarsCode { get; set; }
        public bool? NationWideOnly { get; set; }
        public uint? DistanceInMiles { get; set; }
        public uint? PostedInLastNumberOfDays { get; set; }
        public List<string> Categories { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }

}