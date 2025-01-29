using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Vacancies.Queries.GetApprenticeshipVacancyCount
{
    public class GetApprenticeshipVacancyCountQueryHandler(IAcsVacancySearchRepository acsVacancySearchRepository)
        : IRequestHandler<GetApprenticeshipVacancyCountQuery, int>
    {
        public async Task<int> Handle(GetApprenticeshipVacancyCountQuery request, CancellationToken cancellationToken)
        {
            var model = new FindVacanciesCountModel
            {
                SearchTerm = request.SearchTerm,
                Categories = request.Categories,
                RouteIds = request.RouteIds,
                Lat = request.Lat,
                Lon = request.Lon,
                DistanceInMiles = request.DistanceInMiles,
                NationWideOnly = request.NationWideOnly,
                Levels = request.Levels,
                DisabilityConfident = request.DisabilityConfident,
                DataSources = request.DataSources,
                WageType = request.WageType,
            };
            return await acsVacancySearchRepository.Count(model);
        }
    }
}