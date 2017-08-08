using GYX.Data.Domain.Assets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.Assets
{
    public class AssetsMap : EntityTypeConfiguration<AssetsTable>
    {
        public AssetsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Total).HasPrecision(18, 2);

            // Table & Column Mappings
            this.ToTable("Assets");
        }
    }
}
