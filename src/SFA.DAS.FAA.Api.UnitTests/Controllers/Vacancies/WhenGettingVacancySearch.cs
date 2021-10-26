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
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Api.UnitTests.Controllers.Vacancies
{
    public class WhenGettingVacancySearch
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Search_Result_From_Mediator(
            int pageNumber,
            int pageSize,
            int ukprn,
            string accountPublicHashedId,
            string accountLegalEntityPublicHashedId,
            SearchApprenticeshipVacanciesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacanciesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<SearchApprenticeshipVacanciesQuery>(query =>
                        query.PageNumber == pageNumber &&
                        query.PageSize == pageSize && 
                        query.Ukprn == ukprn && 
                        query.AccountPublicHashedId == accountPublicHashedId && 
                        query.AccountLegalEntityPublicHashedId == accountLegalEntityPublicHashedId), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await controller.Search(pageNumber, pageSize, ukprn, accountPublicHashedId, accountLegalEntityPublicHashedId) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var apiModel = result.Value as GetSearchApprenticeshipVacanciesResponse;
            apiModel.Should().NotBeNull();
            apiModel.Should().BeEquivalentTo((GetSearchApprenticeshipVacanciesResponse)mediatorResult);
        }
    }
}