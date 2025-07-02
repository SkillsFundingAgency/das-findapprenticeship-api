using MediatR;
using SFA.DAS.Common.Domain.Models;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQuery : IRequest<GetApprenticeshipVacancyResult>
    {
        public VacancyReference VacancyReference { get; init; }
    }
}