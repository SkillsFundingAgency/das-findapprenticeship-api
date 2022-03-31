using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.UnitTests.ApiResponses
{
    public class WhenCastingToGetApprenticeshipVacancyDetailResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(ApprenticeshipVacancyItem source)
        {
            var actual = (GetApprenticeshipVacancyDetailResponse)source;

            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.Duration)
                .Excluding(c=>c.DurationUnit)
            );
        }
        
        
        [Test]
        [InlineAutoData(1, "year", "1 year")]
        [InlineAutoData(3, "month", "3 months")]
        [InlineAutoData(3, "weeks", "3 weeks")]
        public void Then_The_Expected_Duration_Is_Set(int duration, string unit, string expectedText, ApprenticeshipVacancyItem source)
        {
            source.Duration = duration;
            source.DurationUnit = unit;
            var response = (GetApprenticeshipVacancyDetailResponse)source;

            response.ExpectedDuration.Should().Be(expectedText);
        }
    }
}