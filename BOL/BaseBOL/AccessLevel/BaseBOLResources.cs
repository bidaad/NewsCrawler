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
/// Summary description for BaseBOLResources
/// </summary>
public class BaseBOLResources : Resources, IBaseBOL<Resources>
{
    List<AccessListStruct> AccessList;
    public BaseBOLResources()
    {
        dataContext = new AccessLevelDataContext();
        
    }
    protected string TableOrViewName = "Resources";
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


    AccessLevelDataContext dataContext;
    void IBaseBOL.DeleteRecord(params string[] DelParam)
    {
        Resources ObjTable = dataContext.Resources.Single(p => p.Code.Equals(DelParam[0]));
        dataContext.Resources.DeleteOnSubmit(ObjTable);
        dataContext.SubmitChanges();
    }

    public int SaveChanges(bool IsNewRecord)
    {
        Resources ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new Resources();
            dataContext.Resources.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.Resources.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
            ObjTable.Name= this.Name;
            if (this.EditPath != null)
                ObjTable.EditPath = this.EditPath;
            ObjTable.Code = this.Code;
            if(this.BaseID != null)
            ObjTable.BaseID = this.BaseID;
            if(this.EngName != null)
                ObjTable.EngName = this.EngName;
            if(this.TypeCode != null)
                ObjTable.TypeCode = this.TypeCode;
            ObjTable.MasterCode = this.MasterCode;

            dataContext.SubmitChanges();
        }
        catch (Exception exp)
        {

            throw new Exception("");
        }

        return ObjTable.Code;
    }
    #region IBaseBOL Members
    string IBaseBOL.EditForm
    {
        get { return "Resources/EditResources.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }

    string IBaseBOL.PageLable
    {
        get { return "اخبار"; }
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
        dataContext = new AccessLevelDataContext();
        string WhereCond = Tools.GetCondition(sFilterCols);
        var ListResult = from v in dataContext.Resources
                         where ! v.MasterCode.HasValue //|| v.TypeCode.Equals(2) || v.TypeCode.Equals(3) || v.TypeCode.Equals(4)
                         select v;
        DataTable dt = new Converter<Resources>().ToDataTable(ListResult);
        return dt;
    }

    public int GetCount(SearchFilterCollection sFilterCols)
    {
        int RecordCount = 1;
        DBToolsDataContext dcTools = new DBToolsDataContext();
        string WhereCond = Tools.GetCondition(sFilterCols);
        WhereCond = WhereCond.Replace("''", "'");
        string TableOrViewName = "Resources";
        var ResultQuery = dcTools.spGetCount(TableOrViewName, WhereCond);
        
        RecordCount = (int)ResultQuery.ReturnValue;
        return RecordCount;
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
        dataCell.MaxLength = 150;
        dataCell.Width = 200;
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
        dataCell.MaxLength = 100;
        dataCell.Width = 50;
        CellCol.Add(dataCell);

        dataCell = new DataCell("Name", "نام", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.MaxLength = 100;
        CellCol.Add(dataCell);

        dataCell = new DataCell("EngName", "نام انگلیسی", AlignTypes.Right, 100);
        dataCell.IsListTitle = true;
        dataCell.MaxLength = 200;
        CellCol.Add(dataCell);

        dataCell = new DataCell("BaseID", "لینک", AlignTypes.Right, 100);
        dataCell.IsListTitle = true;
        dataCell.MaxLength = 150;
        CellCol.Add(dataCell);

        return CellCol;
    }

    #region IBaseBOL<Resources> Members

    Resources IBaseBOL<Resources>.GetDetails(int Code)
    {
        return dataContext.Resources.Single(p => p.Code.Equals(Code));

    }

    #endregion
}
