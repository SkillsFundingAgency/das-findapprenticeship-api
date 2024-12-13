using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    [TestFixture]
    public class WhenMappingApprenticeSearchItemToApprenticeshipVacancyItem
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(ApprenticeshipSearchItem source)
        {
            // Act
            var result = ApprenticeshipVacancyItem.FromApprenticeshipSearchItem(source);

            result.Should().BeEquivalentTo(source);
        }
    }
}