using System;
using System.Web;
using System.Web.Configuration;
using System.Collections.Generic;


public class CheckPermissionHttpModule :IHttpModule
{
    #region IHttpModule Members
    public CheckPermissionHttpModule()
    {
        
    }

    public void Init(HttpApplication objApplication)
    {
        objApplication.AcquireRequestState += Acquire_RequestState;
    }
  

    protected void Acquire_RequestState(object sender, EventArgs e)
    {
        HttpApplication Sender = (HttpApplication) sender;
        string q = Sender.Context.Request.QueryString.ToString();
        if (QueryStringContainsIllegalCharacters(q))
        {
            //Sender.Context.Session["PathAndQuery"] = Sender.Context.Request.Url.AbsolutePath.ToUpperInvariant() + GetQueryString(Sender.Context.Request.QueryString);
            //Sender.Context.Response.Redirect("~/Default.aspx");
            return;
        }
      
    }


    public static bool QueryStringContainsIllegalCharacters(string q)
    {
        q = q.ToUpper();
        bool result = (
                q.Contains("SELECT ") ||
                q.Contains(" SELECT") ||
                q.Contains("%20SELECT ") ||
                q.Contains("SELECT%20") ||
                q.Contains("DELETE") ||
                q.Contains("INSERT") ||
                q.Contains("SHUTDOWN") ||
                q.Contains("EXEC") ||
                q.Contains("SCRIPT") ||
                q.Contains("UNION") ||
                q.Contains("ALTER") ||
                q.Contains("DROP") ||
                q.Contains("LIKE") ||
                q.Contains("CHR") ||
                q.Contains("0X") ||
                q.Contains("%20OR") ||
                q.Contains(" OR") ||
                q.Contains("OR%20") ||
                q.Contains("OR ") ||
                q.Contains("%20AND") ||
                q.Contains(" AND") ||
                q.Contains("AND%20") ||
                q.Contains("AND ") ||
                q.Contains("WHERE") ||
                q.Contains("GROUP BY") ||
                q.Contains("HAVING") ||
                q.Contains("LIKE") ||
                q.Contains("ALERT") ||
                q.Contains("%00")
            );
        return result;
            
    }

    public void Dispose()
    {
        // Simply do nothing!
    }
    #endregion

   
}
