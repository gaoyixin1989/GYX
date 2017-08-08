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
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BookName).HasColumnName("BookName");
            this.Property(t => t.BillType).HasColumnName("BillType");
            this.Property(t => t.PayTime).HasColumnName("PayTime");
            this.Property(t => t.PayType).HasColumnName("PayType");
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.CurrencyType).HasColumnName("CurrencyType");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.DataState).HasColumnName("DataState");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
