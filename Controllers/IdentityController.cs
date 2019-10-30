using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Contracts;
using TestYourself.Contracts.Requests;
using TestYourself.Contracts.Responses;
using TestYourself.Data;
using TestYourself.Services;

namespace TestYourself.Controllers
{
  public class IdentityController: Controller
  {

    private readonly IIdentityService _identityService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly DataContext _dataContext;

    public IdentityController(IIdentityService identityService, DataContext dataContext, UserManager<IdentityUser> userManager)
    {
      _identityService = identityService;
      _dataContext = dataContext;
      _userManager = userManager;
    }


    [HttpPost(template: ApiRoutes.Identity.Register)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request) 
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new AuthFailResponse
        {
          Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        });
      }

      var authResponse = await _identityService.Register(request.Email, request.Password); 

      if (!authResponse.Success)
      {
        return BadRequest(new AuthFailResponse
        {
          Errors = authResponse.Errors
        });
      }

      return Ok(new AuthSuccessResponse
      {
        Token = authResponse.Token,
        RefreshToken = authResponse.RefreshRoken
      });
    }

    [HttpPost(template: ApiRoutes.Identity.Login)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
      var authResponse = await _identityService.Login(request.UserName, request.Password);

      if (!authResponse.Success)
      {
        return BadRequest(new AuthFailResponse
        {
          Errors = authResponse.Errors
        });
      }

      return Ok(new AuthSuccessResponse
      {
        Token = authResponse.Token,
        RefreshToken = authResponse.RefreshRoken
      });
    }

    [HttpPost(template: ApiRoutes.Identity.Refresh)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
      var authResponse = await _identityService.RefreshToken(request.Token, request.RefreshToken);

      if (!authResponse.Success)
      {
        return BadRequest(new AuthFailResponse
        {
          Errors = authResponse.Errors
        });
      }

      return Ok(new AuthSuccessResponse
      {
        Token = authResponse.Token,
        RefreshToken = authResponse.RefreshRoken
      });
    }

    [HttpPost("api/v1/add_role/admin")]
    public async Task<IActionResult> CreateAdmin([FromBody] AddRoleRequest request)
    {
      var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);

      if (user == null) return BadRequest("User not found");

      var created = await _userManager.AddToRoleAsync(user, request.RoleName);
      

      if (created != null)
      {
        await _dataContext.SaveChangesAsync();
        return Ok("Now this user is Admin");
      }
      return BadRequest("Something was wrong");
    }
  }
}
