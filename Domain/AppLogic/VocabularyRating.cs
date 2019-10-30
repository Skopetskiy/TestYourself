using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Domain.AppLogic
{
  public class VocabularyRating
  {
    [Key]
    public int VocabularyRatingId { get; set; }
    public string VocabularyCaption { get; set; }

    
    public int VocabularyId { get; set; }
    [ForeignKey(nameof(VocabularyId))]
    public Vocabulary Vocabulary { get; set; }
  }
}
