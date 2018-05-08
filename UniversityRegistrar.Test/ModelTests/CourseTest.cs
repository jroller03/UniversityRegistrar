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




    }
}
