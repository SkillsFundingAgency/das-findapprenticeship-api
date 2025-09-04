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
                ApprenticeshipTypes = request.ApprenticeshipTypes,
                Categories = request.Categories,
                DataSources = request.DataSources,
                DisabilityConfident = request.DisabilityConfident,
                DistanceInMiles = request.DistanceInMiles,
                ExcludeNational = request.ExcludeNational,
                Lat = request.Lat,
                Levels = request.Levels,
                Lon = request.Lon,
                RouteIds = request.RouteIds,
                SearchTerm = request.SearchTerm,
                WageType = request.WageType,
            };
            return await acsVacancySearchRepository.Count(model);
        }
    }
}