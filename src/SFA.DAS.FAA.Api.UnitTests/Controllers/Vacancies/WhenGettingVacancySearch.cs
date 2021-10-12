using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Api.Controllers;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Vacancies
{
    public class WhenGettingVacancySearch
    {
        public async Task Then_Gets_Search_Result_From_Mediator(
            string searchTerm,
            int pageNumber,
            int pageSize,
            SearchApprenticeshipVacanciesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacanciesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<SearchApprenticeshipVacanciesQuery>(query =>
                        query.SearchTerm == searchTerm &&
                        query.PageNumber == pageNumber &&
                        query.PageSize == pageSize), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await controller.Search(searchTerm, pageNumber, pageSize) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var apiModel = result.Value as GetSearchApprenticeshipVacanciesResponse;
            apiModel.Should().NotBeNull();
            apiModel.Should().BeEquivalentTo((GetSearchApprenticeshipVacanciesResponse)mediatorResult);
        }
    }
}