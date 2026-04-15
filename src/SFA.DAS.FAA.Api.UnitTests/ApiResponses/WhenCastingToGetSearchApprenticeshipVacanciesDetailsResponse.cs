using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Application.Vacancies.Queries.SearchApprenticeshipVacancies;
using System.Linq;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.UnitTests.ApiResponses;

public class WhenCastingToGetSearchApprenticeshipVacanciesDetailsResponse
{
    [Test, AutoData]
    public void Then_Maps_Fields(SearchApprenticeshipVacanciesResult source)
    {
        source.ApprenticeshipVacanciesWithDetails.ToList().ForEach(v => v.Wage = null);
        source.ApprenticeshipVacanciesWithDetails.ToList().ForEach(x => x.VacancySource = nameof(DataSource.Raa));
        
        var response = (GetSearchApprenticeshipVacanciesDetailsResponse)source;

        response.TotalFound.Should().Be(source.TotalFound);
        response.Total.Should().Be(source.Total);
        response.Should().BeEquivalentTo(source, options => options
            .Excluding(c => c.ApprenticeshipVacancies)
            .Excluding(c=>c.ApprenticeshipVacanciesWithDetails));
        response.ApprenticeshipVacancies.Should().BeEquivalentTo(source.ApprenticeshipVacanciesWithDetails, options => options
            .ExcludingMissingMembers());
    }
}
