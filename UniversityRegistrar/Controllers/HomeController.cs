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
      [HttpGet("/view-all-students")]
      public ActionResult ListStudents()
      {
        List<Student> allStudents = Student.GetAllStudents();
        return View("ViewAllStudents", allStudents);
      }
      [HttpGet("/view-all-courses")]
      public ActionResult ListCourses()
      {
        List<Course> allCourses = Course.GetAllCourses();
        return View("ViewAllCourses", allCourses);
      }

    }
}
