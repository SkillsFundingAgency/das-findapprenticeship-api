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
            source.ExpectedDuration = null;
            source.Course = null;
            source.Wage = null;
            
            var response = (GetApprenticeshipVacancyResponse)source;

            response.Should().BeEquivalentTo(source, options=>options
                .Excluding(c=>c.Duration)
                .Excluding(c=>c.DurationUnit)
                .Excluding(c=>c.EmployerDescription)
                .Excluding(c=>c.ExpectedDuration)
                .Excluding(c=>c.Wage)
                .Excluding(c=>c.Course)
                .Excluding(c => c.TypicalJobTitles)
            );
            response.ExpectedDuration.Should().Be($"{source.Duration} {(source.Duration == 1 ? source.DurationUnit : $"{source.DurationUnit}s")}");
        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_From_Azure_Search(ApprenticeshipSearchItem source)
        {
            source.ExpectedDuration = null;
            source.Category = null;
            source.CategoryCode = null;
            source.Location.Lon = 0;
            source.Location.Lat = 0;
            source.StandardLarsCode = null;
            
            var response = (GetApprenticeshipVacancyResponse)source;

            response.Should().BeEquivalentTo(source, options=>options
                .Excluding(c=>c.Duration)
                .Excluding(c=>c.DurationUnit)
                .Excluding(c=>c.EmployerDescription)
                .Excluding(c=>c.ExpectedDuration)
                .Excluding(c=>c.Wage)
                .Excluding(c=>c.Course)
                .Excluding(c=>c.Category)
                .Excluding(c=>c.CategoryCode)
                .Excluding(c=>c.Location)
                .Excluding(c=>c.StandardLarsCode)
                .Excluding(c => c.TypicalJobTitles)
            );
            response.ExpectedDuration.Should().Be($"{source.Duration} {(source.Duration == 1 ? source.DurationUnit : $"{source.DurationUnit}s")}");
            response.StandardTitle.Should().Be(source.Course.Title);
            response.Category.Should().Be(source.Course.Title);
            response.CategoryCode.Should().Be("SSAT1.UNKNOWN");
            response.StandardLarsCode.Should().Be(source.Course.LarsCode);
            response.RouteCode.Should().Be(source.Course.RouteCode);
            response.Location.Lon.Should().Be(source.Address.Longitude);
            response.Location.Lat.Should().Be(source.Address.Latitude);
        }

        [Test]
        [InlineAutoData(1, "year", "1 year")]
        [InlineAutoData(3, "month", "3 months")]
        [InlineAutoData(3, "weeks", "3 weeks")]
        [InlineAutoData(3, null, "3 ")]
        public void Then_The_Expected_Duration_Is_Set(int duration, string unit, string expectedText, ApprenticeshipSearchItem source)
        {
            source.ExpectedDuration = null;
            source.Duration = duration;
            source.DurationUnit = unit;
            
            var response = (GetApprenticeshipVacancyResponse)source;

            response.ExpectedDuration.Should().Be(expectedText);
        }

        [Test, AutoData]
        public void Then_If_ExpectedDuration_Then_Used(ApprenticeshipSearchItem source)
        {
            var response = (GetApprenticeshipVacancyResponse)source;

            response.Should().BeEquivalentTo(source, options=>options
                .Excluding(c=>c.Duration)
                .Excluding(c=>c.DurationUnit)
                .Excluding(c=>c.EmployerDescription)
                .Excluding(c=>c.Wage)
                .Excluding(c=>c.TypicalJobTitles)
                .Excluding(c=>c.Course)
            );
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