using GYX.Data.Domain.Assets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.Assets
{
    public class AccountBookMap : EntityTypeConfiguration<AccountBook>
    {
        public AccountBookMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BookName).HasMaxLength(50);
            this.Property(t => t.BillType).HasMaxLength(50);
            this.Property(t => t.CurrencyType).HasMaxLength(50);
            this.Property(t => t.Money).HasPrecision(18, 2);

            // Table & Column Mappings
            this.ToTable("AccountBook");
        }
    }
}
