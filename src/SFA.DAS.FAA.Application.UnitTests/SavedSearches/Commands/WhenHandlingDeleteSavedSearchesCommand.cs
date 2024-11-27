using SFA.DAS.FAA.Application.SavedSearches.Commands.DeleteSavedSearches;
using SFA.DAS.FAA.Data.SavedSearch;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearches.Commands;

[TestFixture]
public class WhenHandlingDeleteSavedSearchesCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Repository_Called(
        DeleteSavedSearchesCommand command,
        [Frozen] Mock<ISavedSearchRepository> repository,
        DeleteSavedSearchesCommandHandler handler)
    {
        await handler.Handle(command, CancellationToken.None);

        repository.Verify(x => x.DeleteAll(command.UserReference, It.IsAny<CancellationToken>()), Times.Once);
    }
}