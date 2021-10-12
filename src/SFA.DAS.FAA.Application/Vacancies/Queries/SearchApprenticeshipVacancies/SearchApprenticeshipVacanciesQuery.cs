﻿using MediatR;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQuery : IRequest<SearchApprenticeshipVacanciesResult>
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}