using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Domain;

namespace TestYourself.Contracts.Requests
{
  public class CreatePostRequest
  {
    public string Name { get; set; }
    public IEnumerable<string> Tags { get; set; }
  }
}
