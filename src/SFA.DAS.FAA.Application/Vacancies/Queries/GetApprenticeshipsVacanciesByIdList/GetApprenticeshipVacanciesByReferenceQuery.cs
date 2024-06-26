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
            public string City { get; set; }
            public string Postcode { get; set; }
            public string ApplicationUrl { get; set; }
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
                        City = x.Address.AddressLine4,
                        Postcode = x.Address.Postcode,
                        ApplicationUrl = x.ApplicationUrl

                    }).ToList()
            };
        }
    }
}
