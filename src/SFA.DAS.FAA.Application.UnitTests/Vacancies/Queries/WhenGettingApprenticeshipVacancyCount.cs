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
        public async Task Then_Gets_Vacancies_From_Repository(
            GetApprenticeshipVacancyCountQuery query,
            int vacancyCount,
            [Frozen] Mock<IVacancySearchRepository> mockVacancyIndexRepository,
            GetApprenticeshipVacancyCountQueryHandler handler)
        {
            mockVacancyIndexRepository
                .Setup(repository => repository.Count())
                .ReturnsAsync(vacancyCount);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result
                .Should().Be(vacancyCount);
        }
    }
}