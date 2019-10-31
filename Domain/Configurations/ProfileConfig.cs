using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Domain.AppLogic;

namespace TestYourself.Domain.Configurations
{
  public class ProfileConfig : IEntityTypeConfiguration<Profile>
  {
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
      builder.ToTable("Profiles");

      builder.Property(p => p.FirstName).HasColumnType("nvarchar(30)").HasMaxLength(30);
      builder.Property(p => p.LastName).HasColumnType("nvarchar(30)").HasMaxLength(30);
      //builder.Property(p => p.RegisterDate).HasDefaultValue(DateTime.Now);
    }
  }
}
