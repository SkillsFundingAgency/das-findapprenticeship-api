using System.Collections.Generic;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.ApiRequests
{
    public class GetVacanciesByReferenceRequest
    {
        public List<VacancyReference> VacancyReferences { get; set; }
    }
}
