using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

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
                .Setup(repository => repository.Count(query.AdditionalDataSources, query.WageType))
                .ReturnsAsync(vacancyCount);

            var result = await handler.Handle(query, CancellationToken.None);

            result
                .Should().Be(vacancyCount);
        }
    }
}