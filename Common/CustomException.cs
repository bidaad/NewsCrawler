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
using System.Data.SqlClient;

/// <summary>
/// Summary description for CustomException
/// </summary>
public class CustomException : Exception
{
    
    public string ShowPersianError(Exception exp)
    {
            string ErrorMessage = "";
            switch (exp.GetType().FullName)
            {
                case "System.Data.SqlClient.SqlException":
                    switch (((SqlException)exp).Number)
                    {
                        case -2146232060:
                            ErrorMessage = "بدلیل استفاده شدن رکورد مورد نظر در سیستم قادر به حذف نمی باشید.";
                            break;
                        case 8152:
                            ErrorMessage = "رشته ورودی طولانی‌تر از حد مجاز است.";
                            break;
                        default:
                            break;
                    }
                    break;
                case "System.NotDivideByZero":
                    ErrorMessage = "تقسیم بر صفر امکانپذیر نیست.";
                    break;
                case "System.FormatException":
                    break;
                default:
                    break;
            }
            return ErrorMessage;
        
    }

}
