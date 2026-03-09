using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch;

[TestFixture]
public class WhenUpdatingSavedSearch
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task And_Then_SavedSearch_Updates_Result_Is_Returned(
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository repository)
    {
        //Arrange
        context.Setup(x => x.SavedSearchEntities).ReturnsDbSet([savedSearchEntity]);

        //Act
        await repository.Update(savedSearchEntity, CancellationToken.None);

        //Assert
        context.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}