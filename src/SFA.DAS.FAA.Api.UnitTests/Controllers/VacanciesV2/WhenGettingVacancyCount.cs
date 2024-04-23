using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.ApiRequests;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.VacanciesV2
{
    public class WhenGettingVacancyCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Count_Result_From_Mediator(
            SearchVacancyTotalRequest request,
            int mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacanciesV2Controller controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeshipVacancyCountQuery>(query =>
                        query.Source == SearchSource.AzureSearch && query.AdditionalDataSources == request.AdditionalDataSources),
                    It.IsAny<CancellationToken>()))

                .ReturnsAsync(mediatorResult);

            var result = await controller.GetVacancyCount(request) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var apiModel = result.Value as GetCountApprenticeshipVacanciesResponse;
            apiModel.Should().NotBeNull();
            apiModel.TotalVacancies.Should().Be(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Count_And_If_Error_Returns_Status_Code_Result_InternalServerError(
            SearchVacancyTotalRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacanciesV2Controller controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetApprenticeshipVacancyCountQuery>(), 
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var result = await controller.GetVacancyCount(request) as StatusCodeResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}