using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetSavedSearches;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearch
{
    [TestFixture]
    public class WhenHandlingGetSavedSearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Azure_Repository_When_Source_Is_Azure(
            List<string> vacancyReferences,
            GetSavedSearchesQueryResult.SearchParameters searchParameters,
            GetSavedSearchesQuery query,
            List<SavedSearchEntity> savedSearchEntities,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
            GetSavedSearchesQueryHandler handler)
        {
            //arrange
            foreach (var savedSearchEntity in savedSearchEntities)
            {
                savedSearchEntity.SearchParameters = JsonConvert.SerializeObject(searchParameters);
                savedSearchEntity.VacancyRefs = JsonConvert.SerializeObject(vacancyReferences);

            }
            savedSearchRepository
                .Setup(repository => repository.GetAll(query.LastRunDateFilter, CancellationToken.None))
                .ReturnsAsync(savedSearchEntities);

            var result = await handler.Handle(query, CancellationToken.None);

            result.SavedSearches
                .Should().BeEquivalentTo(savedSearchEntities, options => options
                    .Excluding(ex => ex.SearchParameters)
                    .Excluding(ex => ex.VacancyRefs)
                    .Excluding(ex => ex.UserRef)
                );
        }
    }
}
