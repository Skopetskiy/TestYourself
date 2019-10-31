using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Data;
using TestYourself.Domain.AppLogic;

namespace TestYourself.Services
{
  public class ProfileService : IProfileService
  {
    private readonly DataContext _context;

    public ProfileService(DataContext context)
    {
      _context = context;
    }
    public async Task<bool> CreateProfile(Profile profile)
    {
      await _context.Profiles.AddAsync(profile);

      var count = await _context.SaveChangesAsync();
      return count > 0;
    }

    public async Task<bool> DeleteProfile(Guid profileId)
    {
      var profile = await GetProfileById(profileId);

      if (profile != null)
      {
        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();
        return true;
      }

      return false;
    }

    public async Task<List<Profile>> GetAllProfiles()
    {
      return await _context.Profiles
        .Include(x => x.User)
        .ToListAsync();
    }

    public async Task<Profile> GetProfileById(Guid profileId)
    {
      return await _context.Profiles
       .Include(x => x.User)
       .SingleOrDefaultAsync(x => x.ProfileId == profileId);
    }

    public async Task<Profile> LoadImage()
    {
      throw new NotImplementedException();
    }

    public async Task<bool> UpdateProfile(Profile profile, Guid profileId)
    {
      _context.Profiles.Update(profile);

      var count = await _context.SaveChangesAsync();

      return count > 0;
    }
  }
}
