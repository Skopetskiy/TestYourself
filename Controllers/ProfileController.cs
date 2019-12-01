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
using TestYourself.Models.DTOs;
using TestYourself.Services;

namespace TestYourself.Controllers
{
 // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  public class ProfileController : Controller
  {
    private readonly IProfileService _profiles;
    private readonly AutoMapper.IMapper _mapper;

    public ProfileController(IProfileService profiles, AutoMapper.IMapper mapper)
    {
      _profiles = profiles;
      _mapper = mapper;
    }

    [HttpGet(ApiRoutes.Profiles.GetAll)]
    public async Task<IActionResult> Get()
    {
      var profiles = await _profiles.GetAllProfiles();
      var profileDtos = _mapper.Map<List<ProfileDto>>(profiles);
      return Ok(profileDtos);
    }

    [HttpGet(Contracts.ApiRoutes.Profiles.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid profileId)
    {
      var profile = await _profiles.GetProfileById(profileId);

      if (profile == null)
      {
        return NotFound();
      }

      var profileDto = _mapper.Map<ProfileDto>(profile);

      return Ok(profileDto);
    }

    [HttpPut(ApiRoutes.Profiles.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid profileId, [FromBody] UpdateProfileRequest request)
    {
      //Can be improved by checking vallues to NULL or initializing update-form with existing values(Front-side)
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


    [HttpDelete(ApiRoutes.Profiles.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid profileId)
    {
      if (await _profiles.DeleteProfile(profileId))
      {
        return NoContent();
      }

      return NotFound();
    }

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
        RegisterDate = DateTime.Now,
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
        //As well we need to use Claims and assign UserId value with current user ID
      };


      await _profiles.CreateProfile(profile);

      //I cant come up with situation we can use it but let it be
      var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
      var locationUrl = baseUrl + "/" + ApiRoutes.Profiles.Get.Replace("{profileId}", profile.ProfileId.ToString());

      var response = new ProfileResponse 
      { 
        FirstName = profile.FirstName,
        LastName = profile.LastName
      };

      return Created(locationUrl, response);
    }
  }
  
}
