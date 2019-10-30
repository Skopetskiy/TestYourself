using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestYourself.Authorization
{
  public class WorksHandler : AuthorizationHandler<WorksRequirements>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorksRequirements requirement)
    {
      var userEmail = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

      if (userEmail.EndsWith(requirement.DomainName))
      {
        context.Succeed(requirement);
        return Task.CompletedTask;
      }

      context.Fail();
      return Task.CompletedTask;
    }
  }
}
