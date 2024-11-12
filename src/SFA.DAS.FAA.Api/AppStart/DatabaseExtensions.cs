using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FAA.Data;
using SFA.DAS.FAA.Domain.Configuration;
using System;

namespace SFA.DAS.FAA.Api.AppStart
{
    public static class DatabaseExtensions
    {
        public static void AddDatabaseRegistration(this IServiceCollection services, FindApprenticeshipsApiConfiguration config, string? environmentName)
        {
            services.AddHttpContextAccessor();
            
            if (environmentName!.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddDbContext<FindApprenticeshipsDataContext>(options => options.UseSqlServer(config.DatabaseConnectionString), ServiceLifetime.Transient);
            }
            else
            {
                services.AddDbContext<FindApprenticeshipsDataContext>(ServiceLifetime.Transient);
            }

            services.AddSingleton(new EnvironmentConfiguration(environmentName));

            services.AddScoped<IFindApprenticeshipsDataContext, FindApprenticeshipsDataContext>(provider => provider.GetService<FindApprenticeshipsDataContext>()!);
            services.AddScoped(provider => new Lazy<FindApprenticeshipsDataContext>(provider.GetService<FindApprenticeshipsDataContext>()!));
        }
    }
}
