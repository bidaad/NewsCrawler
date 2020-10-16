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
using System.Web.SessionState;
using ParsetCrawler.DAL;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Linq;
using System.Collections;

public class BOLGroupResources : BaseBOLGroupResources, IBaseBOL<GroupResources>
{
    public BOLGroupResources(params int[] MCodes)
        : base(MCodes)
    {
        
    }

    public GroupResources GetByGroupandResourceCode(int GroupCode, int ResourceCode)
    {
        AccessLevelDataContext dataContext = new AccessLevelDataContext();
        return dataContext.GroupResources.SingleOrDefault(p => p.GroupCode.Equals(GroupCode) && p.ResourceCode.Equals(ResourceCode) );
    }

    public void GetAccess(int AccessType, int GroupCode, int MasterCode)
    {
        int ResourceCode;
        dataContext = new AccessLevelDataContext();
        var Result = dataContext.sp_GetMultipleAccess(AccessType, GroupCode, MasterCode);
        var ListResult = dataContext.ExecuteQuery<GroupResources>(string.Format("exec sp_GetMultipleAccess '{0}','{1}','{2}'", AccessType, GroupCode, MasterCode));

        foreach (var r in ListResult)
        {
            ResourceCode = r.ResourceCode.Value;
            GetAccess(AccessType, GroupCode, ResourceCode);
        }
    }
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here

        if (false)
            messages.Add("");

        return messages;
    }

}
