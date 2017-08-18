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
            

            // Table & Column Mappings
            this.ToTable("Assets");

            // 外键
            this.HasMany(t => t.DetailList).WithOptional().HasForeignKey(t => t.AssetsId);
        }
    }
}
