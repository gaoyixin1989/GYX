using GYX.Data.Domain.System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GYX.Data.Mapping.System
{
    public class SysUserMap : EntityTypeConfiguration<SysUser>
    {
        public SysUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.RealName).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SysUser");
        }
    }
}
