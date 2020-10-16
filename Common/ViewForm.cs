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

/// <summary>
/// Summary description for ViewForm
/// </summary>
public class ViewForm<T> : System.Web.UI.Page
where T : class
{
    protected IBaseBOL<T> BOLClass;
    public string BaseID;
    public ViewForm()
    {
        string t = typeof(T).ToString();
        BaseID = t.Substring(t.LastIndexOf(".") + 1);
    }
    public int SelectedTabIndex = 0;


    #region Properties

    protected string Keyword
    {
        get
        {
            try
            {
                if (ViewState["_Keyword"] == null)
                {
                    ViewState["_Keyword"] = "";
                    return "";
                }
                else
                    return (string)ViewState["_Keyword"].ToString();
            }
            catch
            {
                return "";
            }
        }
        set
        {
            ViewState["_Keyword"] = value;
        }
    }
    protected string ConditionCode
    {
        get
        {
            try
            {
                if (ViewState["_ConditionCode"] == null)
                {
                    ViewState["_ConditionCode"] = "";
                    return "";
                }

                else
                    return (string)ViewState["_ConditionCode"].ToString();
            }
            catch
            {
                return "";
            }

        }
        set
        {
            ViewState["_ConditionCode"] = value;
        }
    }
    protected string FilterColumns
    {
        get
        {
            try
            {
                if (ViewState["_FilterColumns"] == null)
                {
                    ViewState["_FilterColumns"] = "";
                    return "";
                }

                else
                    return (string)ViewState["_FilterColumns"].ToString();
            }
            catch
            {
                return "";
            }

        }
        set
        {
            ViewState["_FilterColumns"] = value;
        }
    }

    protected int? Code
    {
        get
        {
            if (ViewState["_Code"] == null)
            {
                try
                {
                    Keyword = Request["Keyword"];
                    ConditionCode = Request["ConditionCode"];
                    FilterColumns = Request["FilterColumns"];

                    Tools tools = new Tools();
                    tools.CheckEditButtnAccess(Page.Master, BaseID);
                    int intCode = Int32.Parse(Request["Code"]);
                    ViewState["_Code"] = intCode;
                    return intCode;
                }
                catch
                {
                    return null;
                }
            }
            else
                return Int32.Parse(ViewState["_Code"].ToString());
        }
        set
        {
            ViewState["_Code"] = value;
        }
    }
    
    
    #endregion

    


    protected void GoToList(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("~/Main/Default.aspx?BaseID=" + BaseID + "&Keyword=" + Keyword + "&ConditionCode=" + ConditionCode + "&FilterColumns=" + FilterColumns, false);
    }
}
