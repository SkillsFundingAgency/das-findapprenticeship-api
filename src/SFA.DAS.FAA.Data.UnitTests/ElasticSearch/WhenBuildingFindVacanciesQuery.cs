using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Data.ElasticSearch;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Data.UnitTests.ElasticSearch
{
    public class WhenBuildingFindVacanciesQuery
    {
        [Test, MoqAutoData]
        public void Then_Calculates_StartDocumentIndex_And_Adds_To_Query(
            int pageNumber, 
            int pageSize, 
            [Frozen] Mock<IElasticSearchQueries> mockQueries,
            ElasticSearchQueryBuilder queryBuilder)
        {
            //arr
            mockQueries
                .Setup(queries => queries.FindVacanciesQuery)
                .Returns(@"{""from"": ""{startingDocumentIndex}""}");
            var expectedStartingDocumentIndex = pageNumber < 2 ? 0 : (pageNumber - 1) * pageSize;
            
            //act
            var query = queryBuilder.BuildFindVacanciesQuery(pageNumber, pageSize, null);

            //ass
            query.Should().Contain(@$"""from"": ""{expectedStartingDocumentIndex}""");
        }

        [Test, MoqAutoData]
        public void Then_Adds_PageSize_To_Query(
            int pageNumber, 
            int pageSize, 
            [Frozen] Mock<IElasticSearchQueries> mockQueries,
            ElasticSearchQueryBuilder queryBuilder)
        {
            //arr
            mockQueries
                .Setup(queries => queries.FindVacanciesQuery)
                .Returns(@"{""size"": ""{pageSize}""}");
            
            //act
            var query = queryBuilder.BuildFindVacanciesQuery(pageNumber, pageSize, null);

            //ass
            query.Should().Contain(@$"""size"": ""{pageSize}""");
        }
        
        [Test, MoqAutoData]
        public void And_Ukprn_HasValue_Then_Adds_Must_Condition(
            int pageNumber, 
            int pageSize, 
            int ukprn,
            [Frozen] Mock<IElasticSearchQueries> mockQueries,
            ElasticSearchQueryBuilder queryBuilder)
        {
            //arr
            mockQueries
                .Setup(queries => queries.FindVacanciesQuery)
                .Returns(@"{""must"": [ {mustConditions} ] }");
            
            //act
            var query = queryBuilder.BuildFindVacanciesQuery(pageNumber, pageSize, ukprn);

            //ass
            query.Should().Contain(@$"""must"": [ {{ ""term"": {{ ""{nameof(ukprn)}"": ""{ukprn}"" }}}} ]");
        }
        
        
        [Test, MoqAutoData]
        public void And_AccountId_HasValue_Then_Adds_Must_Condition(
            int pageNumber, 
            int pageSize, 
            int ukprn,
            string accountPublicHashedId,
            [Frozen] Mock<IElasticSearchQueries> mockQueries,
            ElasticSearchQueryBuilder queryBuilder)
        {
            //arr
            mockQueries
                .Setup(queries => queries.FindVacanciesQuery)
                .Returns(@"{""must"": [ {mustConditions} ] }");
            
            //act
            var query = queryBuilder.BuildFindVacanciesQuery(pageNumber, pageSize, null, accountPublicHashedId);

            //ass
            query.Should().Contain(@$"""must"": [ {{ ""term"": {{ ""{nameof(accountPublicHashedId)}"": ""{accountPublicHashedId}"" }}}} ]");
        }
        
        [Test, MoqAutoData]
        public void And_AccountLegalEntityId_HasValue_Then_Adds_Must_Condition(
            int pageNumber, 
            int pageSize, 
            int ukprn,
            string accountLegalEntityPublicHashedId,
            [Frozen] Mock<IElasticSearchQueries> mockQueries,
            ElasticSearchQueryBuilder queryBuilder)
        {
            //arr
            mockQueries
                .Setup(queries => queries.FindVacanciesQuery)
                .Returns(@"{""must"": [ {mustConditions} ] }");
            
            //act
            var query = queryBuilder.BuildFindVacanciesQuery(pageNumber, pageSize, null, null, accountLegalEntityPublicHashedId);

            //ass
            query.Should().Contain(@$"""must"": [ {{ ""term"": {{ ""{nameof(accountLegalEntityPublicHashedId)}"": ""{accountLegalEntityPublicHashedId}"" }}}} ]");
        }
        
        
        
        [Test, MoqAutoData]
        public void And_Ukprn_And_AccountId_And_AccountLegalEntity_HasValue_Then_Adds_Must_Condition(
            int pageNumber, 
            int pageSize, 
            int ukprn,
            string accountPublicHashedId,
            string accountLegalEntityPublicHashedId,
            [Frozen] Mock<IElasticSearchQueries> mockQueries,
            ElasticSearchQueryBuilder queryBuilder)
        {
            //arr
            mockQueries
                .Setup(queries => queries.FindVacanciesQuery)
                .Returns(@"{""must"": [ {mustConditions} ] }");
            
            //act
            var query = queryBuilder.BuildFindVacanciesQuery(pageNumber, pageSize, ukprn, accountPublicHashedId, accountLegalEntityPublicHashedId);

            //ass
            query.Should().Contain(@$"""must"": [ {{ ""term"": {{ ""{nameof(ukprn)}"": ""{ukprn}"" }}}}, {{ ""term"": {{ ""{nameof(accountPublicHashedId)}"": ""{accountPublicHashedId}"" }}}}, {{ ""term"": {{ ""{nameof(accountLegalEntityPublicHashedId)}"": ""{accountLegalEntityPublicHashedId}"" }}}} ]");
        }
    }
}