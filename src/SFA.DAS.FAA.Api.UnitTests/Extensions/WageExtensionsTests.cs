using SFA.DAS.FAA.Api.Extensions;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.UnitTests.Extensions;
[TestFixture]
internal class WageExtensionsTests
{
    private const string MonthUnit = "Month"; 

    [Test]
    public void GetDuration_WhenExpectedDurationIsProvided_ReturnsExpectedDuration()
    {
        var item = new ApprenticeshipSearchItem
        {
            ExpectedDuration = "12 Months"
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("12 Months");
    }

    [Test]
    public void GetDuration_WhenDurationUnitIsMonth_AndDurationIsOnlyMonths_ReturnsMonths()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 6,
            DurationUnit = MonthUnit
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("6 Months");
    }

    [Test]
    public void GetDuration_WhenDurationUnitIsMonth_AndIsOneMonth_ReturnsSingularMonth()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 1,
            DurationUnit = MonthUnit
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("1 Month");
    }

    [Test]
    public void GetDuration_WhenDurationIs12Months_ReturnsOneYear()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 12,
            DurationUnit = MonthUnit
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("1 Year");
    }

    [Test]
    public void GetDuration_WhenDurationIs18Months_ReturnsOneYearSixMonths()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 18,
            DurationUnit = MonthUnit
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("1 Year 6 Months");
    }

    [Test]
    public void GetDuration_WhenDurationIs30Months_ReturnsTwoYearsSixMonths()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 30,
            DurationUnit = MonthUnit
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("2 Years 6 Months");
    }

    [Test]
    public void GetDuration_WhenDurationUnitIsNotMonth_SingularCase()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 1,
            DurationUnit = "Week"
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("1 Week");
    }

    [Test]
    public void GetDuration_WhenDurationUnitIsNotMonth_PluralCase()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 4,
            DurationUnit = "Week"
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("4 Weeks");
    }

    [Test]
    public void GetDuration_WhenDurationUnitAlreadyPlural_DoesNotAddExtraS()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 4,
            DurationUnit = "Hours"
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("4 Hours");
    }

    [Test]
    public void GetDuration_WhenDurationIsZero_UsesWageDuration()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 0,
            DurationUnit = "Week",
            Wage = new WageSearchDocument { Duration = 5 }
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("5 Weeks");
    }

    [Test]
    public void GetDuration_WhenDurationUnitIsNull_UsesWageUnit()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 3,
            Wage = new WageSearchDocument
            {
                Duration = 3,
                WageUnit = WageUnit.Week
            }
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("3 Weeks");
    }

    [Test]
    public void GetDuration_WhenUnitIsMissing_ReturnsJustDuration()
    {
        var item = new ApprenticeshipSearchItem
        {
            Duration = 5,
            DurationUnit = null
        };

        var result = WageExtensions.GetDuration(item);

        result.Should().Be("5");
    }
}
