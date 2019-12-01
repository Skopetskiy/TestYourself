using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Domain.AppLogic;

namespace TestYourself.Contracts.Filters
{
  public class IndexViewModel
  {
    public FilterViewModel Filter { get; set; }
    public IEnumerable<Phone> Phones { get; set; }
  }
}
