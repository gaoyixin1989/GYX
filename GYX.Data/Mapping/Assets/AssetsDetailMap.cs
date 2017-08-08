using GYX.Data.Domain.Assets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.Assets
{
    public class AssetsDetailMap : EntityTypeConfiguration<AssetsDetail>
    {
        public AssetsDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(50);
            this.Property(t => t.Money).HasPrecision(18, 2);

            // Table & Column Mappings
            this.ToTable("AssetsDetail");
        }
    }
}
