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
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuText).HasColumnName("MenuText");
            this.Property(t => t.MenuUrl).HasColumnName("MenuUrl");
            this.Property(t => t.MenuType).HasColumnName("MenuType");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.ImgUrl_Small).HasColumnName("ImgUrl_Small");
            this.Property(t => t.ImgUrl_Big).HasColumnName("ImgUrl_Big");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.IsShow).HasColumnName("IsShow");
            this.Property(t => t.IsUse).HasColumnName("IsUse");
            this.Property(t => t.DataState).HasColumnName("DataState");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
