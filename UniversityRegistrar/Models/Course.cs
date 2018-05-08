using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UniversityRegistrar.Models;

namespace UniversityRegistrar
{
  public class Course
  {
    private int _id;
    private string _courseName;
    private int _courseNo;

    public Course(string CourseName, int CourseNumber, int id = 0)
    {
      _courseName = CourseName;
      _courseNo = CourseNumber;
      _id = id;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetCourseName()
    {
      return _courseName;
    }
    public int GetCourseNo()
    {
      return _courseNo;
    }
    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
         Course newCourse = (Course) otherCourse;
         bool idEquality = this.GetId() == newCourse.GetId();
         bool courseNameEquality = this.GetCourseName() == newCourse.GetCourseName();
         bool courseNoEquality = this.GetCourseNo() == newCourse.GetCourseNo();
         return (idEquality && courseNameEquality && courseNoEquality);
       }
    }
    public override int GetHashCode()
    {
         return this.GetCourseName().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO courses (course_name, course_number) VALUES (@thisCourseName, @thisCourseNumber);";

      cmd.Parameters.Add(new MySqlParameter("@thisCourseName", _courseName));
      cmd.Parameters.Add(new MySqlParameter("@thisCourseNumber", _courseNo));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public static List<Course> GetAllCourses()
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        int courseNo = rdr.GetInt32(2);
        Course newCourse = new Course(courseName, courseNo, id);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allCourses;
    }
    public static Course Find (int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int courseId = 0;
      string courseName = "";
      int courseNo = 0;

      while(rdr.Read())
      {
        courseId = rdr.GetInt32(0);
        courseName = rdr.GetString(1);
        courseNo = rdr.GetInt32(2);
      }
      Course newCourse = new Course(courseName, courseNo, courseId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newCourse;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

  }
}
