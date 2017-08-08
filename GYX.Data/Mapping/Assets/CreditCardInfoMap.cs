using GYX.Data.Domain.Assets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.Assets
{
    public class CreditCardInfoMap : EntityTypeConfiguration<CreditCardInfo>
    {
        public CreditCardInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CardName).HasMaxLength(50);
            this.Property(t => t.CardNo).HasMaxLength(50);
            this.Property(t => t.LimitMoney).HasPrecision(18, 2);


            // Table & Column Mappings
            this.ToTable("CreditCardInfo");


            //外键
            this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserId);
        }
    }
}
