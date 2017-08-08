using GYX.Data.Domain.Assets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.Assets
{
    public class CreditCardTakeRecordMap : EntityTypeConfiguration<CreditCardTakeRecord>
    {
        public CreditCardTakeRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TakeMoney).HasPrecision(18, 2);
            this.Property(t => t.Fee).HasPrecision(18, 2);

            // Table & Column Mappings
            this.ToTable("CreditCardTakeRecord");

            //外键
            this.HasOptional(t => t.CardObj).WithMany().HasForeignKey(d => d.CardId);

        }
    }
}
