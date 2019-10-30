using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Contracts.Requests
{
  public class AddRoleRequest
  {
    public string RoleName { get; set; }
    public string UserName { get; set; }
  }
}
