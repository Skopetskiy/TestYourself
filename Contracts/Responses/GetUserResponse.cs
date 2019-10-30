using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Contracts.Responses
{
  public class GetUserResponse
  {
    public string UserName { get; set; }
    //Later we need to figure out which fields we need here. In some way we still need to use token
  }
}
