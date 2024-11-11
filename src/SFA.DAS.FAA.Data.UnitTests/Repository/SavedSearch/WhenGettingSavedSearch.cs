using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch;

public class WhenGettingSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_SavedSearch_Is_Returned(
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        // arrange
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(new List<SavedSearchEntity>{savedSearchEntity});

        // act
        var result = await sut.Get(savedSearchEntity.UserRef, savedSearchEntity.Id, CancellationToken.None);
        
        // assert
        result.Should().BeEquivalentTo(savedSearchEntity);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_No_Entity_Is_Returned(
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        // arrange
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(new List<SavedSearchEntity>{savedSearchEntity});

        // act
        var result = await sut.Get(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);
        
        // assert
        result.Should().BeNull();
    }
}