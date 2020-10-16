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

public class BOLNewsKeywords : BaseBOLNewsKeywords, IBaseBOL<NewsKeywords>
{
    public BOLNewsKeywords(params int[] MCodes) : base(MCodes)
    {

    }
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here

        if (false)
            messages.Add("");

        return messages;
    }


    public object GetTopKeywords(int NewsCode, int? TakeCount)
    {
        KeywordsDataContext dc = new KeywordsDataContext();
        if(TakeCount == null)
            return dc.vNewsKeywords.Where(p => p.EntityCode.Equals(NewsCode));
        else
            return dc.vNewsKeywords.Where(p => p.EntityCode.Equals(NewsCode)).Take((int)TakeCount);

    }


}