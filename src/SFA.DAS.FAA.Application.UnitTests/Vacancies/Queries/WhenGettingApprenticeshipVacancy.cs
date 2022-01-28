using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    public class WhenGettingApprenticeshipVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Repository(
            GetApprenticeshipVacancyQuery query,
            ApprenticeshipVacancyItem responseFromRepository,
            [Frozen] Mock<IVacancySearchRepository> mockVacancyIndexRepository,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            mockVacancyIndexRepository
                .Setup(repository => repository.Get(query.VacancyReference))
                .ReturnsAsync(responseFromRepository);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.ApprenticeshipVacancy
                .Should().BeEquivalentTo(responseFromRepository);
        }
    }
}