using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Extensions;

namespace TestYourself.Domain.AppLogic
{
  public class Vocabulary
  {
    [Key]
    public int VocabularyId { get; set; }
    public string Caption { get; set; }
    public VocabularyTypes Type { get; set; }
    public bool IsOfficial { get; set; }
    public int Grade { get; set; }//No validation generally. Can be increased only at the succesfull end of test
    public int? CreatorId { get; set; }


    public int VocabularyValuesId { get; set; }
    [ForeignKey(nameof(VocabularyValuesId))]
    public VocabularyValues VocabularyValues { get; set; }
    
    //public int ProfileId { get; set; }
    //[ForeignKey(nameof(ProfileId))]
    //public List<Profile> Profiles { get; set; }

    //public Vocabulary()
    //{
    //  Profiles = new List<Profile>();
    //}
  }
}
