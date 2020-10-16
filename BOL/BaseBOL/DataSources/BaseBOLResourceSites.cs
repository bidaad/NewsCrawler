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
  
public class BaseBOLResourceSites : ResourceSites, IBaseBOL<ResourceSites>
{
    protected ResourceSitesDataContext dataContext;
    protected string TableOrViewName="vResourceSites";
public string BaseID = "ResourceSites";
    List<AccessListStruct> AccessList;
    public BaseBOLResourceSites()
    {
        dataContext = new ResourceSitesDataContext();
        
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


    
    ResourceSites IBaseBOL<ResourceSites>.GetDetails(int Code)
    {
        return dataContext.ResourceSites.Single(p => p.Code.Equals(Code));
    }
    public int SaveChanges( bool IsNewRecord)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        ResourceSites ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new ResourceSites();
            dataContext.ResourceSites.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.ResourceSites.Single(p => p.Code.Equals(this.Code));
        }
        try
        {
           #region Save changes after checking Access 
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
        get { return "DataResources/EditResourceSites.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }

    string IBaseBOL.PageLable
    {
        get { return "سایت های منبع"; }
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
        string WhereCond = Tools.GetCondition(sFilterCols);
        var ListResult = dataContext.ExecuteQuery<vResourceSites>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vResourceSites>().ToDataTable(ListResult);
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
        ResourceSites ObjTable = dataContext.ResourceSites.Single(p => p.Code.Equals(DelParam[0]));
        dataContext.ResourceSites.DeleteOnSubmit(ObjTable);
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

        dataCell = new DataCell("Name", "نام", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("CatName", "نام گروه", AlignTypes.Right, 200);
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

        dataCell = new DataCell("Name", "نام", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("Active", "فعال", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);
        dataCell = new DataCell("LangName", "زبان", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);

                
  
        return CellCol;
    }
}
