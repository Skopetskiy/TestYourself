using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Domain.AppLogic
{
  public class VocabularyProfile
  {
    [Key]
    public int VocabularyProfileId { get; set; }


    public int VocabularyId { get; set; }
    [ForeignKey(nameof(VocabularyId))]
    public Vocabulary Vocabulary { get; set; }
    

    public Guid ProfileId { get; set; }
    [ForeignKey(nameof(ProfileId))]
    public Profile Profile { get; set; }
  }
}
