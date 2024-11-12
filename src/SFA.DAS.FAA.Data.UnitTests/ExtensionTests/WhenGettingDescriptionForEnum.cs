using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Data.Extensions;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Data.UnitTests.ExtensionTests
{
    [TestFixture]
    public class WhenGettingDescriptionForEnum
    {
        [Test]
        [MoqInlineAutoData(WageType.CompetitiveSalary, "Competitive Salary")]
        [MoqInlineAutoData(WageType.Unknown, "Unknown")]
        [MoqInlineAutoData(WageType.NationalMinimumWage, "National Minimum Wage")]
        [MoqInlineAutoData(WageType.NationalMinimumWageForApprentices, "National Minimum Wage For Apprentices")]
        [MoqInlineAutoData(WageType.FixedWage, "Fixed Wage")]
        public void Then_The_Description_Is_Returned_Correctly(
            WageType wageType,
            string expectedDescription)
        {
            // sut
            var sut = wageType.GetDescription();

            // assert
            sut.Should().Be(expectedDescription);
        }
    }
}
