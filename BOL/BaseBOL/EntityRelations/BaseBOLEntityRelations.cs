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

/// <summary>
/// Summary description for BaseBOLEntityRelations
/// </summary>
public class BaseBOLEntityRelations : EntityRelations, IBaseBOL<EntityRelations>
{
    List<AccessListStruct> AccessList;
    public BaseBOLEntityRelations()
    {
        dataContext = new EntityRelationsDataContext();
        
    }
    protected string TableOrViewName = "vEntityRelations";
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

    EntityRelationsDataContext dataContext;
    public int SaveChanges(bool IsNewRecord)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        EntityRelations ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new EntityRelations();
            dataContext.EntityRelations.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.EntityRelations.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
            #region Save Controls
            string BaseID = this.ToString().Substring(3, this.ToString().Length - 3);
            Tools tools = new Tools();
            tools.AccessList = tools.GetAccessList(BaseID);
            ObjTable.EntityCode = this.EntityCode;
            ObjTable.HCEntityTypeCode = this.HCEntityTypeCode;
            ObjTable.RelatedEntityCode = this.RelatedEntityCode;
            ObjTable.HCRelatedEntityTypeCode = this.HCRelatedEntityTypeCode;

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
        get { return "Admin/EntityRelations/EditEntityRelations.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return "Admin/EntityRelations/ViewEntityRelations.aspx"; }
    }

    string IBaseBOL.PageLable
    {
        get { return "ارتباطات موجودیتها"; }
    }
    int IBaseBOL.EditWidth
    {
        get { return 950; }
    }

    int IBaseBOL.EditHeight
    {
        get { return 700; }
    }
    DataTable IBaseBOL.GetDataSource(SearchFilterCollection sFilterCols, string SortString, int PageSize, int CurrentPage)
    {
        string WhereCond = Tools.GetCondition(sFilterCols);
        var ListResult = dataContext.ExecuteQuery<vEntityRelations>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vEntityRelations>().ToDataTable(ListResult);
        return dt;
    }


    void IBaseBOL.DeleteRecord(params string[] DelParam)
    {
        EntityRelations ObjTable = dataContext.EntityRelations.Single(p => p.Code.Equals(DelParam[0]));
        dataContext.EntityRelations.DeleteOnSubmit(ObjTable);
        dataContext.SubmitChanges();
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

        dataCell = new DataCell("FirstName", "نام", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("LastName", "نام خانوادگی", AlignTypes.Right, 200);
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

        dataCell = new DataCell("FirstName", "نام", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("LastName", "نام خانوادگی", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Country", "کشور", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("CharacterTypes", "نوع شخصیت", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("GroupCode", "کد گروه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("HCCharacterTypeCode", "کد شخصیت", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("CountryCode", "کد کشور", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        CellCol.Add(dataCell);
        dataCell = new DataCell("ZoneCode", "کد منطقه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Hidden;
        CellCol.Add(dataCell);


        return CellCol;
    }

    #region IBaseBOL Members


    EntityRelations IBaseBOL<EntityRelations>.GetDetails(int Code)
    {
        return dataContext.EntityRelations.Single(p => p.Code.Equals(Code));
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