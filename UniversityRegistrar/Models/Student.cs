using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UniversityRegistrar.Models;

namespace UniversityRegistrar
{
  public class Student
  {
    private int _id;
    private string _name;
    private string _date;

    public Student(string Name, string DateOfEnrollment, int id = 0)
    {
      _name = Name;
      _date = DateOfEnrollment;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetDate()
    {
      return _date;
    }
    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
         Student newStudent = (Student) otherStudent;
         bool idEquality = this.GetId() == newStudent.GetId();
         bool nameEquality = this.GetName() == newStudent.GetName();
         bool dateEquality = this.GetDate() == newStudent.GetDate();
         return (idEquality && nameEquality && dateEquality);
       }
    }
    public override int GetHashCode()
     {
          return this.GetName().GetHashCode();
     }
     public static List<Student> GetAllStudents()
    {
      List<Student> allStudents = new List<Student>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText= @"SELECT * FROM students;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string date = rdr.GetString(2);
        Student newStudent = new Student(name, date, id);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO students (name, date_of_enrollment) VALUES (@thisName, @thisDate);";

      cmd.Parameters.Add(new MySqlParameter("@thisName", _name));
      cmd.Parameters.Add(new MySqlParameter("@thisDate", _date));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public static Student Find (int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText= @"SELECT * FROM students WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int studentId = 0;
      string name = "";
      string date = "";

      while(rdr.Read())
      {
        studentId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        date = rdr.GetString(2);
      }
      Student newStudent = new Student(name, date, studentId);

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newStudent;
    }
    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id ) VALUES (@CourseId, @StudentId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@CourseId";
      course_id.Value = newCourse.GetId();
      cmd.Parameters.Add(course_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@StudentId";
      student_id.Value = _id;
      cmd.Parameters.Add(student_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Course> GetCourses()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT courses.* FROM students
      JOIN courses_students ON (student_id = courses_students.student_id)
      JOIN courses ON (courses_students.course_id=courses.id)
      WHERE student_id = @StudentId;";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Course> courses = new List<Course> {};
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        int courseNumber = rdr.GetInt32(2);
        Course newCourse = new Course(courseName, courseNumber, courseId);
        courses.Add(newCourse);
      }
      rdr.Dispose();

      conn.Close();
      if (conn != null)
      {
         conn.Dispose();
      }
      return courses;
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public static List<Student> GetAll()
    {
        List<Student> allStudents = new List<Student> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM students;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int studentId = rdr.GetInt32(0);
          string studentName = rdr.GetString(1);
          string studentDate = rdr.GetString(2);

          Student newStudent = new Student(studentName, studentDate, studentId);
          allStudents.Add(newStudent);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allStudents;
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students WHERE id = @thisId; DELETE FROM courses_students WHERE course_id = @thisId;";

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
    public void UpdateStudent(string newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE students SET name = @name WHERE id = @searchId";
      cmd.Parameters.Add(new MySqlParameter("@searchId", _id));
      cmd.Parameters.Add(new MySqlParameter("@name", newStudent));
      cmd.ExecuteNonQuery();
      _name = newStudent;
      conn.Close();
      if (conn !=null)
      {
          conn.Dispose();
      }
    }
  }
}
