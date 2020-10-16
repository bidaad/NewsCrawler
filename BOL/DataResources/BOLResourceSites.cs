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
using System.Collections;

public class BOLResourceSites : BaseBOLResourceSites, IBaseBOL<ResourceSites>
{
    public IQueryable<ResourceSites> GetActiveSites()
    {
        ResourceSitesDataContext dc = new ResourceSitesDataContext();
        IQueryable<ResourceSites> Result = from s in dc.ResourceSites
                     where s.Active.Equals(true)
                     select s;

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

    public object GetRandResources()
    {
        ResourceSitesDataContext dc = new ResourceSitesDataContext();
        return dc.vRandDataResources;
    }


    public object GetList()
    {
        ResourceSitesDataContext dc = new ResourceSitesDataContext();
        return dc.vResourceSites;
    }
}