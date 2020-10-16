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
using System.Collections.Generic;
using System.Reflection;
using ParsetCrawler.DAL;
using DBToolsDataContext = ParsetCrawler.DAL.DBToolsDataContext;

public class BaseBOLNews : News, IBaseBOL<News>
{
    protected NewsDataContext dataContext;
    protected string TableOrViewName = "vNews";
    public string BaseID = "News";
    public BaseBOLNews()
    {
        dataContext = new NewsDataContext();
    }

    string IBaseBOL.QueryObjName
    {
        get
        {
            return TableOrViewName;
        }
        set
        {
            TableOrViewName = value;
        }
    }
    public List<WebControl> ObjectList;

    public News GetDetails(int Code)
    {
        return dataContext.News.Single(p => p.Code.Equals(Code));
    }

    //News IBaseBOL<News>.GetDetails(int Code)
    //{
    //    return dataContext.News.Single(p => p.Code.Equals(Code));
    //}
    public virtual int SaveChanges(bool IsNewRecord)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        News ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new News();
            dataContext.News.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.News.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
            #region Save Controls

            string BaseID = this.ToString().Substring(3, this.ToString().Length - 3);
            Tools tools = new Tools();
            tools.AccessList = tools.GetAccessList(BaseID);
            foreach (WebControl wc in ObjectList)
            {
                string Property = wc.ID.Substring(3, wc.ID.Length - 3);
                PropertyInfo pi = ObjTable.GetType().GetProperty(Property);
                string FullPropName = BaseID + "." + Property;
                if (tools.HasAccess("Edit", BaseID + "." + Property))
                    pi.SetValue(ObjTable, Tools.GetControlValue(wc), new object[] { });
            }
            #endregion

            dataContext.SubmitChanges();
        }
        catch (Exception exp)
        {
            throw exp;
        }

        return ObjTable.Code;
    }
    #region IBaseBOL Members
    string IBaseBOL.EditForm
    {
        get { return "News/EditNews.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return "News/ViewNews.aspx"; }
    }
    string IBaseBOL.PageLable
    {
        get { return "اخبار و اطلاع رسانی"; }
    }
    int IBaseBOL.EditWidth
    {
        get { return 750; }
    }
    int IBaseBOL.EditHeight
    {
        get { return 600; }
    }
    DataTable IBaseBOL.GetDataSource(SearchFilterCollection sFilterCols, string SortString, int PageSize, int CurrentPage)
    {
        string WhereCond = Tools.GetCondition(sFilterCols, "HCLevelCode <= " + HttpContext.Current.Session["HCLevelCode"]);
        var ListResult = dataContext.ExecuteQuery<vNews>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vNews>().ToDataTable(ListResult);
        return dt;
    }
    int IBaseBOL.GetCount(SearchFilterCollection sFilterCols)
    {
        int RecordCount = 1;
        string WhereCond = Tools.GetCondition(sFilterCols, "HCLevelCode <= " + HttpContext.Current.Session["HCLevelCode"]).Replace("''", "'");
        var ResultQuery = new DBToolsDataContext().spGetCount(TableOrViewName, WhereCond);

        RecordCount = (int)ResultQuery.ReturnValue;
        return RecordCount;
    }

    void IBaseBOL.DeleteRecord(params string[] DelParam)
    {
        Tools tools = new Tools();
        tools.AccessList = tools.GetAccessList(BaseID);

        if (tools.HasAccess("Edit", "News"))
        {
            int NewsCode = Convert.ToInt32(DelParam[0]);


            News ObjTable = dataContext.News.Single(p => p.Code.Equals(NewsCode));
            dataContext.News.DeleteOnSubmit(ObjTable);
            dataContext.SubmitChanges();
        }
    }

    CellCollection IBaseBOL.GetCellCollection()
    {
        return GetCellCollection();
    }
    CellCollection IBaseBOL.GetListCellCollection()
    {
        DataCell dataCell;
        CellCollection CellCol = new CellCollection();

        dataCell = new DataCell();
        dataCell.CaptionName = "کد";
        dataCell.IsKey = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        dataCell.Align = AlignTypes.Right;
        dataCell.FieldName = "Code";
        dataCell.MaxLength = 100;
        dataCell.Width = 50;
        CellCol.Add(dataCell);

        #region Data Cells
        dataCell = new DataCell("Title", "عنوان", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);
        dataCell = new DataCell("NewsDate", "تاریخ", AlignTypes.Right, 50);
        dataCell.IsListTitle = false;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);


        #endregion


        return CellCol;
    }
    #endregion
    protected CellCollection GetCellCollection()
    {
        DataCell dataCell;
        CellCollection CellCol = new CellCollection();

        dataCell = new DataCell();
        dataCell.CaptionName = "کد";
        dataCell.IsKey = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        dataCell.Align = AlignTypes.Right;
        dataCell.Editable = false;
        dataCell.FieldName = "Code";
        dataCell.Width = 50;
        CellCol.Add(dataCell);

        #region Data Cells
        dataCell = new DataCell("Title", "عنوان", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);
        dataCell = new DataCell("NewsDate", "تاریخ", AlignTypes.Right, 50);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);
        dataCell = new DataCell("CatName", "گروه", AlignTypes.Right, 50);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Priority", "اولویت", AlignTypes.Right, 50);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.HiddenDefault;
        CellCol.Add(dataCell);
        dataCell = new DataCell("GatherType", "نحوه تهیه", AlignTypes.Right, 50);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.HiddenDefault;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Language", "زبان", AlignTypes.Right, 50);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.HiddenDefault;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Active", "فعال", AlignTypes.Right, 50);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.HiddenDefault;
        CellCol.Add(dataCell);
        dataCell = new DataCell("HCResourceSiteCatCode", "کد گروه سایت", AlignTypes.Right, 50);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        CellCol.Add(dataCell);


        #endregion

        return CellCol;
    }
}
