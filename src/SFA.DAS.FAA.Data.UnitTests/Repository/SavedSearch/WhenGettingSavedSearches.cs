using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;
using System;
using System.Threading.Tasks;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch
{
    [TestFixture]
    public class WhenGettingSavedSearches
    {
        [Test]
        [RecursiveMoqAutoData]
        public async Task And_Then_SavedSearch_Result_Is_Returned(
            int pageNumber,
            int pageSize,
            DateTime lastRunDateTime,
            PaginatedList<SavedSearchEntity> savedSearchEntities,
            [Frozen] Mock<IFindApprenticeshipsDataContext> context,
            SavedSearchRepository repository)
        {
            //Arrange
            foreach (var savedSearchEntity in savedSearchEntities.Items)
            {
                savedSearchEntity.LastRunDate = lastRunDateTime.AddDays(1);
            }
            
            context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(savedSearchEntities.Items);

            //Act
            var result = await repository.GetAll(lastRunDateTime, 1, 1000, default);

            //Assert
            result.Items.Should().BeEquivalentTo(savedSearchEntities.Items);
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task And_Then_LastRun_Null_SavedSearch_Result_Is_Returned(
            DateTime lastRunDateTime,
            PaginatedList<SavedSearchEntity> savedSearchEntities,
            [Frozen] Mock<IFindApprenticeshipsDataContext> context,
            SavedSearchRepository repository)
        {
            //Arrange
            foreach (var savedSearchEntity in savedSearchEntities.Items)
            {
                savedSearchEntity.LastRunDate = null;
            }

            context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(savedSearchEntities.Items);

            //Act
            var result = await repository.GetAll(lastRunDateTime, 1, 1000, default);

            //Assert
            result.Items.Should().BeEquivalentTo(savedSearchEntities.Items);
        }
    }
}
