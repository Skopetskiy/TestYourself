using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Domain.AppLogic;

namespace TestYourself.Domain.Configurations
{
  public class VocabularyValuesConfig : IEntityTypeConfiguration<VocabularyValues>
  {
    public void Configure(EntityTypeBuilder<VocabularyValues> builder)
    {
      builder.ToTable("VocabularyValues");

      builder.Property(p => p.Word).HasColumnType("nvarchar(1000)").HasMaxLength(1000);
      builder.Property(p => p.WordTranslation).HasColumnType("nvarchar(1000)").HasMaxLength(1000);
    }
  }
}
