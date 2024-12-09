using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipsVacanciesByIdList
{
    public class GetApprenticeshipVacanciesByReferenceQuery : IRequest<GetApprenticeshipVacanciesByReferenceQueryResult>
    {
        public List<string> VacancyReferences { get; set; }
    }

    public class GetApprenticeshipVacanciesByReferenceQueryResult
    {
        public List<ApprenticeshipVacancy> ApprenticeshipVacancies { get; set; }

        public class ApprenticeshipVacancy
        {
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public string Title { get; set; }
            public DateTime ClosingDate { get; set; }
            public string ApplicationUrl { get; set; }
            public bool IsPrimaryLocation { get; set; }
            public Address Address { get; set; }
            public List<Address> OtherAddresses { get; set; }
        }

        public class Address
        {
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string Postcode { get; set; }

            public static implicit operator Address(Domain.Entities.Address source)
            {
                return new Address
                {
                    AddressLine4 = source.AddressLine4,
                    AddressLine3 = source.AddressLine3,
                    AddressLine2 = source.AddressLine2,
                    AddressLine1 = source.AddressLine1,
                    Postcode = source.Postcode
                };
            }
        }
    }

    public class GetApprenticeshipVacanciesByReferenceQueryHandler(IAzureSearchHelper searchHelper) : IRequestHandler<GetApprenticeshipVacanciesByReferenceQuery, GetApprenticeshipVacanciesByReferenceQueryResult>
    {
        public async Task<GetApprenticeshipVacanciesByReferenceQueryResult> Handle(GetApprenticeshipVacanciesByReferenceQuery request, CancellationToken cancellationToken)
        {
            var results = await searchHelper.Get(request.VacancyReferences);

            return new GetApprenticeshipVacanciesByReferenceQueryResult
            {
                ApprenticeshipVacancies = results.Select(x =>
                    new GetApprenticeshipVacanciesByReferenceQueryResult.ApprenticeshipVacancy
                    {
                        EmployerName = x.EmployerName,
                        VacancyReference = x.VacancyReference,
                        Title = x.Title,
                        ClosingDate = x.ClosingDate,
                        IsPrimaryLocation = x.IsPrimaryLocation,
                        Address = x.Address,
                        OtherAddresses = x.OtherAddresses.Select(add => (GetApprenticeshipVacanciesByReferenceQueryResult.Address)add).ToList(),
                        ApplicationUrl = x.ApplicationUrl

                    }).ToList()
            };
        }
    }
}
