using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetSavedSearches;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.SavedSearch
{
    [TestFixture]
    public class WhenGettingSavedSearch
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Result_From_Mediator(
            DateTime lastRunDateFilter,
            GetSavedSearchesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetSavedSearchesQuery>(query =>
                        query.LastRunDateFilter == lastRunDateFilter),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await controller.Get(lastRunDateFilter) as OkObjectResult;

            result.Should().NotBeNull();
            result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var apiModel = result.Value as GetSavedSearchesQueryResult;
            apiModel.Should().NotBeNull();
            apiModel.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Error_Returns_Status_Code_Result_InternalServerError(
            DateTime lastRunDateFilter,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetSavedSearchesQuery>(query =>
                        query.LastRunDateFilter == lastRunDateFilter),
                    It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new Exception());

            var result = await controller.Get(lastRunDateFilter) as StatusCodeResult;

            result.Should().NotBeNull();
            result!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
