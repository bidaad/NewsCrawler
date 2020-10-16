using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Linq;
using System.Collections;

/// <summary>
/// Summary description for BaseBOLClass
/// </summary>
public interface IBaseUIClass
{
    string EditForm
    {
        get;

    }

}


public interface IBaseBOL
{

    string EditForm { get; }
    string ViewForm { get; }
    string PageLable { get; }
    string QueryObjName { get; set; }
    int EditWidth { get; }
    int EditHeight { get; }

    DataTable GetDataSource(SearchFilterCollection sFilterCols, string SortString, int PageSize, int CurrentPage);
    int GetCount(SearchFilterCollection sFilterCols);

    CellCollection GetCellCollection();
    CellCollection GetListCellCollection();
    void DeleteRecord(params string[] DelParam);

}
public interface IBaseBOL<TClassName> : IBaseBOL
{
    TClassName GetDetails(int Code);
}





public interface IBaseBOLTree
{
    string EditForm { get; }
    string ViewForm { get; }
    string PageLable { get; }
    string QueryObjName { get; set; }
    int EditWidth { get; }
    int EditHeight { get; }

    int Code { get; set; }
    string Name { get; set; }
    int? MasterCode { get; set; }

    DataTable GetDataSource(SearchFilterCollection sFilterCols, string SortString, int PageSize, int CurrentPage);

    int SaveChanges(bool IsNewRecord);
    void DeleteRecord(params string[] DelParam);
    CellCollection GetListCellCollection();

}

public interface IBaseBOLTree<TClassName> : IBaseBOLTree
{
    TClassName GetDetails(int Code);
}




public class BrowseSchema
{
    private DataTable _dt;
    public DataTable DataTBL
    {
        get
        {
            return _dt;
        }
        set
        {
            _dt = value;
        }
    }
    private CellCollection _cellCollection;
    public CellCollection CellCollection
    {
        get
        {
            return _cellCollection;
        }
        set
        {
            _cellCollection = value;
        }
    }
    private int _count;
    public int Count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
        }
    }


}