using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.SavedSearch
{
    public class SavedSearchEntityConfiguration : IEntityTypeConfiguration<SavedSearchEntity>
    {
        public void Configure(EntityTypeBuilder<SavedSearchEntity> builder)
        {
            builder.ToTable("SavedSearch");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.UserRef).HasColumnName("UserRef").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.DateCreated).HasColumnName("DateCreated").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.LastRunDate).HasColumnName("LastRunDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.EmailLastSendDate).HasColumnName("EmailLastSendDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UnSubscribeToken).HasColumnName("UnSubscribeToken").HasColumnType("varchar(max)").IsRequired();
            builder.Property(x => x.SearchParameters).HasColumnName("SearchParameters").HasColumnType("varchar(max)").IsRequired();
        }
    }
}