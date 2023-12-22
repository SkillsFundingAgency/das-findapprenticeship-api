using System.Linq;
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
        public void Then_Maps_Fields(SearchApprenticeshipVacanciesResult source)
        {
            source.ApprenticeshipVacancies.ToList().ForEach(v => v.Wage = null);

            var response = (GetSearchApprenticeshipVacanciesResponse)source;

            response.Should().BeEquivalentTo(source, options=>options.Excluding(c=>c.ApprenticeshipVacancies));
            response.ApprenticeshipVacancies.Should().BeEquivalentTo(source.ApprenticeshipVacancies, options => options
                .Excluding(c => c.EmployerDescription)
                .Excluding(c => c.Duration)
                .Excluding(c => c.DurationUnit)
                .Excluding(c=>c.ExpectedDuration)
                .Excluding(c=>c.Wage)
                .Excluding(c=>c.Course)
                .Excluding(c=>c.SearchGeoPoint)
            );
        }
    }
}