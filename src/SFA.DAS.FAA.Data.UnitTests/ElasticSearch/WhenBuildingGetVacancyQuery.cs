using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Data.ElasticSearch;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Data.UnitTests.ElasticSearch
{
    public class WhenBuildingGetVacancyQuery
    {
        [Test, MoqAutoData]
        public void Then_Returns_Query_From_Queries(
            string vacancyReference,
            [Frozen] Mock<IElasticSearchQueries> mockQueries,
            ElasticSearchQueryBuilder queryBuilder)
        {
            //act
            var query = queryBuilder.BuildGetVacancyQuery(vacancyReference);

            //ass
            query.Should().Be(mockQueries.Object.GetVacancyQuery);
        }
    }
}