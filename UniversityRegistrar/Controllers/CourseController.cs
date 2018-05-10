using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;

namespace UniversityRegistrar.Controllers
{
    public class CourseController : Controller
    {

        [HttpGet("/courses/index")]
        public ActionResult Index()
        {
            List<Course> allCourses = Course.GetAllCourses();
            return View(allCourses);
        }
        [HttpGet("/courses/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/courses")]
        public ActionResult Create()
        {
            Course newCourse = new Course(Request.Form["courseName"], Int32.Parse(Request.Form["courseNo"]));
            newCourse.Save();
            return RedirectToAction("Success", "Home");
        }
        [HttpGet("/courses/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Course selectedCourse = Course.Find(id);
            List<Student> courseStudents = selectedCourse.GetStudents();
            List<Student> allStudents = Student.GetAll();
            model.Add("selectedCourse", selectedCourse);
            model.Add("courseStudents", courseStudents);
            model.Add("allStudents", allStudents);
            return View(model);
        }
        [HttpPost("/courses/{courseId}/students/new")]
        public ActionResult AddStudent(int courseId)
        {
            Course course = Course.Find(courseId);
            Student student = Student.Find(Int32.Parse(Request.Form["student-id"]));
            course.AddStudent(student);
            return RedirectToAction("Details",  new { id = courseId });
        }
        [HttpGet("/courses/{courseId}/delete")]
        public ActionResult DeleteCourse(int courseId)
        {
          Course course = Course.Find(courseId);
          course.Delete();
          List<Course> allCourses = Course.GetAllCourses();
          return View("Index", allCourses);
        }
        [HttpGet("/courses/deleteall")]
        public ActionResult DeleteAll()
        {
            Course.DeleteAll();
            List<Course> allCourses = Course.GetAllCourses();
            return View("Index", allCourses);
        }
    }
}
