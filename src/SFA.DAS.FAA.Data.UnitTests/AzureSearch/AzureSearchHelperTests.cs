using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Moq;
using SFA.DAS.FAA.Data.AzureSearch;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Data.UnitTests.AzureSearch;

public class AzureSearchHelperTests
{
    private Mock<SearchClient> _searchClient;
    private Mock<SearchIndexClient> _searchIndexClient;
    private AzureSearchHelper _helper;

    [SetUp]
    public void Setup()
    {
        _searchClient = new Mock<SearchClient>();
        _searchIndexClient = new Mock<SearchIndexClient>();
        _helper = new AzureSearchHelper(_searchClient.Object, _searchIndexClient.Object);
    }

    [Test, AutoData]
    public async Task Find_When_IncludeDetails_Is_False_Returns_ApprenticeshipVacancies(FindVacanciesModel model)
    {
        model = model with { IncludeDetails = false };
        var searchDocument = new SearchDocument(new Dictionary<string, object> { { "Id", "1" }, { "VacancyReference", "123" } });
        
        var mockSearchResults = SearchModelFactory.SearchResults(
            [SearchModelFactory.SearchResult(searchDocument, null, null)],
            1, null, null, new Mock<Response>().Object);

        _searchClient.Setup(x => x.SearchAsync<SearchDocument>(It.IsAny<string>(), It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(mockSearchResults, new Mock<Response>().Object));

        var result = await _helper.Find(model);

        result.ApprenticeshipVacancies.Should().NotBeEmpty();
        result.ApprenticeshipVacanciesWithDetails.Should().BeEmpty();
        result.TotalFound.Should().Be(1);
    }

    [Test, AutoData]
    public async Task Find_When_IncludeDetails_Is_True_And_PageSize_Less_Than_Or_Equal_To_One_Hundred_Returns_ApprenticeshipVacanciesWithDetails(FindVacanciesModel model)
    {
        model = model with { IncludeDetails = true, PageSize = 10 };
        var searchDocument = new SearchDocument(new Dictionary<string, object> { { "Id", "1" }, { "VacancyReference", "123" } });
        
        var mockSearchResults = SearchModelFactory.SearchResults(
            [SearchModelFactory.SearchResult(searchDocument, null, null)],
            1, null, null, new Mock<Response>().Object);

        _searchClient.Setup(x => x.SearchAsync<SearchDocument>(It.IsAny<string>(), It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(mockSearchResults, new Mock<Response>().Object));

        var result = await _helper.Find(model);

        result.ApprenticeshipVacancies.Should().BeEmpty();
        result.ApprenticeshipVacanciesWithDetails.Should().NotBeEmpty();
        result.TotalFound.Should().Be(1);
    }
}
