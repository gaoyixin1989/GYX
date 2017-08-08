using GYX.Data.Domain.System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.System
{
    public class SysDictMap : EntityTypeConfiguration<SysDict>
    {
        public SysDictMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DictCode).HasMaxLength(50);
            this.Property(t => t.DictText).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SysDict");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DictCode).HasColumnName("DictCode");
            this.Property(t => t.DictText).HasColumnName("DictText");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.IsDefalut).HasColumnName("IsDefalut");
            this.Property(t => t.IsUse).HasColumnName("IsUse");
            this.Property(t => t.DataState).HasColumnName("DataState");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
