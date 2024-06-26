using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipsVacanciesByIdList;

namespace SFA.DAS.FAA.Api.ApiResponses
{
    public class GetApprenticeshipVacanciesByReferenceApiResponse
    {
        public IEnumerable<ApprenticeshipVacancy> ApprenticeshipVacancies { get; set; }

        public class ApprenticeshipVacancy
        {
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public string Title { get; set; }
            public DateTime ClosingDate { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
            public string ApplicationUrl { get; set; }
        }

        public static implicit operator GetApprenticeshipVacanciesByReferenceApiResponse(
            GetApprenticeshipVacanciesByReferenceQueryResult source)
        {
            return new GetApprenticeshipVacanciesByReferenceApiResponse
            {
                ApprenticeshipVacancies = source.ApprenticeshipVacancies.Select(x => new ApprenticeshipVacancy
                {
                    EmployerName = x.EmployerName,
                    VacancyReference = x.VacancyReference,
                    Title = x.Title,
                    ClosingDate = x.ClosingDate,
                    City = x.City,
                    Postcode = x.Postcode,
                    ApplicationUrl = x.ApplicationUrl
                })
            };
        }
    }
}
