using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch;

[TestFixture]
public class WhenDeletingAllSavedSearches
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_SavedSearches_Is_Deleted_If_Exists(
        Guid userReference,
        List<SavedSearchEntity> savedSearchEntities,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        //arrange
        foreach (var savedSearchEntity in savedSearchEntities)
        {
            savedSearchEntity.UserRef = userReference;
        }
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(savedSearchEntities);

        await sut.DeleteAll(userReference, CancellationToken.None);
        
        context.Verify(x => x.SavedSearchEntities.RemoveRange(savedSearchEntities), Times.Once);
        context.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_SavedSearches_Does_Not_Exist_Delete_Does_Not_Occur(
        List<SavedSearchEntity> savedSearchEntities,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(savedSearchEntities);

        await sut.DeleteAll(It.IsAny<Guid>(), CancellationToken.None);

        context.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}