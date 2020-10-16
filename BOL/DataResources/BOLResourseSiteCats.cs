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

public class BOLResourseSiteCats : BaseBOLResourseSiteCats, IBaseBOL<ResourseSiteCats>
{
    public BOLResourseSiteCats(params int[] MCodes)
        : base(MCodes)
    {

    }
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();

        #region Business Rules
        //Example
        //if (string.IsNullOrEmpty(this.FirstName))
        //    messages.Add("Please fill FirstName.");

        #endregion
        return messages;
    }

    public vResourseSiteCats GetSingleSite(int SiteCatCode)
    {
        ResourceSitesDataContext dc = new ResourceSitesDataContext();
        return dc.vResourseSiteCats.SingleOrDefault(p => p.Code.Equals(SiteCatCode));
    }

    public IQueryable<vResourseSiteCats> GetActiveSites()
    {
        ResourceSitesDataContext dc = new ResourceSitesDataContext();
        return dc.vResourseSiteCats.Where(p => p.ActiveCat.Equals(true));

    }
}
