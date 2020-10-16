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

/// <summary>
/// Summary description for AccessListStruct
/// </summary>
public struct AccessListStruct
{
    public AccessListStruct(string _AccessName, string _FieldName)
    {
        AccessName = _AccessName;
        FieldName = _FieldName;
    }
    public string AccessName;
    public string FieldName;
}
