using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Contracts.Requests
{
  public class UserLoginRequest
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}
