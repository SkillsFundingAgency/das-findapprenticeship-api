using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch;

public class WhenGettingSavedSearchesByUserReference
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_SavedSearch_Is_Deleted_If_Exists(
        List<SavedSearchEntity> entities,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        // arrange
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(entities);

        // act
        var result = await sut.GetByUserReference(entities.First().UserRef, default);
        
        // assert
        entities.Count.Should().Be(3); // Just verify automoq is generating more than 1 item
        result.Count.Should().Be(1);
        result.Should().BeEquivalentTo(new List<SavedSearchEntity> { entities.First() });
    }
}