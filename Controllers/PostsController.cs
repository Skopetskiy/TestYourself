using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestYourself.Contracts;
using TestYourself.Contracts.Requests;
using TestYourself.Contracts.Responses;
using TestYourself.Domain;
using TestYourself.Extensions;
using TestYourself.Services;

namespace TestYourself.Controllers
{
  public class PostsController : Controller
  {
    private readonly IPostService _posts;

    public PostsController(IPostService posts)
    {
      _posts = posts;
    }

    [HttpGet(ApiRoutes.Posts.GetAll)]
    public async Task<IActionResult> Get()
    {
      return Ok( await _posts.GetPosts());
    }

    [HttpGet(Contracts.ApiRoutes.Posts.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid postId)
    {
      var post = await _posts.GetPostById(postId);

      if (post == null)
      {
        return NotFound(); 
      }
      return Ok(post);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut(ApiRoutes.Posts.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostContract request)
    {
      bool userOwnsPost = await _posts.UserOwnsPost(postId, HttpContext.GetUserId());

      if (!userOwnsPost)
      {
        return BadRequest(error: new { error = "You are not owner of this post" });
      }

      var post = await _posts.GetPostById(postId);
      post.Name = request.Name;

      if (await _posts.UpdatePost(post))
      {
        return Ok(post);
      }

      return NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete(ApiRoutes.Posts.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid postId)
    {
      bool userOwnsPost = await _posts.UserOwnsPost(postId, HttpContext.GetUserId());

      if (!userOwnsPost)
      {
        return BadRequest(error: new { error = "You are not owner of this post" });
      }

      if (await _posts.DeletePost(postId))
      {
        return NoContent();
      }
      
      return NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost(ApiRoutes.Posts.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
    {
      var newPostId = Guid.NewGuid();

      var post = new Post
      {
        Id = newPostId,
        Name = postRequest.Name,
        UserId = HttpContext.GetUserId(),
        Tags = postRequest.Tags.Select(x => new PostTag { PostId = newPostId, TagName = x }).ToList()
      };


      await _posts.CreatePost(post);

      var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
      var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

      var response = new PostResponse { Id = post.Id };

      return Created(locationUrl, response);
    }
  }
}
