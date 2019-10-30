using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Contracts.Responses
{
  public class AuthSuccessResponse
  {
    //Before the giving back a token, we need to verify the user Email!
    public string Token { get; set; }
    public string RefreshToken { get; set; }
  }
}
