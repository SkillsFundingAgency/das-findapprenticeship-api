using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;

namespace SFA.DAS.FAA.Api.UnitTests.ApiResponses
{
    public class WhenCastingToGetSearchApprenticeshipVacanciesResponse
    {
        [Test, AutoData]
        public void Then_Maps_Fields(
            SearchApprenticeshipVacanciesResult source)
        {
            var response = (GetSearchApprenticeshipVacanciesResponse)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}