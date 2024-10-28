using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FAA.Data.AzureSearch;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddTransient<IAcsVacancySearchRepository, AcsVacancySearchRepository>();
            services.AddTransient<IAzureSearchHelper, AzureSearchHelper>();
            services.AddScoped<ISavedSearchesRepository, SavedSearchesRepository>();
        }
    }
}
