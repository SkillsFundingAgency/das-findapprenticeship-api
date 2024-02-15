using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    public class WhenGettingApprenticeshipVacancyCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Repository(
            GetApprenticeshipVacancyCountQuery query,
            int vacancyCount,
            [Frozen] Mock<IVacancySearchRepository> mockRepository,
            GetApprenticeshipVacancyCountQueryHandler handler)
        {
            query.Source = SearchSource.Elastic;

            mockRepository
                .Setup(repository => repository.Count())
                .ReturnsAsync(vacancyCount);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result
                .Should().Be(vacancyCount);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_AzureSearch(
            GetApprenticeshipVacancyCountQuery query,
            int vacancyCount,
            [Frozen] Mock<IAcsVacancySearchRepository> mockRepository,
            GetApprenticeshipVacancyCountQueryHandler handler)
        {
            query.Source = SearchSource.AzureSearch;

            mockRepository
                .Setup(repository => repository.Count())
                .ReturnsAsync(vacancyCount);

            var result = await handler.Handle(query, CancellationToken.None);

            result
                .Should().Be(vacancyCount);
        }
    }
}