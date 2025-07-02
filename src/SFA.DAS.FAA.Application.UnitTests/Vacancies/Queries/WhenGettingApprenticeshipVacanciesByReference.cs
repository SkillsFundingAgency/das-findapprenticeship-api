using SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipsVacanciesByIdList;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Common.Domain.Models;

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
            azureSearchHelper.Setup(x => x.Get(It.Is<List<VacancyReference>>(y => y == query.VacancyReferences)))
                .ReturnsAsync(helperResult);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ApprenticeshipVacancies.Should().BeEquivalentTo(helperResult.Select(x => new
            {
                x.Title,
                x.EmployerName,
                x.VacancyReference,
                x.ClosingDate,
                x.ApprenticeshipType
            }));
        }
    }
}
