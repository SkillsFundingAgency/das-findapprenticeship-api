using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

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
                .Excluding(c => c.TypicalJobTitles)
                .Excluding(c=>c.SearchGeoPoint)
                .Excluding(c => c.VacancySource)
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
                .Excluding(c=>c.WageType)
                .Excluding(c=>c.Course)
                .Excluding(c=>c.Category)
                .Excluding(c=>c.CategoryCode)
                .Excluding(c=>c.Location)
                .Excluding(c=>c.StandardLarsCode)
                .Excluding(c=>c.TypicalJobTitles)
                .Excluding(c => c.WageUnit)
                .Excluding(c=>c.SearchGeoPoint)
                .Excluding(c => c.VacancySource)
            );
            response.StandardTitle.Should().Be(source.Course.Title);
            response.Category.Should().Be(source.Course.Title);
            response.CategoryCode.Should().Be("SSAT1.UNKNOWN");
            response.StandardLarsCode.Should().Be(source.Course.LarsCode);
            response.RouteCode.Should().Be(source.Course.RouteCode);
            response.Location.Lon.Should().Be(source.Address.Longitude);
            response.Location.Lat.Should().Be(source.Address.Latitude);
            response.WageUnit.Should().Be(4);
            response.WageType.Should().Be((int)source.Wage.WageType);
        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_From_Azure_Search_For_No_Location(ApprenticeshipVacancyItem source)
        {
            source.Category = null;
            source.CategoryCode = null;
            source.Location.Lon = 0;
            source.Location.Lat = 0;
            source.StandardLarsCode = null;
            source.SearchGeoPoint = null;
            source.Distance = null;
            
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
                .Excluding(c => c.WageType)
                .Excluding(c => c.WageUnit)
                .Excluding(c=>c.SearchGeoPoint)
                .Excluding(c=>c.Distance)
                .Excluding(c => c.TypicalJobTitles)
                .Excluding(c => c.VacancySource)
            );
            response.StandardTitle.Should().Be(source.Course.Title);
            response.Category.Should().Be(source.Course.Title);
            response.CategoryCode.Should().Be("SSAT1.UNKNOWN");
            response.StandardLarsCode.Should().Be(source.Course.LarsCode);
            response.RouteCode.Should().Be(source.Course.RouteCode);
            response.Location.Lon.Should().Be(source.Address.Longitude);
            response.Location.Lat.Should().Be(source.Address.Latitude);
            response.WageUnit.Should().Be(4);
            response.Distance.Should().Be(0);
            response.WageType.Should().Be((int)source.Wage.WageType);
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
            source.Wage = null;
            var response = (GetApprenticeshipVacancyDetailResponse)source;

            response.ExpectedDuration.Should().Be(expectedText);
        }
        
        [Test]
        [InlineAutoData(1, WageUnit.Year, "1 Year")]
        [InlineAutoData(3, WageUnit.Month, "3 Months")]
        [InlineAutoData(3, WageUnit.Week, "3 Weeks")]
        public void Then_The_Expected_Duration_Is_Set_For_Azure(int duration, WageUnit unit, string expectedText, ApprenticeshipVacancyItem source)
        {
            source.ExpectedDuration = null;
            source.Duration = 0;
            source.DurationUnit = null;
            source.Wage = new WageSearchDocument
            {
                WageUnit = unit,
                Duration = duration
            };
            var response = (GetApprenticeshipVacancyDetailResponse)source;

            response.ExpectedDuration.Should().Be(expectedText);
        }
    }
}