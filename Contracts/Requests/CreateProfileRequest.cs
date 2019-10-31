using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Contracts.Requests
{
  public class CreateProfileRequest
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime RegisterDate { get; set; }
    public string LocationCity { get; set; }
    public string LocationCountry { get; set; } 
  }
}
