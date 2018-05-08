using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using UniversityRegistrar.Models;
using UniversityRegistrar;
using MySql.Data.MySqlClient;


namespace UniversityRegistrar.Tests
{

   [TestClass]
   public class StudentTests : IDisposable
   {
       public StudentTests()
       {
           DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar;";
       }
        public void Dispose()
        {
          Student.DeleteAll();
          Course.DeleteAll();
        }
        [TestMethod]
        public void Save_SavesStudentToDatabase_StudentList()
        {
          Student testStudent = new Student("John", "2018-08-22");
          testStudent.Save();

          List<Student> testResult = Student.GetAllStudents();
          List<Student> allStudents = new List<Student> {testStudent};

          CollectionAssert.AreEqual(testResult, allStudents);
        }
        [TestMethod]
        public void Save_DatabaseAssignsIdToObject_Id()
        {
           //Arrange
           Student testStudent = new Student("John", "2018-08-22");
           testStudent.Save();

           //Act
           Student savedStudent = Student.GetAllStudents()[0];

           int result = savedStudent.GetId();
           int testId = testStudent.GetId();

           //Assert
           Assert.AreEqual(testId, result);
        }
        [TestMethod]
        public void Equals_OverrideTrueForSameName_Student()
        {
           //Arrange, Act
           Student firstStudent = new Student("John", "2018-08-22");
           Student secondStudent = new Student("John", "2018-08-22");

           //Assert
           Assert.AreEqual(firstStudent, secondStudent);
        }
        [TestMethod]
        public void Find_FindsStudentInDatabase_Student()
        {
           //Arrange
           Student testStudent = new Student("John", "2018-08-22");
           testStudent.Save();

           //Act
           Student foundStudent = Student.Find(testStudent.GetId());

           //Assert
           Assert.AreEqual(testStudent, foundStudent);
        }
        [TestMethod]
        public void GetCourses_ReturnsAllStudentCourses_CourseList()
        {
          //Arrange
          Student testStudent = new Student("John", "2018-08-22");
          testStudent.Save();

          Course testCourse1 = new Course("Mathamatics", 8911);
          testCourse1.Save();

          Course testCourse2 = new Course("Chemistry", 7911);
          testCourse2.Save();

          //Act
          testStudent.AddCourse(testCourse1);
          testStudent.AddCourse(testCourse2);
          List<Course> result = testStudent.GetCourses();
          List<Course> testList = new List<Course> {testCourse1, testCourse2};

          //Assert
          CollectionAssert.AreEqual(testList, result);
        }
    }
}
