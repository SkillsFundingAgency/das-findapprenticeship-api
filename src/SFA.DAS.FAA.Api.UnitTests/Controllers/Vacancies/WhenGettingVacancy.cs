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
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancy;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Vacancies
{
    public class WhenGettingVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Result_From_Mediator(
            string vacancyReference,
            GetApprenticeshipVacancyResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacanciesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeshipVacancyQuery>(query =>
                        query.VacancyReference == vacancyReference), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await controller.Get(vacancyReference) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var apiModel = result.Value as GetApprenticeshipVacancyResponse;
            apiModel.Should().NotBeNull();
            apiModel.Should().BeEquivalentTo((GetApprenticeshipVacancyResponse)mediatorResult.ApprenticeshipVacancy);
        }
    }
}