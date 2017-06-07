using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Builder;
using Nancy;
using Nancy.Owin;
using Nancy.ViewEngines.Razor;

namespace ToDoList
{
  public class Startup
  {
    public void Configure(IApplicationBuilder app)
    {
      app.UseOwin(x => x.UseNancy());
    }
  }
  public class CustomRootPathProvider : IRootPathProvider
  {
    public string GetRootPath()
    {
      return Directory.GetCurrentDirectory();
    }
  }
  public class RazorConfig : IRazorConfiguration
  {
    public IEnumerable<string> GetAssemblyNames()
    {
      return null;
    }

    public IEnumerable<string> GetDefaultNamespaces()
    {
      return null;
    }

    public bool AutoIncludeModelNamespace
    {
      get { return false; }
    }
  }
  //  Method that tells  application where to find the database.
  public static class DBConfiguration
  {
    //  The three parts of this connection string are:
    //  1. DATA SOURCE: Identified the server.
    //  2. INITAL CATALOG: The database name
    //  3. INTEGRATED SECURITY: Sets the security of the database access to the
    //  Windows user that is currently logged in.
      public static string ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo;Integrated Security=SSPI;";
  }
}
