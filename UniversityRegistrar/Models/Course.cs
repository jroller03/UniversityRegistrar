using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UniversityRegistrar.Models;

namespace UniversityRegistrar
{
  public class Course
  {

    private string _courseName;
    private int _courseNo;
    private int _id;

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

      cmd.CommandText = @"INSERT INTO courses (course_name, course_number) VALUES (@thisCourseName, @thisCourseNo);";

      cmd.Parameters.Add(new MySqlParameter("@thisCourseName", this._courseName));
      cmd.Parameters.Add(new MySqlParameter("@thisCourseNo", this._courseNo));

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
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses WHERE id = @thisId; DELETE FROM courses_students WHERE course_id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = this.GetId();
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO course (student_id, course_id) VALUES (@studentId, @CourseId);";

        MySqlParameter student_id = new MySqlParameter();
        student_id.ParameterName = "@StudentId";
        student_id.Value = newStudent.GetId();
        cmd.Parameters.Add(student_id);

        MySqlParameter _id = new MySqlParameter();
        _id.ParameterName = "@CourseId";
        _id.Value = _id;
        cmd.Parameters.Add(_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public List<Student> GetStudents()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT students.* FROM courses
            JOIN course_students ON (courses.course_id = course_students.course_id)
            JOIN students ON (course_students.student_id = students.student_id)
            WHERE courses.course_id = @CourseId;";

        MySqlParameter idParameter = new MySqlParameter();
        idParameter.ParameterName = "@CourseId";
        idParameter.Value = _id;
        cmd.Parameters.Add(idParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> studentIds = new List<int> {};
        while(rdr.Read())
        {
            int studentId = rdr.GetInt32(0);
            studentIds.Add(studentId);
        }
        rdr.Dispose();

        List<Student> students = new List<Student> {};
        foreach (int studentId in studentIds)
        {
            var studentQuery = conn.CreateCommand() as MySqlCommand;
            studentQuery.CommandText = @"SELECT * FROM students WHERE student_id = @StudentId;";

            MySqlParameter studentIdParameter = new MySqlParameter();
            studentIdParameter.ParameterName = "@StudentId";
            studentIdParameter.Value = studentId;
            studentQuery.Parameters.Add(studentIdParameter);

            var studentQueryRdr = studentQuery.ExecuteReader() as MySqlDataReader;
            while(studentQueryRdr.Read())
            {
                int StudentId = studentQueryRdr.GetInt32(0);
                string studentName = studentQueryRdr.GetString(1);
                string studentDate = studentQueryRdr.GetString(2);
                Student foundStudent = new Student(studentName, studentDate, studentId);
                students.Add(foundStudent);
            }
            studentQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return students;
    }
  }
}
