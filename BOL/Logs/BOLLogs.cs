using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using ParsetCrawler.DAL;
  
public class BOLLogs 
{
    public int? InsertLog(string  UserAgent,string  QueryString, string  UserInfo, string UserHostAddress, string AbsolutePath, DateTime LogDate, string Http_referer, ref int? Result)
    {
        LogsDataContext dataContext = new LogsDataContext();
        dataContext.spInsertLog(UserAgent, QueryString, UserInfo, UserHostAddress, AbsolutePath, LogDate, Http_referer,ref Result);
        return Result;
    }

    public string GetLogCountByDate(DateTime PassDate)
    {
        LogsDataContext dataContext = new LogsDataContext();
        return dataContext.Logs.Where(p => p.LogDateTime.Value.Date.Equals(PassDate.Date)).Count().ToString();
    }

    public string GetTotalCount()
    {
        LogsDataContext dataContext = new LogsDataContext();
        return  dataContext.Logs.Count().ToString();
    }
}
