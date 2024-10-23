using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Data.UnitTests.DatabaseMock;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Data.UnitTests.Repository.SavedSearch
{
    [TestFixture]
    public class WhenGettingSavedSearch
    {
        [Test]
        [RecursiveMoqAutoData]
        public async Task And_Then_SavedSearch_Result_Is_Returned(
            DateTime lastRunDateTime,
            List<SavedSearchEntity> savedSearchEntities,
            [Frozen] Mock<IFindApprenticeshipsDataContext> context,
            SavedSearchRepository repository)
        {
            //Arrange
            foreach (var savedSearchEntity in savedSearchEntities)
            {
                savedSearchEntity.LastRunDate = lastRunDateTime.AddDays(1);
            }
            
            context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(savedSearchEntities);

            //Act
            var result = await repository.GetAll(lastRunDateTime);

            //Assert
            result.Should().BeEquivalentTo(savedSearchEntities);
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task And_Then_LastRun_Null_SavedSearch_Result_Is_Returned(
            DateTime lastRunDateTime,
            List<SavedSearchEntity> savedSearchEntities,
            [Frozen] Mock<IFindApprenticeshipsDataContext> context,
            SavedSearchRepository repository)
        {
            //Arrange
            foreach (var savedSearchEntity in savedSearchEntities)
            {
                savedSearchEntity.LastRunDate = null;
            }

            context.Setup(x => x.SavedSearchEntities).ReturnsDbSet(savedSearchEntities);

            //Act
            var result = await repository.GetAll(lastRunDateTime);

            //Assert
            result.Should().BeEquivalentTo(savedSearchEntities);
        }
    }
}
