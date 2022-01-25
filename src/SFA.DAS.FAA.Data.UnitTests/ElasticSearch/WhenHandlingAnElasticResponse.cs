using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Data.ElasticSearch;

namespace SFA.DAS.FAA.Data.UnitTests.ElasticSearch
{
    public class WhenHandlingAnElasticResponse
    {
        [Test]
        public void ThenWillShowSearchHitsAsItems()
        {
            //Arrange
            var expectedResponseHits = new List<Hit<string>> {
                new Hit<string>{_source = "1"},
                new Hit<string>{_source = "2"},
                new Hit<string>{_source = "3"}};
            var response = new ElasticResponse<string>
            {
                hits = new Hits<string>()
                {
                    hits = expectedResponseHits
                        .Select(h => h)
                        .ToList()
                }
            };

            //Act
            var responseItems = response.Items;

            //Assert
            responseItems.Should().BeEquivalentTo(expectedResponseHits);
        }

        [Test]
        public void ThenWillReturnEmptyListIfNoHitsExist()
        {
            //Arrange
            var response = new ElasticResponse<string> {hits = new Hits<string>()};

            //Act
            var responseItems = response.Items;

            //Assert
            responseItems.Should().NotBeNull();
            responseItems.Should().BeEmpty();
        }

        [Test]
        public void ThenWillReturnEmptyListIfHitsIsNull()
        {
            //Arrange
            var response = new ElasticResponse<string>();

            //Act
            var responseItems = response.Items;

            //Assert
            responseItems.Should().NotBeNull();
            responseItems.Should().BeEmpty();
        }
    }
}
