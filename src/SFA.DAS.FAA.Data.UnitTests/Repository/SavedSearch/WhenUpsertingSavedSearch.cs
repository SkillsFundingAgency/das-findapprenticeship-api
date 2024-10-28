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
    public async Task Then_The_Application_Is_Inserted_If_Not_Exists(
        SavedSearchEntity applicationEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchesRepository sut)
    {
        // arrange
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(new List<SavedSearchEntity>());

        // act
        var actual = await sut.Upsert(applicationEntity);

        // assert
        context.Verify(x => x.SavedSearchEntities.AddAsync(applicationEntity, CancellationToken.None), Times.Once);
        context.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        actual.Should().BeEquivalentTo(applicationEntity);
    }
}