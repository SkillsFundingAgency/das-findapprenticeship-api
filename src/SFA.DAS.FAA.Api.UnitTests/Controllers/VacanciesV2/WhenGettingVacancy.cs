using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.VacanciesV2;
public class WhenGettingVacancy
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Result_From_Mediator(
        GetApprenticeshipVacancyResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] VacanciesController controller)
    {
        var vacancyReference = new string("VAC1234");
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipVacancyQuery>(query =>
                    query.VacancyReference == vacancyReference), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var result = await controller.Get(vacancyReference) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        var apiModel = result.Value as GetApprenticeshipVacancyDetailResponse;
        apiModel.Should().NotBeNull();
        apiModel.Should().BeEquivalentTo(GetApprenticeshipVacancyDetailResponse.From(mediatorResult.ApprenticeshipVacancy));
    }
    
    [Test, MoqAutoData]
    public async Task And_Null_From_Mediator_Then_Returns_NotFound(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] VacanciesController controller)
    {
        var vacancyReference = new string("VAC12345");
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipVacancyQuery>(query =>
                    query.VacancyReference == vacancyReference), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApprenticeshipVacancyResult());

        var result = await controller.Get(vacancyReference) as NotFoundResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
    }
}
