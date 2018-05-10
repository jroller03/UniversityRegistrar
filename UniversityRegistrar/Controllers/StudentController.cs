using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;

namespace UniversityRegistrar.Controllers
{
    public class StudentController : Controller
    {

        [HttpGet("/students/index")]
        public ActionResult Index()
        {
            List<Student> allStudents = Student.GetAll();
            return View(allStudents);
        }
        [HttpGet("/students/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/students")]
        public ActionResult Create()
        {
            Student newStudent = new Student(Request.Form["student-name"], Request.Form["student-date"]);
            newStudent.Save();
            return RedirectToAction("Success", "Home");
        }
        [HttpGet("/students/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Student selectedStudent = Student.Find(id);
            List<Course> studentCourses = selectedStudent.GetCourses();
            List<Course> allCourses = Course.GetAll();
            model.Add("selectedStudent", selectedStudent);
            model.Add("studentCourses", studentCourses);
            model.Add("allCourses", allCourses);
            return View(model);
        }
        [HttpPost("/students/{studentId}/courses/new")]
        public ActionResult AddCourse(int studentId)
        {
            Student student = Student.Find(studentId);
            Course course = Course.Find(Int32.Parse(Request.Form["course-id"]));
            student.AddCourse(course);
            return RedirectToAction("Details",  new { id = studentId });
        }
        [HttpGet("/students/{studentId}/delete")]
        public ActionResult DeleteStudent(int studentId)
        {
            Student student = Student.Find(studentId);
            student.Delete();
            List<Student> allStudents = Student.GetAll();
            return View("Index", allStudents);
        }
        [HttpGet("/students/delete")]
        public ActionResult DeleteAll()
        {
            Student.DeleteAll();
            List<Student> allStudents = Student.GetAll();
            return View("Index", allStudents);
        }
    }
}
