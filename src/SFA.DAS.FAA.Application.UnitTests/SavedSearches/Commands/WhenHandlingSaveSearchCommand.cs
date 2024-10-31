using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.FAA.Application.SavedSearches.Commands.SaveSearch;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

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
        SavedSearchEntity passedEntity = null;
        
        savedSearchesRepository.Setup(repo => repo.Upsert(It.IsAny<SavedSearchEntity>(), default))
            .Callback<SavedSearchEntity, CancellationToken>((cb, _) => passedEntity = cb)
            .ReturnsAsync(savedSearchEntity);
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
        
        // assert
        result.Id.Should().Be(savedSearchEntity.Id);
        passedEntity.UserRef.Should().Be(request.UserReference);
        passedEntity.SearchParameters.Should().BeEquivalentTo(JsonConvert.SerializeObject(request.SearchParameters));
    }
}