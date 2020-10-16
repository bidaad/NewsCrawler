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

public class BaseBOLEntityKeywords : EntityKeywords, IBaseBOL<EntityKeywords>
{
    protected KeywordsDataContext dataContext;
    protected string TableOrViewName="vEntityKeywords";
public string BaseID = "EntityKeywords";
    List<AccessListStruct> AccessList;
    public BaseBOLEntityKeywords()
    {
        dataContext = new KeywordsDataContext();
        
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



    EntityKeywords IBaseBOL<EntityKeywords>.GetDetails(int Code)
    {
        return dataContext.EntityKeywords.Single(p => p.Code.Equals(Code));
    }
    public int SaveChanges(bool IsNewRecord)
    {
        //dataContext = new KeywordsDataContext();
        EntityKeywords ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new EntityKeywords();
            dataContext.EntityKeywords.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.EntityKeywords.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
            #region Save changes after checking Access
            ObjTable.KeywordCode = this.KeywordCode;
            ObjTable.HCEntityTypeCode = this.HCEntityTypeCode;
            ObjTable.EntityCode = this.EntityCode;
            ObjTable.Deleted = false;

            #endregion

                //IX_EntityKeywords error. LINQ Bug ?!!
                //if(IsNewRecord)
                //    dataContext.ExecuteCommand(string.Format("Insert into EntityKeywords (KeywordCode, HCEntityTypeCode, EntityCode) values ('{0}','{1}','{2}')", KeywordCode, HCEntityTypeCode, EntityCode));
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
        get { return "Admin/EntityKeywords/EditEntityKeywords.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }
    string IBaseBOL.PageLable
    {
        get { return "کلمات کلیدی موجودیتها"; }
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
        var ListResult = dataContext.ExecuteQuery<vEntityKeywords>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vEntityKeywords>().ToDataTable(ListResult);
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

        if (tools.HasAccess("Edit", "EntityKeywords"))
        {
            EntityKeywords ObjTable = dataContext.EntityKeywords.Single(p => p.Code.Equals(DelParam[0]));
            ObjTable.Deleted = true;
            //dataContext.EntityKeywords.DeleteOnSubmit(ObjTable);
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

        dataCell = new DataCell("Keyword", "کد کلید واژه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Name", "کد موجودیت", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
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

        dataCell = new DataCell("Keyword", "کد کلید واژه", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Name", "کد موجودیت", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        dataCell.DisplayMode = DisplayModes.Visible;
        CellCol.Add(dataCell);


        return CellCol;
    }
}
