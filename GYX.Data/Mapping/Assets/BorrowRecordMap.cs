using GYX.Data.Domain.Assets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.Assets
{
    public class BorrowRecordMap : EntityTypeConfiguration<BorrowRecord>
    {
        public BorrowRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            

            // Table & Column Mappings
            this.ToTable("BorrowRecord");
            
        }
    }
}
