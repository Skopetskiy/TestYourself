using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Domain.AppLogic
{
  public class Profile
  {
    [Key]
    public Guid ProfileId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime RegisterDate { get; set; }
    public int VocabularyCount { get; set; }
    public int RatePosition { get; set; }
    public string LocationCity { get; set; } 
    public string LocationCountry { get; set; } //we propbably can imply a drop-list choosing here
    public int TotalWordsCount { get; set; }
    public int Level { get; set; }

   
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }
    public List<Vocabulary> Vocabularies { get; set; }

    public Profile()
    {
      Vocabularies = new List<Vocabulary>();
    }

  }
}
