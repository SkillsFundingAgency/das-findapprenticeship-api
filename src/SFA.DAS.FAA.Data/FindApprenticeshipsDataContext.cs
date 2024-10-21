using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Data.SavedSearch;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data
{
    public interface IFindApprenticeshipsDataContext
    {
        DbSet<SavedSearchEntity> SavedSearchEntities { get; set; }
    }

    public class FindApprenticeshipsDataContext : DbContext, IFindApprenticeshipsDataContext
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly ChainedTokenCredential _azureServiceTokenProvider;
        private readonly EnvironmentConfiguration _environmentConfiguration;
        public DbSet<SavedSearchEntity> SavedSearchEntities { get; set; }

        private readonly FindApprenticeshipsApiConfiguration? _configuration;
        public FindApprenticeshipsDataContext()
        {
        }

        public FindApprenticeshipsDataContext(DbContextOptions options) : base(options)
        {

        }
        public FindApprenticeshipsDataContext(IOptions<FindApprenticeshipsApiConfiguration> config, DbContextOptions options, ChainedTokenCredential azureServiceTokenProvider, EnvironmentConfiguration environmentConfiguration) : base(options)
        {
            _azureServiceTokenProvider = azureServiceTokenProvider;
            _environmentConfiguration = environmentConfiguration;
            _configuration = config.Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            if (_configuration == null
                || _environmentConfiguration.EnvironmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase)
                || _environmentConfiguration.EnvironmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            var connection = new SqlConnection
            {
                ConnectionString = _configuration.DatabaseConnectionString,
                AccessToken = _azureServiceTokenProvider.GetTokenAsync(new TokenRequestContext(scopes: new string[] { AzureResource })).Result.Token,
            };

            optionsBuilder.UseSqlServer(connection, options =>
                options.EnableRetryOnFailure(
                    5,
                    TimeSpan.FromSeconds(20),
                    null
                ));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SavedSearchEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
