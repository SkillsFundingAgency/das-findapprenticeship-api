using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Data.Repository;
using SFA.DAS.FAA.Data.UnitTests.Extensions;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Data.UnitTests.Repository
{
    public class WhenFindingApprenticeshipVacancies
    {
        private const string ExpectedEnvironmentName = "test";

        private Mock<IElasticLowLevelClient> _mockClient;
        private FindApprenticeshipsApiEnvironment _apiEnvironment;
        private ApprenticeshipVacancySearchRepository _repository;
        private Mock<IElasticSearchQueries> _mockElasticSearchQueries;

        [SetUp]
        public void Init()
        {
            _mockClient = new Mock<IElasticLowLevelClient>();
            _mockElasticSearchQueries = new Mock<IElasticSearchQueries>();
            _apiEnvironment = new FindApprenticeshipsApiEnvironment(ExpectedEnvironmentName);
            _repository = new ApprenticeshipVacancySearchRepository(_mockClient.Object, _apiEnvironment, _mockElasticSearchQueries.Object, Mock.Of<ILogger<ApprenticeshipVacancySearchRepository>>());

            var searchReponse =
                @"{""took"":33,""timed_out"":false,""_shards"":{""total"":1,""successful"":1,""skipped"":0,""failed"":0},
                    ""hits"":{""total"":{""value"":3,""relation"":""eq""},""max_score"":null,""hits"":[{""_index"":
                    ""test-faa-apprenticeships.2021-10-08-14-30"",""_type"":""_doc"",""_id"":
                    ""1000006648"",""_score"":1.0,""_source"":{          
                        ""id"" : 1000006648,
                        ""title"" : ""dbcMgHEgpl_14Jul2020_10014932357 apprenticeship"",
                        ""startDate"" : ""2020-10-18T00:00:00Z"",
                        ""closingDate"" : ""2020-09-17T00:00:00Z"",
                        ""postedDate"" : ""2020-07-14T10:06:25.8640000Z"",
                        ""employerName"" : ""ESFA LTD"",
                        ""providerName"" : ""BALTIC TRAINING SERVICES LIMITED"",
                        ""description"" : ""VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO"",
                        ""numberOfPositions"" : 2,
                        ""isPositiveAboutDisability"" : false,
                        ""isEmployerAnonymous"" : false,
                        ""vacancyLocationType"" : ""NonNational"",
                        ""location"" : {
                            ""lon"" : -3.169768,
                            ""lat"" : 55.970099
                        },
                        ""apprenticeshipLevel"" : ""Intermediate"",
                        ""vacancyReference"" : ""1000006648"",
                        ""category"" : ""Retail and Commercial Enterprise"",
                        ""categoryCode"" : ""SSAT1.HBY"",
                        ""subCategory"" : ""Butchery > Abattoir worker"",
                        ""subCategoryCode"" : ""STDSEC.5"",
                        ""workingWeek"" : ""hIxfvstIfxOZrDC"",
                        ""wageType"" : 3,
                        ""wageText"" : ""£8,049.60 to £14,976.00"",
                        ""wageUnit"" : 4,
                        ""hoursPerWeek"" : 40.0,
                        ""standardLarsCode"" : 274,
                        ""isDisabilityConfident"" : false,
                        ""ukprn"" : 10019026
                }}]}}";

            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        _repository.GetCurrentApprenticeshipVacanciesIndex(),
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(searchReponse));

            _mockClient.Setup(c =>
                    c.CountAsync<StringResponse>(
                        _repository.GetCurrentApprenticeshipVacanciesIndex(),
                        It.IsAny<PostData>(),
                        It.IsAny<CountRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(@"{""count"":10}"));

            _mockElasticSearchQueries.Setup(x => x.FindVacanciesQuery).Returns(string.Empty);
            _mockElasticSearchQueries.Setup(x => x.GetAllVacanciesQuery).Returns(string.Empty);
            _mockElasticSearchQueries.Setup(x => x.GetVacancyCountQuery).Returns(string.Empty);
        }

        [Test]
        public async Task Then_Will_Lookup_Total_ApprenticeshipVacancies()
        {
            //Arrange
            var expectedQuery = "test query";
            _mockElasticSearchQueries.Setup(x => x.GetVacancyCountQuery).Returns(expectedQuery);

            //Act
            await _repository.Find("10", 1, 1);

            //Assert
            _mockClient.Verify(c =>
                c.CountAsync<StringResponse>(
                    _repository.GetCurrentApprenticeshipVacanciesIndex(),
                    It.Is<PostData>(pd => 
                        pd.GetRequestString().Equals(expectedQuery)),
                    It.IsAny<CountRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Then_Will_Search_Latest_ApprenticeshipVacanciesIndex_Using_SearchTerm()
        {
            //Arrange
            var expectedSearchTerm = "test";
            ushort pageNumber = 1;
            ushort pageSize = 2;

            var searchQueryTemplate = "{searchTerm} - {startingDocumentIndex} - {pageSize}";
            var expectedQuery = $"{expectedSearchTerm} - 0 - {pageSize}";

            _mockElasticSearchQueries.Setup(x => x.FindVacanciesQuery).Returns(searchQueryTemplate);

            //Act
            await _repository.Find(expectedSearchTerm, pageNumber, pageSize);

            //Assert
            _mockClient.Verify(c =>
                c.SearchAsync<StringResponse>(
                    _repository.GetCurrentApprenticeshipVacanciesIndex(),
                    It.Is<PostData>(pd => 
                        pd.GetRequestString().Equals(expectedQuery)),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Then_Will_Search_Latest_ApprenticeshipVacanciesIndex_Without_A_SearchTerm()
        {
            //Arrange
            ushort pageNumber = 1;
            ushort pageSize = 2;

            var searchQueryTemplate = "{startingDocumentIndex} - {pageSize}";
            var expectedQuery = $"0 - {pageSize}";

            _mockElasticSearchQueries.Setup(x => x.GetAllVacanciesQuery).Returns(searchQueryTemplate);

            //Act
            await _repository.Find(string.Empty, pageNumber, pageSize);

            //Assert
            _mockClient.Verify(c =>
                c.SearchAsync<StringResponse>(
                    _repository.GetCurrentApprenticeshipVacanciesIndex(),
                    It.Is<PostData>(pd => 
                        pd.GetRequestString().Equals(expectedQuery)),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Then_Will_Not_Search_ApprenticeshipVacancies_If_LookUp_Is_Empty()
        {
            //Arrange
            var indexLookUpResponse =
                @"{""took"":0,""timed_out"":false,""_shards"":{""total"":0,""successful"":1,""skipped"":0,""failed"":0},
                ""hits"":{""total"":{""value"":0,""relation"":""eq""},""max_score"":null,""hits"":[]}}";

            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        _repository.GetCurrentApprenticeshipVacanciesIndex(),
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(indexLookUpResponse));

            //Act
            await _repository.Find("10", 1, 1);

            //Assert
            _mockClient.Verify(c =>
                c.SearchAsync<StringResponse>(
                    It.Is<string>(s => !s.Equals(_repository.GetCurrentApprenticeshipVacanciesIndex())),
                    It.IsAny<PostData>(),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Then_Will_Not_Search_ApprenticeshipVacancies_If_Latest_ApprenticeshipVacanciesIndex_Has_No_Name()
        {
            //Act
            await _repository.Find("10", 1, 1);

            //Assert
            _mockClient.Verify(c =>
                c.SearchAsync<StringResponse>(
                    It.Is<string>(s => !s.Equals(_repository.GetCurrentApprenticeshipVacanciesIndex())),
                    It.IsAny<PostData>(),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Then_Will_Return_ApprenticeshipVacancies_Found_With_Empty_Search()
        {
            //Act
            var results = await _repository.Find(string.Empty, 1, 1);

            //Assert
            results.TotalApprenticeshipVacancies.Should().Be(3);
            results.ApprenticeshipVacancies.Count().Should().Be(1);
            var vacancy = results.ApprenticeshipVacancies.First();
            vacancy.Id.Should().Be(1000006648);
            vacancy.Title.Should().Be("dbcMgHEgpl_14Jul2020_10014932357 apprenticeship");
            vacancy.StartDate.Date.Should().Be(DateTime.Parse("2020-10-18T00:00:00Z").Date);
            vacancy.ClosingDate.Date.Should().Be(DateTime.Parse("2020-09-17T00:00:00Z").Date);
            vacancy.PostedDate.Date.Should().Be(DateTime.Parse("2020-07-14T10:06:25.8640000Z").Date);
            vacancy.EmployerName.Should().Be("ESFA LTD");
            vacancy.ProviderName.Should().Be("BALTIC TRAINING SERVICES LIMITED");
            vacancy.Description.Should().Be("VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO");
            vacancy.NumberOfPositions.Should().Be(2);
            vacancy.IsPositiveAboutDisability.Should().BeFalse();
            vacancy.IsEmployerAnonymous.Should().BeFalse();
            vacancy.VacancyLocationType.Should().Be(VacancyLocationType.NonNational);
            vacancy.Location.lon.Should().Be(-3.169768);
            vacancy.Location.lat.Should().Be(55.970099);
            vacancy.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Intermediate);
            vacancy.VacancyReference.Should().Be("1000006648");
            vacancy.Category.Should().Be("Retail and Commercial Enterprise");
            vacancy.CategoryCode.Should().Be("SSAT1.HBY");
            vacancy.SubCategory.Should().Be("Butchery > Abattoir worker");
            vacancy.SubCategoryCode.Should().Be("STDSEC.5");
            vacancy.WorkingWeek.Should().Be("hIxfvstIfxOZrDC");
            vacancy.WageType.Should().Be(3);
            vacancy.WageText.Should().Be("£8,049.60 to £14,976.00");
            vacancy.WageUnit.Should().Be(4);
            vacancy.HoursPerWeek.Should().Be(40.0M);
            vacancy.StandardLarsCode.Should().Be(274);
            vacancy.IsDisabilityConfident.Should().BeFalse();
            vacancy.Ukprn.Should().Be(10019026);
        }

        [Test]
        public async Task Then_Will_Return_ApprenticeshipVacancies_Found_With_SearchTerm()
        {
            //Act
            var results = await _repository.Find("Test", 1, 1);

            //Assert
            Assert.AreEqual(3, results.TotalApprenticeshipVacancies);
            Assert.AreEqual(1, results.ApprenticeshipVacancies.Count());
            var vacancy = results.ApprenticeshipVacancies.First();
            vacancy.Id.Should().Be(1000006648);
            vacancy.Title.Should().Be("dbcMgHEgpl_14Jul2020_10014932357 apprenticeship");
            vacancy.StartDate.Date.Should().Be(DateTime.Parse("2020-10-18T00:00:00Z").Date);
            vacancy.ClosingDate.Date.Should().Be(DateTime.Parse("2020-09-17T00:00:00Z").Date);
            vacancy.PostedDate.Date.Should().Be(DateTime.Parse("2020-07-14T10:06:25.8640000Z").Date);
            vacancy.EmployerName.Should().Be("ESFA LTD");
            vacancy.ProviderName.Should().Be("BALTIC TRAINING SERVICES LIMITED");
            vacancy.Description.Should().Be("VyNpryuzIdktcJVjqJgxXpSFuwdrkqJRYCqEriOCbfZefEcOMO");
            vacancy.NumberOfPositions.Should().Be(2);
            vacancy.IsPositiveAboutDisability.Should().BeFalse();
            vacancy.IsEmployerAnonymous.Should().BeFalse();
            vacancy.VacancyLocationType.Should().Be(VacancyLocationType.NonNational);
            vacancy.Location.lon.Should().Be(-3.169768);
            vacancy.Location.lat.Should().Be(55.970099);
            vacancy.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Intermediate);
            vacancy.VacancyReference.Should().Be("1000006648");
            vacancy.Category.Should().Be("Retail and Commercial Enterprise");
            vacancy.CategoryCode.Should().Be("SSAT1.HBY");
            vacancy.SubCategory.Should().Be("Butchery > Abattoir worker");
            vacancy.SubCategoryCode.Should().Be("STDSEC.5");
            vacancy.WorkingWeek.Should().Be("hIxfvstIfxOZrDC");
            vacancy.WageType.Should().Be(3);
            vacancy.WageText.Should().Be("£8,049.60 to £14,976.00");
            vacancy.WageUnit.Should().Be(4);
            vacancy.HoursPerWeek.Should().Be(40.0M);
            vacancy.StandardLarsCode.Should().Be(274);
            vacancy.IsDisabilityConfident.Should().BeFalse();
            vacancy.Ukprn.Should().Be(10019026);
        }

        [Test]
        public async Task Then_Will_Return_Empty_Result_If_ApprenticeshipVacanciesIndex_Lookup_Return_Invalid_Response()
        {
            //Arrange
            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        _repository.GetCurrentApprenticeshipVacanciesIndex(),
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(""));

            //Act
            var result = await _repository.Find(string.Empty, 1, 10);

            //Assert
            Assert.IsNotNull(result?.ApprenticeshipVacancies);
            Assert.IsEmpty(result.ApprenticeshipVacancies);
            Assert.AreEqual(0, result.TotalApprenticeshipVacancies);
        }

        [Test]
        public async Task Then_Will_Return_Empty_Result_If_ApprenticeshipVacanciesIndex_Request_Returns_Invalid_Response()
        {
            //Arrange
            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        _repository.GetCurrentApprenticeshipVacanciesIndex(),
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(""));

            //Act
            var result = await _repository.Find(string.Empty, 1, 10);

            //Assert
            Assert.IsNotNull(result?.ApprenticeshipVacancies);
            Assert.IsEmpty(result.ApprenticeshipVacancies);
            Assert.AreEqual(0, result.TotalApprenticeshipVacancies);
        }

        [Test]
        public async Task Then_Will_Return_Empty_Result_If_ApprenticeshipVacanciesIndex_Lookup_Return_Failed_Response()
        {
            //Arrange
            var response =  @"{""took"":0,""timed_out"":false,""_shards"":{""total"":1,""successful"":0,""skipped"":0,""failed"":1},""hits"":{""total"":
            {""value"":0,""relation"":""eq""},""max_score"":null,""hits"":[]}}";

            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        _repository.GetCurrentApprenticeshipVacanciesIndex(),
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(response));

            //Act
            var result = await _repository.Find(string.Empty, 1, 10);

            //Assert
            Assert.IsNotNull(result?.ApprenticeshipVacancies);
            Assert.IsEmpty(result.ApprenticeshipVacancies);
            Assert.AreEqual(0, result.TotalApprenticeshipVacancies);

            _mockClient.Verify(c =>
                c.SearchAsync<StringResponse>(
                    It.Is<string>(s => !s.Equals(_repository.GetCurrentApprenticeshipVacanciesIndex())),
                    It.IsAny<PostData>(),
                    It.IsAny<SearchRequestParameters>(),
                    It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Then_Will_Return_Empty_Result_If_ApprenticeshipVacanciesIndex_Request_Returns_Failed_Response()
        {
            //Arrange
            var response =  @"{""took"":0,""timed_out"":false,""_shards"":{""total"":1,""successful"":0,""skipped"":0,""failed"":1},""hits"":{""total"":
            {""value"":0,""relation"":""eq""},""max_score"":null,""hits"":[]}}";

            _mockClient.Setup(c =>
                    c.SearchAsync<StringResponse>(
                        _repository.GetCurrentApprenticeshipVacanciesIndex(),
                        It.IsAny<PostData>(),
                        It.IsAny<SearchRequestParameters>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StringResponse(response));

            //Act
            var result = await _repository.Find(string.Empty, 1, 10);

            //Assert
            Assert.IsNotNull(result?.ApprenticeshipVacancies);
            Assert.IsEmpty(result.ApprenticeshipVacancies);
            Assert.AreEqual(0, result.TotalApprenticeshipVacancies);
        }
    }
}
