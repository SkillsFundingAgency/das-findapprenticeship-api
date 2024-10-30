using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        DbContext GetContext();
        DbSet<SavedSearchEntity> SavedSearchEntities { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)); 
    }

    public class FindApprenticeshipsDataContext : DbContext, IFindApprenticeshipsDataContext
    {
        private readonly EnvironmentConfiguration _environmentConfiguration;
        public DbSet<SavedSearchEntity> SavedSearchEntities { get; set; }

        public DbContext GetContext() => this;

        private readonly FindApprenticeshipsApiConfiguration? _configuration;
        public FindApprenticeshipsDataContext() { }

        public FindApprenticeshipsDataContext(DbContextOptions options) : base(options) { }

        public FindApprenticeshipsDataContext(IOptions<FindApprenticeshipsApiConfiguration> config,
            DbContextOptions options,
            EnvironmentConfiguration environmentConfiguration) : base(options)
        {
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

            optionsBuilder.UseSqlServer(new SqlConnection
            {
                ConnectionString = _configuration.DatabaseConnectionString,
            }, options => options
                .EnableRetryOnFailure(
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
