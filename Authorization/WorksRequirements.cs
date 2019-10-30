using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Authorization
{
  public class WorksRequirements : IAuthorizationRequirement
  {
    public string DomainName { get;  }
    public WorksRequirements(string domainName)
    {
      DomainName = domainName;
    }
  }
}
