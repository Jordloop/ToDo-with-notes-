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

    //  Because the default Equals method accepts any type of object, when we
    //  override it, we need to use (System.Object otherTask) for the argument to
    //  match the method signature. Then, we turn the object into a Task object
    //  with Task newTask = (Task) otherTask;. When we change an object from one
    //  type to another, it's called type casting. This method will take into
    //  account if somebody tries to compare a Task with an object of another class.
    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.GetId() == newTask.GetId());
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        return (idEquality && descriptionEquality);      }
    }

    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task>{}; //  Create empty list to store all saved tasks.

      SqlConnection conn = DB.Connection(); //  Instantiates a SqlConnection object
                                            //  named "conn" and set it to the connection
                                            //  string stored as DB.Connection().

      conn.Open();  //  Opens the connection to the database.

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks;", conn);  //  Used to send SQL
                                                        //  statements to the db. Takes two
                                                        //  arguments: The command, and the
                                                        //  db connection (conn).

      SqlDataReader rdr = cmd.ExecuteReader();  //  Command is actually executed
                                                //  when use this method on "rdr".

      while(rdr.Read())
      {
        //  The while loop reads the first column of each row of the result
        //  set and saves it as taskId, then it reads the second column of each
        //  row and saves it as taskDescription, then uses these values to create
        //  a new Task object, and adds this Task to allTasks.
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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      //  WHEN PARAMETERS ARE USED IN QUERIES, THERE ARE THREE STEPS WE NEED TO TAKE:
      //  Step 1: Create the SqlCommand query with parameters.
      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description) OUTPUT INSERTED.id VALUES (@TaskDescription);", conn);

      //  Step 2: Declare a SqlParameter object and assign values.
      SqlParameter descriptionParameter = new SqlParameter(); // Create SqlParameter object for each
                                                              //  parameter that is used in the SqlCommand.

      descriptionParameter.ParameterName = "@TaskDescription";  //  Needs to match the parameter
                                                                //  in the command string.

      descriptionParameter.Value = this.GetDescription(); //  What will replace the parameter in
                                                          //  the command string when executed.

      // Step 3: Add  the SqlParameterobject to the SqlCommand object's Parameters property.
      cmd.Parameters.Add(descriptionParameter); //  If there aremore parameters
                                                //  to add, create an Add for each one.
                                                
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;
      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
