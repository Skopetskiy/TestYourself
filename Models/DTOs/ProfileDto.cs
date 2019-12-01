using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Models.DTOs
{
  public class ProfileDto
  {
    //We can actually simply use CreateProfileRequest in this case.
    //But lets automap just for example and convenience in future
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LocationCity { get; set; }
    public string LocationCountry { get; set; }
  }
}
