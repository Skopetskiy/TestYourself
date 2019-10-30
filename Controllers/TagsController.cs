using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Contracts;
using TestYourself.Services;

namespace TestYourself.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class TagsController : Controller
  {
    //Created only to show Policy and Tag access example
    private readonly IPostService _postService;

    public TagsController(IPostService postService)
    {
      _postService = postService;
    }

    [HttpGet(ApiRoutes.Tags.GetAll)]
    //[Authorize(Policy = "TagViewer")]
    //[Authorize(Policy = "WorksForDima")]
    public async Task<IActionResult> GetAll()
    {
      return Ok(await _postService.GetAllTags());
    }
  }
}


  