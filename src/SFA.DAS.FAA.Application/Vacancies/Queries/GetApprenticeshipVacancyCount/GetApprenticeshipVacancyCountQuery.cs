using MediatR;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQuery : IRequest<int>
    {
        public SearchSource Source { get; set; }
        public List<AdditionalDataSource> AdditionalDataSources { get; set; }
    }
}