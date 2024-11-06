using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch;

public class WhenUpsertingSavedSearch
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_SavedSearch_Is_Inserted_If_Not_Exists(
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        // arrange
        savedSearchEntity.Id = Guid.Empty;
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(new List<SavedSearchEntity>());

        // act
        var actual = await sut.Upsert(savedSearchEntity, default);

        // assert
        context.Verify(x => x.SavedSearchEntities.AddAsync(savedSearchEntity, CancellationToken.None), Times.Once);
        context.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        actual.Should().BeEquivalentTo(savedSearchEntity);
    }
    
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_SavedSearch_Is_Updated_If_Exists(
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        // arrange
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(new List<SavedSearchEntity>{ savedSearchEntity });
        var updatedSearchEntity = new SavedSearchEntity
        {
            Id = savedSearchEntity.Id,
            DateCreated = savedSearchEntity.DateCreated,
            LastRunDate = DateTime.UtcNow,
            EmailLastSendDate = DateTime.UtcNow,
            SearchParameters = "new search parameters",
            UserRef = savedSearchEntity.UserRef,
            UnSubscribeToken = savedSearchEntity.UnSubscribeToken
        };
        
        // act
        var actual = await sut.Upsert(updatedSearchEntity, default);

        // assert
        context.Verify(x => x.SavedSearchEntities.AddAsync(It.IsAny<SavedSearchEntity>(), CancellationToken.None), Times.Never);
        context.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        actual.Should().BeEquivalentTo(updatedSearchEntity);
    }
}