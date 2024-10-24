using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.SavedSearches.Commands.PostUpdateSavedSearches;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.SavedSearch;

[TestFixture]
public class WhenHandingPostUpdateSavedSearchesCommand
{
    [TestFixture]
    public class WhenHandlingGetSavedSearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_SavedSearches_Updates_In_Repository(
            PostUpdateSavedSearchesCommand command,
            [Frozen] Mock<ISavedSearchRepository> savedSearchRepository,
            PostUpdateSavedSearchesCommandHandler handler)
        {
            //arrange
            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            savedSearchRepository.Verify(x => x.Save(It.IsAny<SavedSearchEntity>(), CancellationToken.None), Times.Exactly(command.SavedSearchGuids.Count));
        }
    }
}