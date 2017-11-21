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

            this.HasOptional(a => a.Parent).WithMany().HasForeignKey(a=>a.ParentId);
        }
    }
}
