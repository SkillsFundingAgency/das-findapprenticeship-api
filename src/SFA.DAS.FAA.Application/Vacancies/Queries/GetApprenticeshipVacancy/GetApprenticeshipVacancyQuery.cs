using MediatR;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQuery : IRequest<GetApprenticeshipVacancyResult>
    {
        public VacancyReference VacancyReference { get; init; }
    }
}