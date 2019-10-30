using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestYourself.Data;
using TestYourself.Domain;
using TestYourself.Options;

namespace TestYourself.Services
{
  public class IdentityService : IIdentityService
  {

    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly DataContext _dataContext;
    private readonly RoleManager<IdentityRole> _roleManager;


    public IdentityService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, DataContext dataContext) 
    {
      _jwtSettings = jwtSettings;
      _userManager = userManager;
      _tokenValidationParameters = tokenValidationParameters;
      _dataContext = dataContext;
      _roleManager = roleManager;
    }

    public async Task<AuthenticationResult> Login(string email, string password)
    {
      var user = await _userManager.FindByEmailAsync(email);

      if (user == null)
        return new AuthenticationResult
        {
          Errors = new[] { "User does not exists" }
        };

      bool userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

      if (!userHasValidPassword)
      {
        return new AuthenticationResult
        {
          Errors = new[] { "User pass is wrong" }
        };
      }
      
      return await GenerateAuthenticationResultForUser(user);
    }



    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      try
      {
        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
        if (!IsJwtWithCalidSecurityAlgorithm(validatedToken))
        {
          return null;
        }
        return principal;
      }
      catch
      {
        return null;
      }
    }

    private bool IsJwtWithCalidSecurityAlgorithm(SecurityToken validatedToken)
    {
      return (validatedToken is JwtSecurityToken jwtSecurityToken) && 
        jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }

    public async Task<AuthenticationResult> Register(string email, string password)
    {
      var existingUser = await _userManager.FindByEmailAsync(email);

      if (existingUser != null)
        return new AuthenticationResult
        {
          Errors = new[] { "User with such Email already exists" }
        };

      var newUserId = Guid.NewGuid();
      var newUser = new IdentityUser
      {
        Id = newUserId.ToString(),
        Email = email,
        UserName = email
      };

      var createdUser = await _userManager.CreateAsync(newUser, password);

      if (!createdUser.Succeeded)
      {
        return new AuthenticationResult
        {
          Errors = createdUser.Errors.Select(x => x.Description)
        };
      }

      //await _userManager.AddClaimAsync(newUser, new Claim("tags.view", "true"));

      return await GenerateAuthenticationResultForUser(newUser);
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultForUser(IdentityUser user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

      var claims = new List<Claim>
        {
        new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
          new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
          new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
          new Claim(type: "id", value: user.Id)
        };

      var userClaims = await _userManager.GetClaimsAsync(user);
      claims.AddRange(userClaims);

      //var userRoles = await _userManager.GetRolesAsync(user);
      //foreach (var userRole in userRoles)
      //{
      //  claims.Add(new Claim(ClaimTypes.Role, userRole));
      //  var role = await _roleManager.FindByNameAsync(userRole);
      //  if (role == null) continue;
      //  var roleClaims = await _roleManager.GetClaimsAsync(role);

      //  foreach (var roleClaim in roleClaims)
      //  {
      //    if (claims.Contains(roleClaim))
      //      continue;

      //    claims.Add(roleClaim);
      //  }
      //}

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims: claims),
        Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);

      var refreshToken = new RefreshToken
      {
        JwtId = token.Id,
        UserId = user.Id,
        TokenCreationDate = DateTime.UtcNow,
        ExpiryDate = DateTime.UtcNow.AddMonths(6)
      };

      await _dataContext.RefreshTokens.AddAsync(refreshToken);
      await _dataContext.SaveChangesAsync();

      return new AuthenticationResult
      {
        Success = true,
        Token = tokenHandler.WriteToken(token),
        RefreshRoken = refreshToken.Token
      };
    }

    public async Task<AuthenticationResult> RefreshToken(string token, string refreshToken)
    {
      var validatedToken = GetPrincipalFromToken(token);
      if (validatedToken == null)
      {
        return new AuthenticationResult
        {
          Errors = new[] { "Invalid Token" }
        };
      }

      var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

      var expiryDateTimeUtc = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)
        .AddSeconds(expiryDateUnix);

      if (expiryDateTimeUtc > DateTime.UtcNow)
      {
        return new AuthenticationResult { Errors = new[] { "This token has not expired yet" } };
      }

      var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

      var storedRefreshToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

      if (storedRefreshToken == null)
      {
        return new AuthenticationResult { Errors = new[] { "This token does not exist" } };
      }

      if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
      {
        return new AuthenticationResult { Errors = new[] { "This token has expired. You cannot use it" } };
      }

      if (storedRefreshToken.Invalidated)
      {
        return new AuthenticationResult { Errors = new[] { "This token has been invalidated" } };
      }

      if (storedRefreshToken.Used)
      {
        return new AuthenticationResult { Errors = new[] { "This token has been used" } };
      }

      if (storedRefreshToken.JwtId != jti)
      {
        return new AuthenticationResult { Errors = new[] { "This token does not match this JWT" } };
      }

      storedRefreshToken.Used = true;
      _dataContext.RefreshTokens.Update(storedRefreshToken);
      await _dataContext.SaveChangesAsync();

      var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
      return await GenerateAuthenticationResultForUser(user);
    }
  }
}
