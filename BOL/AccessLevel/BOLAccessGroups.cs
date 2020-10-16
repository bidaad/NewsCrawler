using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Linq;
using ParsetCrawler.DAL;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection;
using System.Collections;

/// <summary>
///AccessGroups Class
/// </summary>
/// 
public partial class BOLAccessGroups : BaseBOLAccessGroups, IBaseBOL<AccessGroups>
{
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here

        if (false)
            messages.Add("");

        return messages;
    }
}

