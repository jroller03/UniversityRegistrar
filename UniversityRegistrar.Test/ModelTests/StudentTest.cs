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
    }
}
