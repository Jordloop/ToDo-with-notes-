using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
  {
    private int _id;
    private string _description;

    public Task(string Description, int Id = 0)
    {
      _id = Id;
      _description = Description;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task>{}; //  Create empty list to store all saved tasks.

      SqlConnection conn = DB.Connection(); //  Instantiates a SqlConnection object named "conn" and set it to the connection string stored as DB.Connection().
      conn.Open();  //  Opens the connection to the database.

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks;", conn);  //  Used to send SQL statements to the db. Takes two arguments: The command, and the db connection (conn).
      SqlDataReader rdr = cmd.ExecuteReader();  //  Command is actually executed when use this method on "rdr".

      while(rdr.Read())
      {
        //  Then the while loop reads the first column of each row of the result set and saves it as taskId, then it reads the second column of each row and saves it as taskDescription, then uses these values to create a new Task object, and adds this Task to allTasks.
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        Task newTask = new Task(taskDescription, taskId);
        allTasks.Add(newTask);
      }
      //  Closes data reader and connection objects.
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTasks;
    }
  }
}
