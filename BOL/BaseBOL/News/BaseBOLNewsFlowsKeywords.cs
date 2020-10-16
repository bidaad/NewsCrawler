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


public class BaseBOLNewsFlowsKeywords : NewsFlowsKeywords, IBaseBOL<NewsFlowsKeywords>
{
    List<AccessListStruct> AccessList; 
    public int MasterCode;
    protected NewsDataContext dataContext;
    protected string TableOrViewName="vNewsFlowsKeywords";
public string BaseID = "NewsFlowsKeywords";
    public BaseBOLNewsFlowsKeywords(params int[] MCodes)
    {
        MasterCode = Convert.ToInt32(MCodes[0]);
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


    public NewsFlowsKeywords GetDetails(int Code)
    {
        return dataContext.NewsFlowsKeywords.Single(p => p.Code.Equals(Code));
    }
    public int SaveChanges(bool IsNewRecord)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        NewsFlowsKeywords ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new NewsFlowsKeywords();
            dataContext.NewsFlowsKeywords.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.NewsFlowsKeywords.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
            #region Save Detail Controls
            PropertyInfo piMasterCode = ObjTable.GetType().GetProperty("NewsFlowCode");
            piMasterCode.SetValue(ObjTable, MasterCode, new object[] { });

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

	    if (tools.HasAccess("Edit", "NewsFlowsKeywords"))
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
        get { return "News/EditNewsFlowsKeywords.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }
    string IBaseBOL.PageLable
    {
        get { return "کلمات کلیدی جریانهای خبری"; }
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
        string WhereCond;
        WhereCond  = Tools.GetCondition(sFilterCols);
        if(WhereCond == "")
            WhereCond = " NewsFlowCode = " + MasterCode;
        else
            WhereCond = " NewsFlowCode = " + MasterCode + " and " + WhereCond;
        
        var ListResult = dataContext.ExecuteQuery<vNewsFlowsKeywords>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vNewsFlowsKeywords>().ToDataTable(ListResult);
        return dt;
    }
    public int GetCount(SearchFilterCollection sFilterCols)
    {
        string WhereCond;
        int RecordCount = 1;
        DBToolsDataContext dcTools = new DBToolsDataContext();
        WhereCond = Tools.GetCondition(sFilterCols);
        if (WhereCond == "")
            WhereCond = " NewsFlowCode = " + MasterCode;
        else
            WhereCond = " NewsFlowCode = " + MasterCode + " and " + WhereCond;
        
        WhereCond = WhereCond.Replace("''", "'");
        var ResultQuery = dcTools.spGetCount(TableOrViewName, WhereCond);
        
        RecordCount = (int)ResultQuery.ReturnValue;
        return RecordCount;
    }

    void IBaseBOL.DeleteRecord(params string[] DelParam)
    {
        Tools tools = new Tools();
	tools.AccessList = tools.GetAccessList(BaseID);

        if (tools.HasAccess("Edit", "NewsFlowsKeywords"))
        {
			NewsFlowsKeywords ObjTable = dataContext.NewsFlowsKeywords.Single(p => p.Code.Equals(DelParam[0]));
			dataContext.NewsFlowsKeywords.DeleteOnSubmit(ObjTable);
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

        dataCell = new DataCell("NewsFlowCode", "کد پدر", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode=DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Name", "کلید واژه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode=DisplayModes.Visible;
        CellCol.Add(dataCell);
        

        return CellCol;
    }
    #endregion
    private CellCollection GetCellCollection()
    {
        DataCell dataCell;
        CellCollection CellCol = new CellCollection();

        dataCell = new DataCell();
        dataCell.CaptionName = "کد";
        dataCell.IsKey = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        dataCell.Align = AlignTypes.Right;
        dataCell.FieldName = "Code";
        dataCell.Width = 50;
        CellCol.Add(dataCell);

        dataCell = new DataCell("NewsFlowCode", "کد پدر", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode=DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Name", "کلید واژه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode=DisplayModes.Visible;
        CellCol.Add(dataCell);
                
  
        return CellCol;
    }
}
