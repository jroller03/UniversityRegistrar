using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;

namespace UniversityRegistrar.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
          return View();
      }
      [HttpGet("/success")]
        public ActionResult Success()
        {
            return View();
        }
    }
}
