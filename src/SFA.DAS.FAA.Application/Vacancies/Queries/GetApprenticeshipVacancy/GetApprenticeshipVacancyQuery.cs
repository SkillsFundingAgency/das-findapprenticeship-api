using MediatR;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQuery : IRequest<GetApprenticeshipVacancyResult>
    {
        public string VacancyReference { get; set; }   
    }
}