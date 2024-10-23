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
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearch
{
    [TestFixture]
    public class WhenHandlingGetSavedSearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_SavedSearches_From_Repository(
            List<string> vacancyReferences,
            GetSavedSearchesQueryResult.SearchParameters searchParameters,
            GetSavedSearchesQuery query,
            PaginatedList<SavedSearchEntity> savedSearchEntities,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
            GetSavedSearchesQueryHandler handler)
        {
            //arrange
            foreach (var savedSearchEntity in savedSearchEntities.Items)
            {
                savedSearchEntity.SearchParameters = JsonConvert.SerializeObject(searchParameters);
                savedSearchEntity.VacancyRefs = JsonConvert.SerializeObject(vacancyReferences);
            }

            savedSearchRepository
                .Setup(repository => repository.GetAll(query.LastRunDateFilter, query.PageNumber, query.PageSize, CancellationToken.None))
                .ReturnsAsync(savedSearchEntities);

            var result = await handler.Handle(query, CancellationToken.None);

            result.SavedSearches
                .Should().BeEquivalentTo(savedSearchEntities.Items, options => options
                    .Excluding(ex => ex.SearchParameters)
                    .Excluding(ex => ex.VacancyRefs)
                    .Excluding(ex => ex.UserRef)
                );
        }
    }
}
