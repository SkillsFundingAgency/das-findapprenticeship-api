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
            public string ApplicationUrl { get; set; }
            public Address Address { get; set; }
        }

        public class Address
        {
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string Postcode { get; set; }
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
                    ApplicationUrl = x.ApplicationUrl,
                    Address = new Address
                    {
                        AddressLine1 = x.Address.AddressLine1,
                        AddressLine2 = x.Address.AddressLine2,
                        AddressLine3 = x.Address.AddressLine3,
                        AddressLine4 = x.Address.AddressLine4,
                        Postcode = x.Address.Postcode
                    }
                })
            };
        }
    }
}
