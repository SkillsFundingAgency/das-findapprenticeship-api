using SFA.DAS.FAA.Application.UnitTests.Extensions;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    public class WhenGettingApprenticeshipVacancy
    {
        [Test, AutoDataWithValidVacancyReference]
        public async Task Then_Gets_Vacancies_From_Azure_Repository_When_Source_Is_Azure(
            GetApprenticeshipVacancyQuery query,
            ApprenticeshipVacancyItem responseFromRepository,
            [Frozen] Mock<IAcsVacancySearchRepository> mockVacancyIndexRepository,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            mockVacancyIndexRepository
                .Setup(repository => repository.Get(query.VacancyReference))
                .ReturnsAsync(responseFromRepository);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy
                .Should().BeEquivalentTo(responseFromRepository);
        }

        [Test, AutoDataWithValidVacancyReference]
        public async Task Then_Get_Vacancy_Is_Null_And_Match_By_Reference_From_Azure_Repository_When_Source_Is_Azure(
            GetApprenticeshipVacancyQuery query,
            List<ApprenticeshipSearchItem> mockApprenticeshipSearchItems,
            [Frozen] Mock<IAcsVacancySearchRepository> mockVacancyIndexRepository,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            mockVacancyIndexRepository
                .Setup(repository => repository.Get(query.VacancyReference))
                .ReturnsAsync((ApprenticeshipVacancyItem)null);

            mockVacancyIndexRepository
                .Setup(repository => repository.GetAll(It.Is<List<VacancyReference>>(refs => refs.SequenceEqual(new List<VacancyReference> { query.VacancyReference.ToShortString() }))))
                .ReturnsAsync(mockApprenticeshipSearchItems);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo(mockApprenticeshipSearchItems.FirstOrDefault());
        }

        [Test, AutoDataWithValidVacancyReference]
        public async Task Then_Get_Vacancy_Is_Null_And_No_Match_By_Reference_From_Azure_Repository_When_Source_Is_Null(
            GetApprenticeshipVacancyQuery query,
            [Frozen] Mock<IAcsVacancySearchRepository> mockVacancyIndexRepository,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            mockVacancyIndexRepository
                .Setup(repository => repository.Get(query.VacancyReference))
                .ReturnsAsync((ApprenticeshipVacancyItem)null);

            mockVacancyIndexRepository
                .Setup(repository => repository.GetAll(It.Is<List<VacancyReference>>(refs => refs.SequenceEqual(new List<VacancyReference> { query.VacancyReference.ToShortString() }))))
                .ReturnsAsync((List<ApprenticeshipSearchItem>)null);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeNull();
        }
    }
}