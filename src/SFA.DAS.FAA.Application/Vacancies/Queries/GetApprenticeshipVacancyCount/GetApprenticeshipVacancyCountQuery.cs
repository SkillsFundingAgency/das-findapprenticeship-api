using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQuery : IRequest<int>
    {
        public bool NationalSearch { get; set; }
        public string? location { get; set; }
        public List<string>? SelectedRouteIds { get; set; }
    }

}