using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteUserSavedSearch;
using SFA.DAS.FAA.Data.SavedSearch;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Commands;

public class WhenHandlingDeleteUserSavedSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Repository_Called(
        DeleteUserSavedSearchCommand command,
        [Frozen] Mock<ISavedSearchRepository> repository,
        DeleteUserSavedSearchCommandHandler handler)
    {
        await handler.Handle(command, CancellationToken.None);
        
        repository.Verify(x => x.Delete(command.UserReference, command.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}