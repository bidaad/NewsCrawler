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
/// Summary description for BOLUsers
/// </summary>
public partial class BOLUsers : BaseBOLUsers, IBaseBOL<Users>
{
    public Users GetDataByUsername(string Username)
    {
        Users ValidUser = dataContext.Users.SingleOrDefault(p => p.Username.Equals(Username));
        return ValidUser;
    }

    public System.Linq.IQueryable<vUserAccess> GetUserAccess(int Code)
    {
        var AccList = from u in dataContext.vUserAccesses
                      where u.UserCode.Equals(Code)
                      select u;
        return AccList;
    }


    public System.Linq.IQueryable<vUserAccess> GetUserAccessByBaseID(int Code,string BaseID)
    {
        if(BaseID != null)
        return from u in dataContext.vUserAccesses
                      where u.UserCode.Equals(Code) && u.ResName.StartsWith(BaseID)
                      select u;
        else
            return from u in dataContext.vUserAccesses
                   where u.UserCode.Equals(Code) && (u.TypeCode.Equals(1) || u.TypeCode.Equals(2))
                   orderby u.Ordering
                   select u;

    }

    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here

        if (false)
            messages.Add("");

        return messages;
    }


    public void SaveDockState(int UserCode, string CurDockState)
    {
        Users ObjTable;
        ObjTable = dataContext.Users.Single(p => p.Code.Equals(UserCode));
        ObjTable.DockState = CurDockState;
        dataContext.SubmitChanges();
    }
}
