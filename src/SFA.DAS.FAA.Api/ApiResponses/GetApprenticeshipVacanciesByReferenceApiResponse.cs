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
            public bool IsPrimaryLocation { get; set; }
            public Address Address { get; set; }
            public List<Address>? OtherAddresses { get; set; } = [];
            public string? EmploymentLocationInformation { get; set; }
            public string? AvailableWhere { get; set; }
        }

        public class Address
        {
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string Postcode { get; set; }

            public static implicit operator Address(GetApprenticeshipVacanciesByReferenceQueryResult.Address source)
            {
                if (source is null) return null;

                return new Address
                {
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    AddressLine3 = source.AddressLine3,
                    AddressLine4 = source.AddressLine4,
                    Postcode = source.Postcode
                };
            }
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
                    OtherAddresses = x.OtherAddresses?.Select(add => (Address)add).ToList(),
                    Address = x.Address,
                    IsPrimaryLocation = x.IsPrimaryLocation,
                    AvailableWhere = x.AvailableWhere,
                    EmploymentLocationInformation = x.EmploymentLocationInformation,
                })
            };
        }
    }
}
