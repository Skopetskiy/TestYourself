using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestYourself.Contracts;
using TestYourself.Contracts.Requests;
using TestYourself.Contracts.Responses;
using TestYourself.Domain.AppLogic;
using TestYourself.Services;

namespace TestYourself.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  public class ProfileController : Controller
  {
    private readonly IProfileService _profiles;

    public ProfileController(IProfileService profiles)
    {
      _profiles = profiles;
    }

    [HttpGet(ApiRoutes.Profiles.GetAll)]
    public async Task<IActionResult> Get()
    {
      return Ok(await _profiles.GetAllProfiles());
    }

    [HttpGet(Contracts.ApiRoutes.Profiles.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid profileId)
    {
      var profile = await _profiles.GetProfileById(profileId);

      if (profile == null)
      {
        return NotFound();
      }
      return Ok(profile);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut(ApiRoutes.Profiles.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid profileId, [FromBody] UpdateProfileRequest request)
    {
      var profile = await _profiles.GetProfileById(profileId);
      profile.FirstName = request.FirstName;
      profile.LastName = request.LastName;
      profile.LocationCity = request.LocationCity;
      profile.LocationCountry = request.LocationCountry;

      if (await _profiles.UpdateProfile(profile, profileId))
      {
        return Ok(profile);
      }

      return NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete(ApiRoutes.Profiles.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid profileId)
    {
      if (await _profiles.DeleteProfile(profileId))
      {
        return NoContent();
      }

      return NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost(ApiRoutes.Profiles.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProfileRequest profileRequest)
    {
      var newProfileId = Guid.NewGuid();

      var profile = new Profile
      {
        ProfileId = newProfileId,
        FirstName = profileRequest.FirstName,
        LastName = profileRequest.LastName,
        AvatarUrl = "https://www.civhc.org/wp-content/uploads/2018/10/question-mark.png",
       // RegisterDate = DateTime.Now,
        //VocabularyCount = _vocabularies.GetProfileVocabularyCount();
        VocabularyCount = 0,
        //RatePosition = _vocabularyRatings.GetProfileRatingPosition();
        RatePosition = 0,
        LocationCity = profileRequest.LocationCity,
        LocationCountry = profileRequest.LocationCountry,
        //TotalWordsCount = _vocabularies.GetTotalWordsCount(); // Actually we can at it with LINQ
        TotalWordsCount = 0,
        //Level = _userRatings.GetUserRatingById(profileId);
        Level = 0
      };


      await _profiles.CreateProfile(profile);

      //I cant come up with situation we can use it but let it be
      var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
      var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", profile.ProfileId.ToString());

      var response = new ProfileResponse 
      { 
        FirstName = profile.FirstName,
        LastName = profile.LastName
      };

      return Created(locationUrl, response);
    }
  }
  
}
