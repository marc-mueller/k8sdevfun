using ReaFx.DataAccess.EntityFramework.Storages;
using DevFun.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFun.Storage.EntityConfigurations
{
    public class DevJokeConfiguration : RelationalEntityConfigurationBase<DevJoke>
    {
        public DevJokeConfiguration(ISchemaManager schemaManager)
            : base(schemaManager)
        {
        }

        protected override string TableName => nameof(DevJoke);

        protected override void ConfigureEntity(EntityTypeBuilder<DevJoke> entity)
        {
            if (entity is null)
            {
                throw new System.ArgumentNullException(nameof(entity));
            }

            // Key
            entity.HasKey(e => e.Id);

            // Properties
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Text).IsRequired(true);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Tags).HasMaxLength(500);
            entity.Property(e => e.LikeCount).HasDefaultValue(0);

            // FK

            // Relations
            entity.HasOne(e => e.Category).WithMany();
        }
    }
}