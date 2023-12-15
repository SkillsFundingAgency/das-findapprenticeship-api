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
            source.Course = null;
            source.Wage = null;
            
            var actual = (GetApprenticeshipVacancyDetailResponse)source;

            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.Duration)
                .Excluding(c=>c.DurationUnit)
                .Excluding(c=>c.Wage)
                .Excluding(c=>c.Course)
            );
        }
        [Test, AutoData]
        public void Then_Maps_Fields_From_Azure_Search(ApprenticeshipVacancyItem source)
        {
            source.Category = null;
            source.CategoryCode = null;
            source.Location.Lon = 0;
            source.Location.Lat = 0;
            source.StandardLarsCode = null;
            
            var response = (GetApprenticeshipVacancyDetailResponse)source;

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
            );
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
        public void Then_The_Expected_Duration_Is_Set_If_No_ExpectedDuration(int duration, string unit, string expectedText, ApprenticeshipVacancyItem source)
        {
            source.ExpectedDuration = null;
            source.Duration = duration;
            source.DurationUnit = unit;
            var response = (GetApprenticeshipVacancyDetailResponse)source;

            response.ExpectedDuration.Should().Be(expectedText);
        }
    }
}