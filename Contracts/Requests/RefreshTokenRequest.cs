﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Contracts.Requests
{
  public class RefreshTokenRequest
  {
    public string Token { get; set; }
    public string RefreshToken { get; set; }
  }
}
