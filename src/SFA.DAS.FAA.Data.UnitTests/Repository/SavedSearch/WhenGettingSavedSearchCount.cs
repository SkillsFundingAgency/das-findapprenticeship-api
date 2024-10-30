using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch;

public class WhenGettingSavedSearchCount
{
    [Test, MoqAutoData]
    public async Task Then_If_There_Are_No_Records_For_A_Candidate_Then_Zero_Is_Returned(
        Guid candidateId,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        // arrange
        context
            .Setup(x => x.SavedSearchEntities)
            .ReturnsDbSet(new List<SavedSearchEntity>());
        
        // act
        var actual = await sut.Count(candidateId);
        
        // assert
        actual.Should().Be(0);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_There_Are_Records_For_A_Candidate_Then_The_Count_Is_Returned(
        [Frozen] Guid candidateId,
        List<SavedSearchEntity> entities,
        [Frozen] Mock<IFindApprenticeshipsDataContext> context,
        SavedSearchRepository sut)
    {
        // arrange
        var savedSearchEntity = entities.First();
        context
            .Setup(x => x.SavedSearchEntities)
            .ReturnsDbSet(entities);
        
        // act
        var actual = await sut.Count(savedSearchEntity.UserRef);
        
        // assert
        actual.Should().BeGreaterThan(0);
        actual.Should().Be(entities.Count);
    }
}