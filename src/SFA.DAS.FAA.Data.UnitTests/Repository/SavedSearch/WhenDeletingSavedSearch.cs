using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch;

public class WhenDeletingSavedSearch
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_SavedSearch_Is_Deleted_If_Exists(
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(new List<SavedSearchEntity>{savedSearchEntity});

        await sut.Delete(savedSearchEntity.Id, CancellationToken.None);
        
        context.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_SavedSearch_Does_Not_Exist_Delete_Does_Not_Error(
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(new List<SavedSearchEntity>{savedSearchEntity});
        
        await sut.Delete(Guid.NewGuid(), CancellationToken.None);
        
        context.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}