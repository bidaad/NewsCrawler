using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Text;

/// <summary>
/// Summary description for DateTimeMethods
/// </summary>
public class DateTimeMethods
{
    public static string TimeDiff = ConfigurationManager.AppSettings["TimeDiff"];
    public static string[] TimeDiffArray = TimeDiff.Split(':');
    public static int HourDiff = Convert.ToInt32(TimeDiffArray[0]);
    public static int MinDiff = Convert.ToInt32(TimeDiffArray[1]);

    #region Exception Messages
    private string _IncorrectYear = "سال معتبر نیست.";
    private string _IncorrectMonth = "ماه معتبر نیست.";
    private string _IncorrectDay = "روز معتبر نیست.";

    #endregion
    public DateTimeMethods()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    PersianCalendar pd = new PersianCalendar();
    /// <summary>
    /// تاریخ امروز شمسی
    /// مثلاً: 1385/1/1
    /// </summary>
    /// <returns></returns>
    public string PersianToday
    {
        get
        {
            return GetPersianDate(DateTime.Now);
        }
    }

    /// <summary>
    /// تبدیل یک تاریخ میلادی به تاریخ شمسی
    /// </summary>
    /// <param name="ChristianDate"></param>
    /// <returns></returns>
    public string GetPersianDate(DateTime ChristianDate)
    {
        string date;
        date = pd.GetYear(ChristianDate) + "/" + pd.GetMonth(ChristianDate) + "/" + pd.GetDayOfMonth(ChristianDate);
        return date;
    }
    /// <summary>
    /// به دست آوردن نام یک روز
    /// </summary>
    /// <param name="ChristianDate">تاریخ روز به صورت میلادی</param>
    /// <param name="Persian">true==> نام روز را به صورت فارسی برمی‌گرداند
    /// false==> نام روز را به صورت میلادی برمی‌گرداند</param>
    /// <returns></returns>
    public string GetDayOfWeek(DateTime ChristianDate, bool Persian)
    {
        DateTime dt = new DateTime();
        dt = ChristianDate;
        string result = pd.GetDayOfWeek(dt).ToString();
        if (Persian)
        {
            switch (result)
            {
                case "Saturday":
                    result = "شنبه";
                    break;
                case "Sunday":
                    result = "یکشنبه";
                    break;
                case "Monday":
                    result = "دوشنبه";
                    break;
                case "Tuesday":
                    result = "سه‌شنبه";
                    break;
                case "Wednesday":
                    result = "چهارشنبه";
                    break;
                case "Thursday":
                    result = "پنج‌شنبه";
                    break;
                case "Friday":
                    result = "جمعه";
                    break;
            }
        }
        return result;
    }
    /// <summary>
    /// شماره ماه جاری شمسی
    /// </summary>
    /// <returns></returns>
    public int PersianCurrentMonthNumber
    {
        get
        {
            return GetPersianMonthNumber(DateTime.Now);
        }
    }
    /// <summary>
    /// شماره سال جاری شمسی
    /// </summary>
    /// <returns></returns>
    public int PersianCurrentYear
    {
        get
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            return pd.GetYear(dt);
        }
    }
    /// <summary>
    /// یک تاریخ خاص میلادی را شمسی کرده و به این صورت برمی‌گرداند:
    /// شنبه، 1 فروردین 1385
    /// </summary>
    /// <returns></returns>
    public string GetPersianLongDate(DateTime ChristianDate)
    {
        string DayName;
        string MonthName;
        string result;

        DayName = GetDayOfWeek(ChristianDate, true);
        MonthName = GetPersianMonthName(ChristianDate);
        result = DayName + "،" + pd.GetDayOfMonth(ChristianDate) + " " + GetPersianMonthName(ChristianDate) + " " + pd.GetYear(ChristianDate);
        return result;

    }
    /// <summary>
    /// شماره ماه شمسی معادل با یک تاریخ میلادی را برمی‌گرداند
    /// مثلاً: 1
    /// </summary>
    /// <param name="ChristianDate"></param>
    /// <returns></returns>
    public int GetPersianMonthNumber(DateTime ChristianDate)
    {
        return pd.GetMonth(ChristianDate);
    }
    /// <summary>
    /// نام ماه جاری را برمی‌گرداند.
    /// مثلاً: فروردین
    /// </summary>
    /// <returns></returns>
    public string PersianCurrentMonthName
    {
        get
        {
            return GetPersianMonthName(DateTime.Now);
        }
    }
    /// <summary>
    /// نام شمسی ماه معادل با یک تاریخ میلادی را برمی‌گرداند
    /// مثلاً: فروردین
    /// </summary>
    /// <param name="ChristianDate"></param>
    /// <returns></returns>
    public string GetPersianMonthName(DateTime ChristianDate)
    {
        int MonthNumber = pd.GetMonth(ChristianDate);
        string MonthName;
        switch (MonthNumber)
        {
            case 1:
                MonthName = "فروردین";
                break;
            case 2:
                MonthName = "اردیبهشت";
                break;
            case 3:
                MonthName = "خرداد";
                break;
            case 4:
                MonthName = "تیر";
                break;
            case 5:
                MonthName = "مرداد";
                break;
            case 6:
                MonthName = "شهریور";
                break;
            case 7:
                MonthName = "مهر";
                break;
            case 8:
                MonthName = "آبان";
                break;
            case 9:
                MonthName = "آذر";
                break;
            case 10:
                MonthName = "دی";
                break;
            case 11:
                MonthName = "بهمن";
                break;
            case 12:
                MonthName = "اسفند";
                break;
            default:
                MonthName = "نامشخص";
                break;
        }
        return MonthName;
    }
    /// <summary>
    /// تاریخ شمسی را به این صورت می‌گیرد:
    /// روز/ماه/سال -مثلاً 5/3/1385
    /// نتیجه خروجی تاریخ اصلاح شده است و در صورتی که نتوان تاریخ را اصلاح کرد خطا روی می‌دهد.
    /// </summary>
    /// <param name="PersianDate"></param>
    /// <returns></returns>
    public string FormatPersianDate(string PersianDate)
    {
        string[] PersianSepereatedDate;
        PersianDate = PersianDate.Replace("-", "/");
        PersianDate = PersianDate.Replace("\\", "/");
        if (PersianDate.IndexOf("/") > 0)
        {
            PersianSepereatedDate = PersianDate.Split('/');
        }
        else
        {
            throw new Exception("روز، ماه و سال باید با کاراکتر '/' از هم جدا شوند");
        }

        int year;
        int month;
        int day;

        if (PersianSepereatedDate[0] != null && PersianSepereatedDate[0] != "" && PersianSepereatedDate[0] != " " && PersianSepereatedDate[0] != "  ")
            year = Convert.ToInt16(PersianSepereatedDate[0]);
        else throw new Exception(_IncorrectYear);

        if (PersianSepereatedDate[1] != null && PersianSepereatedDate[1] != "" && PersianSepereatedDate[1] != " " && PersianSepereatedDate[1] != "  ")
            month = Convert.ToInt16(PersianSepereatedDate[1]);
        else throw new Exception(_IncorrectMonth);


        if (PersianSepereatedDate[2] != null && PersianSepereatedDate[2] != "" && PersianSepereatedDate[2] != " " && PersianSepereatedDate[2] != "  ")
            day = Convert.ToInt16(PersianSepereatedDate[2]);
        else throw new Exception(_IncorrectDay);


        if (year < 100) year += 1300;

        if (year > 1450 || year < 1200)
        {
            throw new Exception(_IncorrectYear);
        }

        if (month < 1 || month > 12)
        {
            throw new Exception(_IncorrectMonth);
        }

        if (day > 31 || day < 1)
        {
            throw new Exception(_IncorrectDay);
        }

        if (month <= 12 && month > 6 && day > 30)
        {
            throw new Exception("این ماه بیش از 30 روز نمی‌تواند باشد");
        }

        StringBuilder date = new StringBuilder();
        if (year < 100) date.Append("13");
        date.Append(year);
        date.Append("/");
        if (month < 10) date.Append("0");
        date.Append(month);
        date.Append("/");
        if (day < 10) date.Append("0");
        date.Append(day);

        return date.ToString();

    }
    /// <summary>
    /// بررسی می‌کند که آیا یک تاریخ شمسی معتبر است یا خیر.
    /// </summary>
    /// <param name="PersianDate"></param>
    /// <returns></returns>
    public bool IsValidPersianDate(string PersianDate)
    {
        string[] PersianSepereatedDate;
        PersianSepereatedDate = PersianDate.Split('/');

        int year;
        if (!int.TryParse(PersianSepereatedDate[0], out year)) return false;
        int month;
        if (!int.TryParse(PersianSepereatedDate[1], out month)) return false;
        int day;
        if (!int.TryParse(PersianSepereatedDate[2], out day)) return false;

        if (year < 100) year += 1300;

        if (year > 1450 || year < 1200)
        {
            // throw new Exception( "سال معتبر نیست.");
            return false;

        }

        if (month < 1 || month > 12)
        {
            //throw new Exception( "ماه معتبر نیست.");
            return false;
        }

        if (day > 31 || day < 1)
        {
            // throw new Exception( "روز معتبر نیست.");
            return false;
        }

        if (month <= 12 && month > 6 && day > 30)
        {
            //throw new Exception( "این ماه بیش از 30 روز نمی‌تواند باشد");
            return false;
        }

        return true;

    }
    /// <summary>
    /// تاریخ را به فرمت 1390/1/2 گرفته و به فرمت 13900102 برمیگرداند
    /// </summary>
    public string GetDateWithoutSlash(string sDate)
    {
        string Result = "";
        string[] sDateArray = sDate.Split('/');
        Result = sDateArray[0];
        if (sDateArray[1].Length == 1)
            Result = Result + "0" + sDateArray[1];
        else
            Result = Result + sDateArray[1];

        if (sDateArray[2].Length == 1)
            Result = Result + "0" + sDateArray[2];
        else
            Result = Result + sDateArray[2];

        return Result;
    }
    public string FormatPersianDate(string sDate, int FormatType)
    {
        string Result = "";


        switch (FormatType)
        {
            case 1:
                if (sDate.Length == 8)
                    Result = sDate.Substring(0, 4) + "/" + sDate.Substring(4, 2) + "/" + sDate.Substring(6, 2);
                else
                    Result = sDate;
                break;
            default:
                break;
        }
        return Result;
    }
    /// <summary>
    /// بررسی می‌کند که آیا یک ساعت معتبر است یا خیر.
    /// </summary>
    /// <param name="TimeFormat">ساعت, مثلاً 3:22</param>
    /// <returns></returns>
    public bool IsValidTimeFormat(string Time)
    {
        string[] CheckTime = Time.Split(':');
        int THours = Convert.ToInt32(CheckTime[0]);
        int TMinute = Convert.ToInt32(CheckTime[1]);

        if (THours < 1 || THours > 24 || TMinute < 0 || TMinute > 60)
            return false;
        else
            return true;
    }
    public enum YearMonthDay
    {
        Year,
        Month,
        Days
    }
    /// <summary>
    /// تعداد روزهای بین دو تاریخ.
    /// </summary>
    /// <param name="FirstPersianDate">اولین تاریخ</param>
    /// <param name="SecondPersianDate">دومین تاریخ</param>
    /// <returns>دومین تاریخ منهای اولین تاریخ با احتساب کبیسه</returns>
    public int GetDaysBetweenTwoDates(string FirstPersianDate, string SecondPersianDate)
    {
        if (!IsValidPersianDate(FirstPersianDate))
            throw new Exception("فرمت تاریخ اول اشتباه است.");

        if (!IsValidPersianDate(SecondPersianDate))
            throw new Exception("فرمت تاریخ دوم اشتباه است.");

        FirstPersianDate = FormatPersianDate(FirstPersianDate);
        SecondPersianDate = FormatPersianDate(SecondPersianDate);

        PersianCalendar pdDates = new PersianCalendar();

        int FirstYear = Convert.ToInt16(FirstPersianDate.Substring(0, 4));
        int FirstMonth = Convert.ToInt16(FirstPersianDate.Substring(5, 2));
        int FirstDay = Convert.ToInt16(FirstPersianDate.Substring(8, 2));

        DateTime dtFirst = pdDates.ToDateTime(FirstYear, FirstMonth, FirstDay, 10, 0, 0, 0);

        int SecondYear = Convert.ToInt16(SecondPersianDate.Substring(0, 4));
        int SecondMonth = Convert.ToInt16(SecondPersianDate.Substring(5, 2));
        int SecondDay = Convert.ToInt16(SecondPersianDate.Substring(8, 2));

        DateTime dtSecond = pdDates.ToDateTime(SecondYear, SecondMonth, SecondDay, 10, 0, 0, 0);

        long ticks = dtSecond.Ticks - dtFirst.Ticks;
        int r = 0;
        if (ticks > 0)
        {
            DateTime dtnew = new DateTime(ticks);
            r = (dtnew.Year - 1) * 365 + (dtnew.DayOfYear - 1);
        }
        else if (ticks < 0)
        {
            ticks = (dtFirst.Ticks - dtSecond.Ticks);
            DateTime dtnew = new DateTime(ticks);
            r = ((dtnew.Year - 1) * 365 + (dtnew.DayOfYear - 1)) * -1;
        }
        else if (ticks == 0) r = 0;
        return r;
    }
    /// <summary>
    /// معتبر بودن 2 تاریخ ورودی و بزرگتر بودن دومی از اولی 
    /// </summary>
    /// <param name="First"></param>
    /// <param name="Last"></param>
    /// <returns></returns>
    public void IsValidAndCompare(string First, string Last)
    {
        bool IsValidFirst = IsValidPersianDate(First);
        bool IsValidLast = IsValidPersianDate(Last);
        if (!IsValidFirst)
            throw new Exception("ازتاریخ را صحیح وارد کنید");

        if (!IsValidLast)
            throw new Exception("تا تاریخ را صحیح وارد کنید");

        if (string.Compare(First, Last) > 0)
            throw new Exception("تاریخ نهایی نمی‌تواند کوچکتر باشد.");
    }
    /// <summary>
    /// Formats the date.
    /// </summary>
    /// <param name="unfDate">date</param>
    /// <returns>formatted date.</returns>
    public string FormatShamsiDate(string unfDate)
    {
        string Result = "";
        if (unfDate != null)
        {
            if (unfDate.Length == 8)
                Result = unfDate.Substring(0, 4) + "/" + unfDate.Substring(5, 2) + "/" + unfDate.Substring(unfDate.Length - 2, 2);
            else
                Result = "";
        }
        else
            Result = "";
        return Result;
    }
    public DateTime GetChristianDT(string PersianDate)
    {
        DateTime ReturnDate = DateTime.Now;
        try
        {
            PersianCalendar pc = new PersianCalendar();
            ReturnDate = pc.ToDateTime(Convert.ToInt32(PersianDate.Substring(0, 4)), Convert.ToInt32(PersianDate.Substring(4, 2)), Convert.ToInt32(PersianDate.Substring(6, 2)), 0, 0, 0, 0);
        }
        catch
        {
        }
        return ReturnDate;

    }
    public static DateTime GetIranChristianDT
    {
        get
        {
            DateTime dt1 = new DateTime();
            dt1 = DateTime.Now;
            dt1 = dt1.AddHours(DateTimeMethods.HourDiff);
            dt1 = dt1.AddMinutes(DateTimeMethods.MinDiff);
            return dt1;
        }
    }

}