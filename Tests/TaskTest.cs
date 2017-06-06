using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    { //  This tells the application where to find the test database.
      //  This overrides "DBConfiguration.ConnectionString" in Startup.cs.
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }
// Tests that there is an empty db to start testing.
    [Fact]
       public void Test_DatabaseEmptyAtFirst()
       {
         //Arrange, Act
         int result = Task.GetAll().Count;

         //Assert
         Assert.Equal(0, result);
       }

    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
