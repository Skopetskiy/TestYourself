using System.ComponentModel.DataAnnotations;

namespace TestYourself.Contracts.Requests
{
  public class UserRegistrationRequest
  {
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
