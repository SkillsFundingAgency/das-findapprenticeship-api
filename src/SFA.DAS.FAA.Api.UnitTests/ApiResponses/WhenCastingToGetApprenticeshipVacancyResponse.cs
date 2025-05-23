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

            response.Should().BeEquivalentTo(source, options => options
                .ExcludingMissingMembers()
                .Excluding(c => c.ExpectedDuration)
            );
            response.ExpectedDuration.Should().Be($"{source.Duration} {(source.Duration == 1 ? source.DurationUnit : $"{source.DurationUnit}s")}");
        }

        [Test, AutoData]
        public void Then_If_No_Location_Search_Maps_Zero_Distance_For_Azure_Search(ApprenticeshipSearchItem source)
        {
            source.ExpectedDuration = null;
            source.Category = null;
            source.CategoryCode = null;
            source.Location.Lon = 0;
            source.Location.Lat = 0;
            source.Address.Latitude = 54.6;
            source.Address.Longitude = -2.44;
            source.StandardLarsCode = null;
            source.SearchGeoPoint = null;
            source.Distance = null;
            
            var response = (GetApprenticeshipVacancyResponse)source;

            response.Should().BeEquivalentTo(source, options => options
                .ExcludingMissingMembers()
                .Excluding(c => c.ExpectedDuration)
                .Excluding(c => c.Category)
                .Excluding(c => c.CategoryCode)
                .Excluding(c => c.Location)
                .Excluding(c => c.StandardLarsCode)
                .Excluding(c => c.WageType)
                .Excluding(c => c.WageUnit)
                .Excluding(c => c.Distance)
            );

            response.Distance.Should().Be(0);
        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_From_Azure_Search_With_Distance(ApprenticeshipSearchItem source)
        {
            source.ExpectedDuration = null;
            source.Category = null;
            source.CategoryCode = null;
            source.Location.Lon = 0;
            source.Location.Lat = 0;
            source.Address.Latitude = 54.6;
            source.Address.Longitude = -2.44;
            source.StandardLarsCode = null;
            source.SearchGeoPoint.Lat = 54.5;
            source.SearchGeoPoint.Lon = -2.43;
            source.Distance = null;
            
            var response = (GetApprenticeshipVacancyResponse)source;

            response.Should().BeEquivalentTo(source, options => options
                .ExcludingMissingMembers()
                .Excluding(c => c.ExpectedDuration)
                .Excluding(c => c.Category)
                .Excluding(c => c.CategoryCode)
                .Excluding(c => c.Location)
                .Excluding(c => c.StandardLarsCode)
                .Excluding(c => c.WageType)
                .Excluding(c => c.WageUnit)
                .Excluding(c => c.Distance)
            );
            response.ExpectedDuration.Should().Be($"{source.Duration} {(source.Duration == 1 ? source.DurationUnit : $"{source.DurationUnit}s")}");
            response.StandardTitle.Should().Be(source.Course.Title);
            response.Category.Should().Be(source.Course.Title);
            response.CategoryCode.Should().Be("SSAT1.UNKNOWN");
            response.StandardLarsCode.Should().Be(source.Course.LarsCode);
            response.RouteCode.Should().Be(source.Course.RouteCode);
            response.Location.Lon.Should().Be(source.Address.Longitude);
            response.Location.Lat.Should().Be(source.Address.Latitude);
            response.WageUnit.Should().Be(4);
            response.WageType.Should().Be((int)source.Wage.WageType);
            response.ApprenticeMinimumWage.Should().Be(source.Wage.ApprenticeMinimumWage);
            response.Over25NationalMinimumWage.Should().Be(source.Wage.Over25NationalMinimumWage);
            response.Under18NationalMinimumWage.Should().Be(source.Wage.Under18NationalMinimumWage);
            response.Between18AndUnder21NationalMinimumWage.Should().Be(source.Wage.Between18AndUnder21NationalMinimumWage);
            response.Between21AndUnder25NationalMinimumWage.Should().Be(source.Wage.Between21AndUnder25NationalMinimumWage);
            response.Distance.Should().Be(6.92865669536914M);
        }

        [Test]
        [InlineAutoData(1, "year", "1 year")]
        [InlineAutoData(3, "month", "3 months")]
        [InlineAutoData(3, "weeks", "3 weeks")]
        [InlineAutoData(3, null, "3 ")]
        public void Then_The_Expected_Duration_Is_Set(int duration, string unit, string expectedText, ApprenticeshipSearchItem source)
        {
            source.Wage = null;
            source.ExpectedDuration = null;
            source.Duration = duration;
            source.DurationUnit = unit;
            
            var response = (GetApprenticeshipVacancyResponse)source;

            response.ExpectedDuration.Should().Be(expectedText);
        }

        [Test, AutoData]
        public void Then_If_Has_ExpectedDuration_Then_Used(ApprenticeshipSearchItem source)
        {
            var response = (GetApprenticeshipVacancyResponse)source;

            response.Should().BeEquivalentTo(source, options=>options
                .ExcludingMissingMembers()
                .Excluding(c => c.WageType)
                .Excluding(c => c.WageUnit)
            );
            response.WageUnit.Should().Be(4);
            response.WageType.Should().Be((int)source.Wage.WageType);
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