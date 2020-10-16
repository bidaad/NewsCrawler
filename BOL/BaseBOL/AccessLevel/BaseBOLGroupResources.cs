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
  
public class BaseBOLGroupResources : GroupResources, IBaseBOL<GroupResources>
{
    public int MasterCode;
    List<AccessListStruct> AccessList;
    public BaseBOLGroupResources(params int[] MCodes)
    {
        MasterCode = Convert.ToInt32(MCodes[0]);
        dataContext = new AccessLevelDataContext();
        
    }
    protected AccessLevelDataContext dataContext;
    protected string TableOrViewName="vGroupResources";
public string BaseID = "GroupResources";


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


    
    GroupResources IBaseBOL<GroupResources>.GetDetails(int Code)
    {
        return dataContext.GroupResources.Single(p => p.Code.Equals(Code));
    }
    public int SaveChanges( bool IsNewRecord)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        GroupResources ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new GroupResources();
            dataContext.GroupResources.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.GroupResources.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
            #region Save Detail Controls
            PropertyInfo piMasterCode = ObjTable.GetType().GetProperty("GroupCode");
            piMasterCode.SetValue(ObjTable, MasterCode, new object[] { });

            string BaseID = this.ToString().Substring(3, this.ToString().Length - 3);
            Tools tools = new Tools();
            tools.AccessList = tools.GetAccessList(BaseID);
            ObjTable.GroupCode = this.GroupCode;
            ObjTable.ResourceCode = this.ResourceCode;
            ObjTable.AccessType = this.AccessType;


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
        get { return "GroupResources/EditGroupResources.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }

    string IBaseBOL.PageLable
    {
        get { return "منابع گروه های دسترسی"; }
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
        string WhereCond = Tools.GetCondition(sFilterCols);
        var ListResult = dataContext.ExecuteQuery<vGroupResources>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vGroupResources>().ToDataTable(ListResult);
        return dt;
    }
    int IBaseBOL.GetCount(SearchFilterCollection sFilterCols)
    {
        int RecordCount = 1;
        string WhereCond = Tools.GetCondition(sFilterCols).Replace("''", "'");
        var ResultQuery = new DBToolsDataContext().spGetCount(TableOrViewName, WhereCond);
        
        RecordCount = (int)ResultQuery.ReturnValue;
        return RecordCount;
    }

    void IBaseBOL.DeleteRecord(params string[] DelParam)
    {
        Tools tools = new Tools();
	tools.AccessList = tools.GetAccessList(BaseID);

        if (tools.HasAccess("Edit", "GroupResources"))
        {
			GroupResources ObjTable = dataContext.GroupResources.Single(p => p.Code.Equals(DelParam[0]));
			dataContext.GroupResources.DeleteOnSubmit(ObjTable);
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

        dataCell = new DataCell("Code", "کد", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("GroupCode", "کد پدر", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("ResourceName", "نام منبع", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("ResourceCode", "کد منبع", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("AccessType", "نوع دسترسی", AlignTypes.Right, 200);
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

        dataCell = new DataCell("Code", "کد", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode= DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("GroupCode", "کد پدر", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("ResourceName", "نام منبع", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("ResourceCode", "کد منبع", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
        dataCell = new DataCell("AccessType", "نوع دسترسی", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;

        CellCol.Add(dataCell);
                
  
        return CellCol;
    }
}
