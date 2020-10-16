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
using ParsetCrawler.DAL;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

/// <summary>
/// Summary description for BOLHardCode
/// </summary>
/// 
public class BOLHardCode : BaseBOLHardCode, IBaseBOL<DataTable>
{
    public int GetCodeByName(string Name)
    {
        int Result = 0;
        SearchFilterCollection sfcol = new SearchFilterCollection();
        SearchFilter sf = new SearchFilter("Name", SqlOperators.Equal, Name);
        sfcol.Add(sf);

        DataTable dt = GetDataSource(sfcol, "Code", 10, 1);
        if (dt.Rows.Count > 0)
        {
            Result = Convert.ToInt32(dt.Rows[0]["Code"]);
        }
        return Result;
    }
    
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here

        if (false)
            messages.Add("");

        return messages;
    }


    public DataTable GetList(string TableOrViewName)
    {
        HardCodeDataContext dataContext = new HardCodeDataContext();
        var ListResult = dataContext.ExecuteQuery<HCYesNo>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, "Code", 1000, 1, ""));
        DataTable dt = new Converter<HCYesNo>().ToDataTable(ListResult);
        return dt;
    }
}
