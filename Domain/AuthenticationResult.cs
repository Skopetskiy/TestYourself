﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Domain
{
  public class AuthenticationResult
  {
    public string Token { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public string RefreshRoken { get; set; }
  }
}
