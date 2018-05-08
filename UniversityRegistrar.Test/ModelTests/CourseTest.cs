using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using UniversityRegistrar.Models;
using UniversityRegistrar;
using MySql.Data.MySqlClient;


namespace UniversityRegistrar.Tests
{

   [TestClass]
   public class CourseTests : IDisposable
   {
       public CourseTests()
       {
           DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar;";
       }
        public void Dispose()
        {
          Course.DeleteAll();
          Student.DeleteAll();
        }
        [TestMethod]
        public void Save_SavesCourseToDatabase_CourseList()
        {
          Course testCourse = new Course("Physics", 11212);
          testCourse.Save();

          List<Course> testResult = Course.GetAllCourses();
          List<Course> allCities = new List<Course> {testCourse};

          CollectionAssert.AreEqual(testResult, allCities);
        }
        [TestMethod]
        public void Save_DatabaseAssignsIdToObject_Id()
        {
           //Arrange
           Course testCourse = new Course("Maths", 89892);
           testCourse.Save();

           //Act
           Course savedCourse = Course.GetAllCourses()[0];

           int result = savedCourse.GetId();
           int testId = testCourse.GetId();

           //Assert
           Assert.AreEqual(testId, result);
        }
        [TestMethod]
        public void Equals_OverrideTrueForSameName_Course()
        {
           //Arrange, Act
           Course firstCourse = new Course("Maths", 89892);
           Course secondCourse = new Course("Maths", 89892);

           //Assert
           Assert.AreEqual(firstCourse, secondCourse);
        }
        [TestMethod]
        public void Find_FindsCourseInDatabase_Course()
        {
           //Arrange
           Course testCourse = new Course("Data Structure", 71232);
           testCourse.Save();

           //Act
           Course foundCourse = Course.Find(testCourse.GetId());

           //Assert
           Assert.AreEqual(testCourse, foundCourse);
        }

    }
}
