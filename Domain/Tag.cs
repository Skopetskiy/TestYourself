﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestYourself.Domain
{
  public class Tag
  {
    [Key]
    public string Name { get; set; }
    public string CreatorId { get; set; }

    [ForeignKey(nameof(CreatorId))]
    public IdentityUser CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
  }
}
