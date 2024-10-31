using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using SFA.DAS.FAA.Application.SavedSearches.Commands.PatchSavedSearch;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Commands;

[TestFixture]
public class WhenHandingPostUpdateSavedSearchesCommand
{
    [TestFixture]
    public class WhenHandlingGetSavedSearchQuery
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_SavedSearches_Updates_In_Repository(
            SearchParameters searchParameters,
            SavedSearchEntity savedSearchEntity,
            PatchSavedSearch patch,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
            PatchSavedSearchCommandHandler handler)
        {
            // arrange
            savedSearchEntity.SearchParameters = searchParameters.ToJson();
            
            var patchCommand = new JsonPatchDocument<PatchSavedSearch>();
            patchCommand.Replace(path => path.LastRunDate, patch.LastRunDate);
            patchCommand.Replace(path => path.EmailLastSendDate, patch.EmailLastSendDate);

            var command = new PatchSavedSearchCommand(savedSearchEntity.Id, patchCommand);

            savedSearchRepository
                .Setup(x => x.GetById(savedSearchEntity.Id, CancellationToken.None))
                .ReturnsAsync(savedSearchEntity);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.Should().NotBeNull();
            result.SavedSearch.Should().BeEquivalentTo(savedSearchEntity, options=>options.Excluding(c=>c.UserRef).Excluding(x => x.SearchParameters));
            result.SavedSearch.UserReference.Should().Be(savedSearchEntity.UserRef);
            result.SavedSearch.SearchParameters.Should().BeEquivalentTo(searchParameters);
            savedSearchRepository.Verify(x => x.Update(It.IsAny<SavedSearchEntity>(), CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_SavedSearches_NotFound_Repository_Not_Called(
            PatchSavedSearchCommand command,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
            PatchSavedSearchCommandHandler handler)
        {
            //arrange
            savedSearchRepository
                .Setup(x => x.GetById(command.Id, CancellationToken.None))
                .ReturnsAsync((SavedSearchEntity)null!);

            var result = await handler.Handle(command, CancellationToken.None);

            result.SavedSearch.Should().BeNull();

            savedSearchRepository.Verify(x => x.Update(It.IsAny<SavedSearchEntity>(), CancellationToken.None), Times.Never);
        }
    }
}