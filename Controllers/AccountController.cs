using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Contracts.Requests;
using TestYourself.Data;
using TestYourself.Services;

namespace TestYourself.Controllers
{
  public class AccountController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly DataContext _dataContext;

    public AccountController(DataContext dataContext, UserManager<IdentityUser> userManager)
    {
      _dataContext = dataContext;
      _userManager = userManager;
    }

    [HttpPost(template: "api/v1/login")]
    public async Task<ActionResult<IdentityUser>> Login([FromBody] UserLoginRequest request)
    {
      var user = await _userManager.FindByNameAsync(request.UserName);

      if (user == null)
        return (null);

      bool userHasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

      if (!userHasValidPassword)
        return (null);

      return Ok(user);
    }

    [HttpPost(template: "api/v1/register")]
    public async Task<ActionResult<IdentityUser>> Register([FromBody]UserRegistrationRequest model)
    {
      var user = new IdentityUser() { UserName = model.UserName, Email = model.Email };

      await _userManager.CreateAsync(user, model.Password);
      await _dataContext.SaveChangesAsync(); // Mb CreateAsync already did it but just to be sure
      return Created("", user);
    }

    [HttpGet("api/v1/users")]
    public async Task<IActionResult> GetUsers()
    {
      var users = await _dataContext.Users.ToListAsync();

      return Ok(users.Select(names => names.UserName).ToList());
    }
  }
}
