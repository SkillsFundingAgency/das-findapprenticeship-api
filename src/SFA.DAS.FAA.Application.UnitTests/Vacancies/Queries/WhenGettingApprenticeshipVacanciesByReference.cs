using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipsVacanciesByIdList;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    [TestFixture]
    public class WhenGettingApprenticeshipVacanciesByReference
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Azure_Search_Helper(
            GetApprenticeshipVacanciesByReferenceQuery query,
            List<ApprenticeshipSearchItem> helperResult,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            GetApprenticeshipVacanciesByReferenceQueryHandler handler)
        {
            azureSearchHelper.Setup(x => x.Get(It.Is<List<string>>(y => y == query.VacancyReferences)))
                .ReturnsAsync(helperResult);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ApprenticeshipVacancies.Should().BeEquivalentTo(helperResult.Select(x => new
            {
                x.Title,
                x.EmployerName,
                x.VacancyReference,
                x.ClosingDate
            }));
        }
    }
}
