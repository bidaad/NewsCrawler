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
using System.Collections;
using System.Reflection;

/// <summary>
/// Summary description for GenericTools
/// </summary>


public class Converter<T>
where T : class
{
    public DataTable ToDataTable(IEnumerable items)
    {
        DataTable dt = new DataTable();

        Type tType = typeof(T);
        PropertyInfo[] props = tType.GetProperties();
        foreach (PropertyInfo prop in props)
        {
            Type colType = prop.PropertyType;
            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                colType = colType.GetGenericArguments()[0];
            dt.Columns.Add(new DataColumn(prop.Name, colType));
        }

        foreach (var item in items)
        {
            DataRow dr = dt.NewRow();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(item, null) != null)
                    dr[prop.Name] = prop.GetValue(item, null);
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }
    public DataColumnCollection GetColumns()
    {
        DataColumnCollection dcc = new DataTable().Columns;
        Type tType = typeof(T);
        PropertyInfo[] props = tType.GetProperties();
        foreach (PropertyInfo prop in props)
        {
            Type colType = prop.PropertyType;
            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                colType = colType.GetGenericArguments()[0];
            dcc.Add(new DataColumn(prop.Name, colType));
            
        }
        return dcc;
    }
}