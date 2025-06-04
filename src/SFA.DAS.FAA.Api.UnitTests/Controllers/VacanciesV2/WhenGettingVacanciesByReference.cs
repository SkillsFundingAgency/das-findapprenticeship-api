using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Api.UnitTests.Extensions;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipsVacanciesByIdList;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.VacanciesV2
{
    [TestFixture]
    public class WhenGettingVacanciesByReference
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Result_From_Mediator(
            GetVacanciesByReferenceRequest request,
            GetApprenticeshipVacanciesByReferenceQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacanciesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeshipVacanciesByReferenceQuery>(query =>
                        query.VacancyReferences == request.VacancyReferences),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await controller.GetByVacancyReferences(request) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var apiModel = result.Value as GetApprenticeshipVacanciesByReferenceApiResponse;
            apiModel.Should().NotBeNull();
            apiModel.Should().BeEquivalentTo((GetApprenticeshipVacanciesByReferenceApiResponse)mediatorResult);
        }
    }
}
