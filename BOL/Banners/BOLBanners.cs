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

public class BOLBanners : BaseBOLBanners, IBaseBOL<Banners>
{
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

    public IQueryable<vBanners> GetBannersByPositionCode(string _positionCode)
    {
        BannersDataContext dataContext = new BannersDataContext();
        return from n in dataContext.vBanners
                    .Where(n => n.PositionCode.Equals(_positionCode))
                    .OrderByDescending(n => n.Code)
               select n;
    }
}
