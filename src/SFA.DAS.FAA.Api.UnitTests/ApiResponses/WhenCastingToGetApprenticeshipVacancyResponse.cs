using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.UnitTests.ApiResponses
{
    public class WhenCastingToGetApprenticeshipVacancyResponse
    {
        [Test, AutoData]
        public void Then_Maps_Fields(ApprenticeshipSearchItem source)
        {
            var response = (GetApprenticeshipVacancyResponse)source;

            response.Should().BeEquivalentTo(source, options=>options
                .Excluding(c=>c.Duration)
                .Excluding(c=>c.DurationUnit)
                .Excluding(c=>c.EmployerDescription)
            );
            response.ExpectedDuration.Should().Be($"{source.Duration} {(source.Duration == 1 ? source.DurationUnit : $"{source.DurationUnit}s")}");
        }

        [Test]
        [InlineAutoData(1, "year", "1 year")]
        [InlineAutoData(3, "month", "3 months")]
        public void Then_The_Expected_Duration_Is_Set(int duration, string unit, string expectedText, ApprenticeshipSearchItem source)
        {
            source.Duration = duration;
            source.DurationUnit = unit;
            var response = (GetApprenticeshipVacancyResponse)source;

            response.ExpectedDuration.Should().Be(expectedText);
        }
        
        
        [Test, AutoData]
        public void Then_If_No_Address_Then_Null_Returned(ApprenticeshipVacancyItem source)
        {
            source.Address = null;
            
            var actual = (GetApprenticeshipVacancyResponse)source;

            actual.Address.Should().BeNull();
        }
    }
}