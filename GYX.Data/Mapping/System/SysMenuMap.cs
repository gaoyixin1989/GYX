using GYX.Data.Domain.System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.System
{
    public class SysMenuMap : EntityTypeConfiguration<SysMenu>
    {
        public SysMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MenuText).HasMaxLength(50);
            this.Property(t => t.MenuUrl).HasMaxLength(200);
            this.Property(t => t.MenuType).HasMaxLength(50);
            this.Property(t => t.ImgUrl_Small).HasMaxLength(50);
            this.Property(t => t.ImgUrl_Big).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SysMenu");
        }
    }
}
