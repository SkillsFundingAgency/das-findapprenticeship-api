using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.FAA.Application.SavedSearches.Commands.UpsertSaveSearch;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Constants;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Commands;

public class WhenHandlingSaveSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Search_Is_Saved(
        UpsertSaveSearchCommand request,
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<ISavedSearchRepository> savedSearchesRepository,
        UpsertSaveSearchCommandHandler sut)
    {
        // arrange
        savedSearchesRepository
            .Setup(x => x.Count(It.IsAny<Guid>()))
            .ReturnsAsync(Constants.SavedSearchLimit - 1);
        
        SavedSearchEntity passedEntity = null;
        savedSearchesRepository
            .Setup(repo => repo.Upsert(It.IsAny<SavedSearchEntity>(), default))
            .Callback<SavedSearchEntity, CancellationToken>((cb, _) => passedEntity = cb)
            .ReturnsAsync(savedSearchEntity);
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
        
        // assert
        result.Id.Should().Be(savedSearchEntity.Id);
        passedEntity.UserRef.Should().Be(request.UserReference);
        passedEntity.SearchParameters.Should().BeEquivalentTo(JsonConvert.SerializeObject(request.SearchParameters));
    }
    
    [Test, MoqAutoData]
    public async Task Then_When_The_SavedSearch_Limit_Is_Reached_Then_The_Search_Is_Not_Saved(
        UpsertSaveSearchCommand request,
        SavedSearchEntity savedSearchEntity,
        [Frozen] Mock<ISavedSearchRepository> savedSearchesRepository,
        UpsertSaveSearchCommandHandler sut)
    {
        // arrange
        savedSearchesRepository
            .Setup(x => x.Count(It.IsAny<Guid>()))
            .ReturnsAsync(Constants.SavedSearchLimit);
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);
        
        // assert
        result.Should().Be(UpsertSaveSearchCommandResult.None);
        savedSearchesRepository.Verify(x => x.Upsert(It.IsAny<SavedSearchEntity>(), default), Times.Never());
    }
}