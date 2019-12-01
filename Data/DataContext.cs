using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Contracts.Requests;
using TestYourself.Domain;
using TestYourself.Domain.AppLogic;
using TestYourself.Domain.Configurations;

namespace TestYourself.Data
{
  public class DataContext : IdentityDbContext
  {
    public DataContext(DbContextOptions options) : base(options){ }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }
    public DbSet<Vocabulary> Vocabularies { get; set; }
    public DbSet<VocabularyRating> VocabularyRatings { get; set; }
    public DbSet<VocabularyValues> VocabularyValues { get; set; }
    public DbSet<VocabularyProfile> VocabularyProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new ProfileConfig());
      builder.ApplyConfiguration(new VocabularyValuesConfig());


      base.OnModelCreating(builder);

      builder.Entity<PostTag>().Ignore(xx => xx.Post).HasKey(x => new { x.PostId, x.TagName });
    }
  }
}
