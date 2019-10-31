using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Domain.AppLogic
{
  public class UserRating
  {
    [Key]
    public int UserRatingId { get; set; }
    public string UserName { get; set; }

    
   
    public Guid ProfileId { get; set; }
    [ForeignKey(nameof(ProfileId))]
    public Profile Profile { get; set; }
  }
}
