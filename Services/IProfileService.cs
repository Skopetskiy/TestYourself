using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Domain.AppLogic;

namespace TestYourself.Services
{
  public interface IProfileService
  {
    Task<List<Profile>> GetAllProfiles();
    Task<Profile> GetProfileById(Guid profileId);
    Task<Profile> LoadImage();
    //Task<bool> IsUserOwnerOfProfile(string profileId, Profile profile);
    Task<bool> UpdateProfile(Profile profile, Guid profileId);
    Task<bool> DeleteProfile(Guid profileId);
    Task<bool> CreateProfile(Profile profile);
  }
}
