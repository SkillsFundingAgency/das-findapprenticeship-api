using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    public class WhenSearchingApprenticeshipVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Repository(
            SearchApprenticeshipVacanciesQuery query,
            ApprenticeshipSearchResponse responseFromRepository,
            [Frozen] Mock<IVacancySearchRepository> mockVacancyIndexRepository,
            SearchApprenticeshipVacanciesQueryHandler handler)
        {
            mockVacancyIndexRepository
                .Setup(repository => repository.Find(It.Is<FindVacanciesModel>(c=> 
                        c.PageNumber.Equals(query.PageNumber) &&
                        c.PageSize.Equals(query.PageSize) &&
                        c.Ukprn.Equals(query.Ukprn) &&
                        c.AccountPublicHashedId.Equals(query.AccountPublicHashedId) &&
                        c.AccountLegalEntityPublicHashedId.Equals(query.AccountLegalEntityPublicHashedId) &&
                        c.StandardLarsCode.Equals(query.StandardLarsCode) &&
                        c.Categories.Equals(query.Categories) &&
                        c.Lat.Equals(query.Lat) &&
                        c.Lon.Equals(query.Lon) &&
                        c.DistanceInMiles.Equals(query.DistanceInMiles) &&
                        c.NationWideOnly.Equals(query.NationWideOnly) &&
                        c.PostedInLastNumberOfDays.Equals(query.PostedInLastNumberOfDays) &&
                        c.VacancySort.Equals(query.VacancySort)
                        )))
                .ReturnsAsync(responseFromRepository);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.ApprenticeshipVacancies
                .Should().BeEquivalentTo(responseFromRepository.ApprenticeshipVacancies);
            result.TotalFound.Should().Be(responseFromRepository.TotalFound);
            result.Total.Should().Be(responseFromRepository.Total);
        }
    }
}