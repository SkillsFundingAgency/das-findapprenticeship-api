using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using SFA.DAS.FAA.Data.AzureSearch;

namespace SFA.DAS.FAA.Data.UnitTests.AzureCognitiveSearchRepository;
public class WhenFindingApprenticeshipVacancies
{
    //TODO: mock responses to be filled in once we can call the search without a distance sort (after 1027 is done).

    //[Test, MoqAutoData]
    //public async Task Then_Will_Return_ApprenticeshipVacancies_Found_And_Distance_Is_Null(
    //    FindVacanciesModel model,
    //    [Frozen] Mock<ILogger<AcsVacancySearchRepository>> logger,
    //    [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
    //    AcsVacancySearchRepository sut)
    //{
    //    azureSearchHelper.Setup(x => x.Find(model)).ReturnsAsync(mockResponse);
    //    // var expectedVacancy = mockResponse first vacancy

    //    var actual = await sut.Find(model);

    //    using (new AssertionScope())
    //    {
    //        actual.Total.Should().Be(10);
    //        actual.TotalFound.Should().Be(2);
    //        actual.ApprenticeshipVacancies.Count().Should().Be(2);
    //        var vacancy = actual.ApprenticeshipVacancies.First();
    //        vacancy.Should().BeEquivalentTo(expectedVacancy);
    //        vacancy.Distance.Should().BeNull();
    //    }
    //}

    [Test, MoqAutoData]
    public async Task Then_Will_Return_Empty_Result_If_ApprenticeshipVacanciesIndex_Request_Returns_No_results(
        FindVacanciesModel model,
        [Frozen] Mock<ILogger<AcsVacancySearchRepository>> logger,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AcsVacancySearchRepository sut)
    {
        var mockResponse = new Domain.Entities.ApprenticeshipSearchResponse();
        azureSearchHelper.Setup(x => x.Find(model)).Returns(Task.FromResult(mockResponse));

        var actual = await sut.Find(model);

        using (new AssertionScope())
        {
            actual.ApprenticeshipVacancies.Should().NotBeNull();
            actual.ApprenticeshipVacancies.Should().BeEmpty();
            actual.TotalFound.Should().Be(0);
        }
    }
}
