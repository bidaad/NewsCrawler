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
  

public class BaseBOLUserGroups : UserGroups, IBaseBOL<UserGroups>
{
    public int MasterCode;
    protected AccessLevelDataContext dataContext;
    protected string TableOrViewName="vUserGroups";
public string BaseID = "UserGroups";
    public BaseBOLUserGroups(params int[] MCodes)
    {
        MasterCode = Convert.ToInt32(MCodes[0]);
        dataContext = new AccessLevelDataContext();
        BaseID = this.ToString().Substring(3, this.ToString().Length - 3);
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


    public UserGroups GetDetails(int Code)
    {
        return dataContext.UserGroups.Single(p => p.Code.Equals(Code));
    }
    public int SaveChanges(bool IsNewRecord)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        UserGroups ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new UserGroups();
            dataContext.UserGroups.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.UserGroups.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
            #region Save Detail Controls
            PropertyInfo piMasterCode = ObjTable.GetType().GetProperty("UserCode");
            piMasterCode.SetValue(ObjTable, MasterCode, new object[] { });

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

	    if (tools.HasAccess("Edit", "UserGroups"))
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
        get { return "AccessLevel/EditUserGroups.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }
    string IBaseBOL.PageLable
    {
        get { return "گروههای کاربران"; }
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
            WhereCond = " UserCode = " + MasterCode;
        else
            WhereCond = " UserCode = " + MasterCode + " and " + WhereCond;
        
        var ListResult = dataContext.ExecuteQuery<vUserGroups>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vUserGroups>().ToDataTable(ListResult);
        return dt;
    }
    public int GetCount(SearchFilterCollection sFilterCols)
    {
        string WhereCond;
        int RecordCount = 1;
        DBToolsDataContext dcTools = new DBToolsDataContext();
        WhereCond = Tools.GetCondition(sFilterCols);
        if (WhereCond == "")
            WhereCond = " UserCode = " + MasterCode;
        else
            WhereCond = " UserCode = " + MasterCode + " and " + WhereCond;
        
        WhereCond = WhereCond.Replace("''", "'");
        var ResultQuery = dcTools.spGetCount(TableOrViewName, WhereCond);
        
        RecordCount = (int)ResultQuery.ReturnValue;
        return RecordCount;
    }

    void IBaseBOL.DeleteRecord(params string[] DelParam)
    {
        Tools tools = new Tools();
	tools.AccessList = tools.GetAccessList(BaseID);

        tools.AccessList = tools.GetAccessList(BaseID);
        if (tools.HasAccess("Edit", "UserGroups"))
        {
			UserGroups ObjTable = dataContext.UserGroups.Single(p => p.Code.Equals(DelParam[0]));
			dataContext.UserGroups.DeleteOnSubmit(ObjTable);
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

        dataCell = new DataCell("UserCode", "کد پدر", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode=DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("GroupCode", "کد گروه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("GroupName", "نام گروه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

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

        dataCell = new DataCell("UserCode", "کد پدر", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode=DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("GroupCode", "کد گروه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("GroupName", "نام گروه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
                
  
        return CellCol;
    }
}
