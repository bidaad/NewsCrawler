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
using ParsetCrawler.DAL;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Summary description for BaseBOLHardCode
/// </summary>
public class BaseBOLHardCode : HCYesNo, IBaseBOL<DataTable>
{
    List<AccessListStruct> AccessList;
    public BaseBOLHardCode()
    {
        dataContext = new HardCodeDataContext();
        
    }
    public string TableOrViewName = "HCYesNo";
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


    HardCodeDataContext dataContext;
    void IBaseBOL.DeleteRecord(params string[] DelParam)
    {
        string SqlCommand = string.Format("Delete from {0} where Code = {1}", TableOrViewName, DelParam[0]);
        int ResultVal = dataContext.ExecuteCommand(SqlCommand);
    }

    public int SaveChanges(bool IsNewRecord)
    {
        string SqlCommand;
        if (IsNewRecord)
            SqlCommand = string.Format("Insert into {0} (Name, Description) values (N'{1}', N'{2}')", TableOrViewName, Name.Replace("'", "''"), Description.Replace("'", "''"));
        else
            SqlCommand = string.Format("Update {0} Set Name = N'{1}', Description = N'{2}' where Code = {3}", TableOrViewName, Name.Replace("'", "''"), Description.Replace("'", "''"), Code);
        int ResultVal = dataContext.ExecuteCommand(SqlCommand);
        return ResultVal;
    }
    public DataTable GetHCDataTable(string TableName)
    {
        TableOrViewName = TableName;
        return GetDataSource(new SearchFilterCollection(), "Code", 1000, 1);
    }
    #region IBaseBOL Members
    string IBaseBOL.EditForm
    {
        get { return "Admin/HardCode/EditHardCode.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }

    string IBaseBOL.PageLable
    {
        get { return "اطلاعات پایه"; }
    }
    int IBaseBOL.EditWidth
    {
        get { return 750; }
    }

    int IBaseBOL.EditHeight
    {
        get { return 300; }
    }
    DataTable IBaseBOL.GetDataSource(SearchFilterCollection sFilterCols, string SortString, int PageSize, int CurrentPage)
    {
        return this.GetDataSource(sFilterCols, SortString, PageSize, CurrentPage);
    }

    public DataTable GetDataSource(SearchFilterCollection sFilterCols, string SortString, int PageSize, int CurrentPage)
    {
        dataContext = new HardCodeDataContext();
        string WhereCond = Tools.GetCondition(sFilterCols);
        var ListResult = dataContext.ExecuteQuery<HCYesNo>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<HCYesNo>().ToDataTable(ListResult);
        return dt;
    }
    //public int GetCount(SearchFilterCollection sFilterCols)
    //{
    //    int RecordCount = 1;
    //    DBToolsDataContext dcTools = new DBToolsDataContext();
    //    string WhereCond = Tools.GetCondition(sFilterCols);
    //    WhereCond = WhereCond.Replace("''", "'");
    //    string TableOrViewName = "News";
    //    var ResultQuery = dcTools.spGetCount(TableOrViewName, WhereCond);

    //    RecordCount = (int)ResultQuery.ReturnValue;
    //    return RecordCount;
    //}
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
        //        dataCell.DisplayMode = DisplayModes.Hidden;
        dataCell.Align = AlignTypes.Right;
        dataCell.FieldName = "Code";
        dataCell.MaxLength = 100;
        dataCell.Width = 50;
        CellCol.Add(dataCell);

        dataCell = new DataCell();
        dataCell.CaptionName = "نام";
        dataCell.Align = AlignTypes.Right;
        dataCell.IsListTitle = true;
        dataCell.FieldName = "Name";
        dataCell.MaxLength = 100;
        dataCell.Width = 200;
        CellCol.Add(dataCell);

        return CellCol;
    }
    #endregion
    private CellCollection GetCellCollection()
    {
        DataCell dataCell;
        CellCollection CellCol = new CellCollection();

        dataCell = new DataCell("Code", "کد", AlignTypes.Right, 50);
        dataCell.IsKey = true;
        dataCell.MaxLength = 100;
        CellCol.Add(dataCell);

        dataCell = new DataCell("Name", "نام", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.MaxLength = 100;
        CellCol.Add(dataCell);

        return CellCol;
    }


    #region IBaseBOL Members


    DataTable IBaseBOL<DataTable>.GetDetails(int Code)
    {
        var ListResult = dataContext.ExecuteQuery<HCYesNo>(string.Format("exec spGetDetail '{0}','{1}'", TableOrViewName, Code));
        DataTable dt = new Converter<HCYesNo>().ToDataTable(ListResult);
        return dt;
    }

    #endregion

    #region IBaseBOL Members


    int IBaseBOL.GetCount(SearchFilterCollection sFilterCols)
    {
        int RecordCount = 1;
        DBToolsDataContext dcTools = new DBToolsDataContext();
        string WhereCond = Tools.GetCondition(sFilterCols);
        WhereCond = WhereCond.Replace("''", "'");
        var ResultQuery = dcTools.spGetCount(TableOrViewName, WhereCond);

        RecordCount = (int)ResultQuery.ReturnValue;
        return RecordCount;
    }

    #endregion
}
