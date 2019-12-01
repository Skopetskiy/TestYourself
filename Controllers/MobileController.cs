using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Contracts.Filters;
using TestYourself.Data;
using TestYourself.Domain.AppLogic;

namespace TestYourself.Controllers
{
  [Route("api/v1/[controller]")]
  public class MobileController : Controller
  {
    private readonly MobileContext db;
    public MobileController(MobileContext context)
    {
      db = context;
    }
    [HttpGet]
    public async Task<IActionResult> Index(FilterViewModel filter)
    {
      var phones = await db.GetPhones(filter.MinPrice, filter.MaxPrice, filter.Name);
      var model = new IndexViewModel { Phones = phones, Filter = filter };
      return Ok(model);
    }


    //[HttpPost]
    //public async Task<IActionResult> Create(Phone p)
    //{
    //  if (ModelState.IsValid)
    //  {
    //    await db.Create(p);
    //    return RedirectToAction("Index");
    //  }
    //  return Ok(p);
    //}
  }
}
