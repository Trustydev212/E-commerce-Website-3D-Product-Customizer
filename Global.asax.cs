using System;
using System.Web;
// using Source.DAL; // Đã bỏ namespace, không cần using này nữa
public partial class Global : System.Web.HttpApplication
{
    protected void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
    }

    protected void Application_End(object sender, EventArgs e)
    {
        // Code that runs on application shutdown
    }

    protected void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        if (ex != null)
        {
            try
            {
                // Log error to database
                // AdminLogDAL logDAL = new AdminLogDAL();
                // logDAL.LogError("Application_Error", ex.Message, ex.StackTrace);
            }
            catch
            {
                // If logging fails, don't throw another exception
            }
        }
    }

    protected void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    protected void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends
    }
}