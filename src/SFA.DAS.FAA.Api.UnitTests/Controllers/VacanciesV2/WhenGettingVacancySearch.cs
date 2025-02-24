using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using SFA.DAS.FAA.Domain.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.VacanciesV2;
public class WhenGettingVacancySearch
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Search_Result_From_Mediator(
    SearchVacancyRequest request,
    SearchApprenticeshipVacanciesResult mediatorResult,
    [Frozen] Mock<IMediator> mockMediator,
    [Greedy] VacanciesController controller)
    {
        request.Sort = VacancySort.DistanceDesc;
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<SearchApprenticeshipVacanciesQuery>(query =>
                    query.PageNumber == request.PageNumber &&
                    query.PageSize == request.PageSize &&
                    query.Ukprn == request.Ukprn &&
                    query.AccountPublicHashedId == request.AccountPublicHashedId &&
                    query.AccountLegalEntityPublicHashedId == request.AccountLegalEntityPublicHashedId &&
                    query.StandardLarsCode == request.StandardLarsCode &&
                    query.ExcludeNational == request.ExcludeNational &&
                    query.Lat.Equals(request.Lat) &&
                    query.Lon.Equals(request.Lon) &&
                    query.DistanceInMiles == request.DistanceInMiles &&
                    query.Categories == request.Categories &&
                    query.RouteIds == request.RouteIds &&
                    query.Levels == request.Levels &&
                    query.PostedInLastNumberOfDays == request.PostedInLastNumberOfDays &&
                    query.VacancySort.Equals(request.Sort) &&
					query.SearchTerm == request.SearchTerm &&
                    query.AdditionalDataSources == request.AdditionalDataSources
                ),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var result = await controller.Search(request) as OkObjectResult;

        result.Should().NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var apiModel = result?.Value as GetSearchApprenticeshipVacanciesResponse;
        apiModel.Should().NotBeNull();
        apiModel.Should().BeEquivalentTo((GetSearchApprenticeshipVacanciesResponse)mediatorResult);
    }
}