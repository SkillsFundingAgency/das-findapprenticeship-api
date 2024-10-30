using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Commands;

public class WhenHandlingSaveSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Search_Is_Saved(
        SaveSearchCommand request,
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<ISavedSearchRepository> savedSearchesRepository,
        SaveSearchCommandHandler sut)
    {
        // arrange
        savedSearchesRepository.Setup(repo => repo.Upsert(It.Is<SavedSearchEntity>(entity => 
            entity.UserRef == request.UserReference
            && entity.SearchParameters == request.SearchParameters
        )))
        .ReturnsAsync(savedSearchEntity);
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
        
        // assert
        result.Id.Should().Be(savedSearchEntity.Id);
    }
}