using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    public class WhenGettingApprenticeshipVacancyCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_AzureSearch(
            GetApprenticeshipVacancyCountQuery query,
            int vacancyCount,
            [Frozen] Mock<IAcsVacancySearchRepository> mockRepository,
            GetApprenticeshipVacancyCountQueryHandler handler)
        {
            mockRepository
                .Setup(repository => repository.Count(It.Is<FindVacanciesCountModel>(c =>
                    c.Categories.Equals(query.Categories) &&
                    c.Levels.Equals(query.Levels) &&
                    c.Lat.Equals(query.Lat) &&
                    c.Lon.Equals(query.Lon) &&
                    c.DistanceInMiles.Equals(query.DistanceInMiles) &&
                    c.NationWideOnly.Equals(query.NationWideOnly) &&
                    c.SearchTerm.Equals(query.SearchTerm) &&
                    c.WageType.Equals(query.WageType) &&
                    c.DisabilityConfident.Equals(query.DisabilityConfident) &&
                    c.AdditionalDataSources.Equals(query.AdditionalDataSources)
                )))
                .ReturnsAsync(vacancyCount);

            var result = await handler.Handle(query, CancellationToken.None);

            result
                .Should().Be(vacancyCount);
        }
    }
}