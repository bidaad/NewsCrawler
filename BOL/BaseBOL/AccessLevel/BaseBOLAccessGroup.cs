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
using System.Reflection;
using System.Collections;
/// <summary>
/// Summary description for BaseBOLAccessGroup
/// </summary>
public class BaseBOLAccessGroups : AccessGroups, IBaseBOL<AccessGroups>
{
    List<AccessListStruct> AccessList;
    public BaseBOLAccessGroups()
    {
        dataContext = new AccessLevelDataContext();
        
    }
    protected string TableOrViewName="vAccessGroups";
public string BaseID = "AccessGroups";
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
        AccessGroups ObjTable = dataContext.AccessGroups.Single(p => p.Code.Equals(DelParam[0]));
        dataContext.AccessGroups.DeleteOnSubmit(ObjTable);
        dataContext.SubmitChanges();
    }
    virtual public int SaveChanges(bool IsNewRecord)
    {
        AccessGroups ObjTable;
        if (IsNewRecord)
        {
            ObjTable = new AccessGroups();
            dataContext.AccessGroups.InsertOnSubmit(ObjTable);
        }
        else
        {
            ObjTable = dataContext.AccessGroups.Single(p => p.Code.Equals(this.Code));
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

            if (tools.HasAccess("Edit", "AccessGroups")) 
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
        get { return "AccessLevel/EditAccessGroup.aspx"; }
    }
    string IBaseBOL.ViewForm
    {
        get { return ""; }
    }

    string IBaseBOL.PageLable
    {
        get { return "سطوح دسترسی"; }
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
        string TableOrViewName = "vAccessGroups";
        //var ListResult = dataContext.ExecuteQuery<AccessGroups>(string.Format("exec spGetList '{0}','{1}'", TableOrViewName, WhereCond));
        var ListResult = dataContext.ExecuteQuery<vAccessGroups>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vAccessGroups>().ToDataTable(ListResult);
        return dt;
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

        dataCell = new DataCell("LevelName", "طبقه بندی", AlignTypes.Right, 200);
        dataCell.IsListTitle = true;
        CellCol.Add(dataCell);

        return CellCol;
    }


    #region IBaseBOL<AccessGroups> Members

    AccessGroups IBaseBOL<AccessGroups>.GetDetails(int Code)
    {
        return dataContext.AccessGroups.Single(p => p.Code.Equals(Code));
    }

    #endregion


    #region IBaseBOL Members


    int IBaseBOL.GetCount(SearchFilterCollection sFilterCols)
    {
        //int RecordCount = 1;
        //DBToolsDataContext dcTools = new DBToolsDataContext();
        //string WhereCond = Tools.GetCondition(sFilterCols);
        //WhereCond = WhereCond.Replace("''", "'");
        //string TableOrViewName = "AccessGroups";
        //var ResultQuery = dcTools.spGetCount(TableOrViewName, WhereCond);
        //RecordCount = (int)ResultQuery.ReturnValue;
        //return RecordCount;

        int RecordCount = 1;
        string WhereCond = Tools.GetCondition(sFilterCols).Replace("''", "'");
        var ResultQuery = new DBToolsDataContext().spGetCount(TableOrViewName, WhereCond);

        RecordCount = (int)ResultQuery.ReturnValue;
        return RecordCount;

    }

    #endregion
}
