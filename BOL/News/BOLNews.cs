using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DataAccess;
using System.Globalization;
using ParsetCrawler.DAL;

public class BOLNews : BaseBOLNews, IBaseBOL<News>
{

    public bool CheckNewsExists(string NewsUrl, int ResourceSiteCatCode)
    {
        try
        {
            NewsDataContext dc = new NewsDataContext();
            IQueryable<News> Result = from n in dc.News
                                      where n.Url.Equals(NewsUrl) && n.ResouceSiteCatCode.Equals(ResourceSiteCatCode)
                                      select n;

            return (Result.Count() > 0);
        }
        catch(Exception err)
        {
            int gg = 1;
            return true;
        }
    }
    public BrowseSchema GetDataSourceByFilter(int? IGroupCode, int? NewsFlowCode, string SortString, int PageSize, int CurrentPage)
    {
        BrowseSchema BS = new BrowseSchema();

        string WhereCond = "";
        string strAnd = "";
        if (IGroupCode.HasValue && NewsFlowCode.HasValue)
            strAnd = "AND";
        string strQueryIGroup = "";
        if (IGroupCode.HasValue)
            strQueryIGroup = string.Format(" (code in (select NewsCode from NewsIGroups where IGroupCode={0}))", IGroupCode.Value);
        string strQueryNewsFlow = "";
        if (NewsFlowCode.HasValue)
            strQueryNewsFlow = string.Format(" (code in (select NewsCode from NewsNewsflows where NewsFlowCode={0}))", NewsFlowCode.Value);

        if (IGroupCode.HasValue || NewsFlowCode.HasValue)
            WhereCond = string.Format(" {0} {1} {2}", IGroupCode.HasValue ? strQueryIGroup : "", strAnd, NewsFlowCode.HasValue ? strQueryNewsFlow : "");

        var ListResult = dataContext.ExecuteQuery<vNews>(string.Format("exec spGetPaged '{0}','{1}','{2}','{3}',N'{4}'", TableOrViewName, SortString, PageSize, CurrentPage, WhereCond));
        DataTable dt = new Converter<vNews>().ToDataTable(ListResult);

        #region Count
        int RecordCount = 1;
        WhereCond = WhereCond.Replace("''", "'");
        var ResultQuery = new DBToolsDataContext().spGetCount(TableOrViewName, WhereCond);
        RecordCount = (int)ResultQuery.ReturnValue;
        BS.Count = RecordCount;
        #endregion
        BS.DataTBL = dt;
        BS.CellCollection = base.GetCellCollection();

        return BS;
    }
    public int InsertfromResourceSites(int? ZoneCode, int? SiteCode, int? HCResourceSiteCatCode, string Title, string Url, DateTime NewsDate, string Contents, string ImgUrl, string VideoSource, string PicName, int LanguageCode, int imgWidth, int imgHeight)
    {

        int? ReturnNewsCode = 0;
        NewsDataContext dc = new NewsDataContext();
        dc.spInsertNews(ZoneCode, SiteCode, HCResourceSiteCatCode, Title, Url, NewsDate, Contents, ImgUrl, VideoSource, PicName, LanguageCode, imgWidth, imgHeight, ref ReturnNewsCode);
        //Indexing  // ADD Bye Shojaei

        //this.ResouceSiteCode = SiteCode;
        //this.HCResourceSiteCatCode = HCResourceSiteCatCode;
        //this.Title = Title;
        //this.Url = Url;
        //this.NewsDate = NewsDate;
        //this.Contents = Contents;
        //this.ImgUrl = ImgUrl;
        //this.LanguageCode = LanguageCode;
        //
        return (int)ReturnNewsCode;
    }

    public IQueryable<News> GetRecentUrgentNews() //My News
    {
        return (from n in dataContext.News
                where n.Urgent.Equals(true) && n.HCLevelCode == 10
                orderby n.Code descending select n).Take(10);
    }
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here
        //if (this.ShowInFirstPage == true)
        //    if(this.HCLevelCode!=10)
        //    messages.Add("تنها اخبار با دسترسی عادی مجاز به نمایش در صفحه اول می باشند");
        return messages;
    }

    public void PutInRelatedNewsFlows(int NewsCode)
    {
        NewsDataContext dc = new NewsDataContext();
        BOLNewsNewsFlows NewsNewsFlowsBOL = new BOLNewsNewsFlows(NewsCode);
        foreach (var item in dc.spGetRelatedNewsFlows(NewsCode))
        {
            NewsNewsFlowsBOL.InsertNewRecord(NewsCode, (int)item.NewsFlowCode);
        }
    }

    public void GenRelatedNewsFlowNews(int NewsFlowCode)
    {
        NewsDataContext dc = new NewsDataContext();
        BOLNewsNewsFlows NewsNewsFlowsBOL = new BOLNewsNewsFlows(1);
        foreach (var item in dc.spGetNewsflowKeywords(NewsFlowCode))
        {
            NewsNewsFlowsBOL.InsertNewRecord(item.NewsCode, NewsFlowCode);
        }
    }

    public IQueryable<vRelatedNews> GetRelatedNews(int NewsCode, int PageSize, int PageNo)
    {
        int SkipCount = (PageNo - 1) * PageSize;
        EntityRelationsDataContext dc = new EntityRelationsDataContext();
        IQueryable<vRelatedNews> Result = dc.vRelatedNews.Where(p => p.EntityCode.Equals(NewsCode)).Skip(SkipCount).Take(PageSize);
        return Result;
    }


    public DataTable ReportNewsStatistics(int? RepType, DateTime? StartNewsDate, DateTime? EndNewsDate,
                                       string Source, int? CountryCode, int? LanguageCode,
                                       int HCResourceSiteCatCode, int HCLevelCode)
    {
        string cnStr = ConfigurationManager.ConnectionStrings["ParsetConnectionString"].ConnectionString;
        SQLServer dal = new SQLServer(cnStr);
        // ************************************** Add SP Parameters *********************************************
        dal.AddParameter("@RepType", RepType, SQLServer.SQLDataType.SQLInteger, 4, ParameterDirection.Input);
        dal.AddParameter("@StartNewsDate", StartNewsDate, SQLServer.SQLDataType.SQLDateTime, 32, ParameterDirection.Input);
        dal.AddParameter("@EndNewsDate", EndNewsDate, SQLServer.SQLDataType.SQLDateTime, 32, ParameterDirection.Input);
        dal.AddParameter("@Source", Source, SQLServer.SQLDataType.SQLString, 50, ParameterDirection.Input);
        dal.AddParameter("@CountryCode", CountryCode, SQLServer.SQLDataType.SQLInteger, 4, ParameterDirection.Input);
        dal.AddParameter("@LanguageCode", LanguageCode, SQLServer.SQLDataType.SQLInteger, 4, ParameterDirection.Input);
        dal.AddParameter("@HCResourceSiteCatCode", HCResourceSiteCatCode, SQLServer.SQLDataType.SQLInteger, 4, ParameterDirection.Input);
        dal.AddParameter("@HCLevelCode", HCLevelCode, SQLServer.SQLDataType.SQLInteger, 4, ParameterDirection.Input);
        // ************************************** Add SP Parameters *********************************************
        DataSet ds = dal.runSPDataSet("dbo.spReportNewsStatistics", null);
        dal.ClearParameters();
        return ds.Tables[0];
    }

    public DataTable DoSearch(string TableName, string OutputColumns,
                              string WhereColumnName, string TextToSearch,
                              string HCLevelCode, string EntityTypeCode, string OrderByColumn)
    {
        string cnStr = ConfigurationManager.ConnectionStrings["ParsetConnectionString"].ConnectionString;
        SQLServer dal = new SQLServer(cnStr);
        // ************************************** Add SP Parameters *********************************************
        dal.AddParameter("@TableName", TableName, SQLServer.SQLDataType.SQLNVarchar, 50, ParameterDirection.Input);
        dal.AddParameter("@OutputColumns", OutputColumns, SQLServer.SQLDataType.SQLNVarchar, 255, ParameterDirection.Input);
        dal.AddParameter("@WhereColumnName", WhereColumnName, SQLServer.SQLDataType.SQLNVarchar, 50, ParameterDirection.Input);
        dal.AddParameter("@TextToSearch", TextToSearch, SQLServer.SQLDataType.SQLNVarchar, 255, ParameterDirection.Input);
        dal.AddParameter("@HCLevelCode", HCLevelCode, SQLServer.SQLDataType.SQLNVarchar, 2, ParameterDirection.Input);
        dal.AddParameter("@EntityTypeCode", EntityTypeCode, SQLServer.SQLDataType.SQLNVarchar, 2, ParameterDirection.Input);
        dal.AddParameter("@OrderByColumn", OrderByColumn, SQLServer.SQLDataType.SQLNVarchar, 50, ParameterDirection.Input);
        // ************************************** Add SP Parameters *********************************************
        DataSet ds = dal.runSPDataSet("dbo.spFullTextSearch", null);
        dal.ClearParameters();
        return ds.Tables[0];
    }
    public IList<News> GetAll()
    {
        NewsDataContext dc = new NewsDataContext();
        IQueryable<News> allNews = (from p in dc.News
                                    select p).Take(100);
        return allNews.ToList<News>();
    }
    public IList<vNews> GetSearchedItems(object[] newcodes, int startIndex, int pageSize)
    {
        NewsDataContext dc = new NewsDataContext();
        IQueryable<vNews> allNews = dc.vNews.Where(c => newcodes.Contains(c.Code)).OrderByDescending(c => c.NewsDate).OrderByDescending(c => c.Rate);
        IList<vNews> nlist = allNews.ToList<vNews>();

        return nlist;
    }
    public override int SaveChanges(bool IsNewRecord)
    {
        int itemId = base.SaveChanges(IsNewRecord);
        return itemId;

    }
    public IList GetDateFilter(DateTime? FromDate, DateTime? ToDate)
    {
        NewsDataContext dc = new NewsDataContext();
        IQueryable<int> Result;
        if (FromDate.HasValue && ToDate.HasValue)
            Result = from p in dc.vNews
                     where p.NewsDate2.HasValue
                     && p.NewsDate2.Value.CompareTo(FromDate) >= 0
                     && p.NewsDate2.Value.CompareTo(ToDate) <= 0
                     orderby p.Rate descending, p.NewsDate descending
                     select p.Code;

        else if (!FromDate.HasValue && ToDate.HasValue)
            Result = from p in dc.vNews
                     where p.NewsDate2.HasValue
                     && p.NewsDate2.Value.CompareTo(ToDate) <= 0
                     orderby p.Rate descending, p.NewsDate descending
                     select p.Code;
        else if (FromDate.HasValue)
            Result = from p in dc.vNews
                     where p.NewsDate2.HasValue
                       && p.NewsDate2.Value.CompareTo(FromDate) >= 0
                     orderby p.Rate descending, p.NewsDate descending
                     select p.Code;
        else
            Result = from p in dc.vNews
                     orderby p.Rate descending, p.NewsDate descending
                     select p.Code;
        return Result.ToList();
    }
    public object GetImportantKeywords(int TakeCount)
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vImportantKeywords.OrderByDescending(p => p.KeywordCount).Take(TakeCount);
    }
    public object GetLatestPicNews(int TakeCount)
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vLatestPicNews.Take(TakeCount);
    }
    public IQueryable<vLatestNews> GetLatestNews(int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);

        NewsDataContext dc = new NewsDataContext();
        return dc.vLatestNews.OrderByDescending(p => p.NewsDate).Skip(SkipCount).Take(PageSize);
    }
    public int GetLatestNewsCount()
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vLatestNews.Count();
    }
    public DataTable GetKeywordTimeLine(string Keyword)
    {
        NewsDataContext dc = new NewsDataContext();
        var ListResult = dc.vKeywordsByTimes.Where(p => p.Keyword.Equals(Keyword)).Take(4);
        DataTable dt = new Converter<vKeywordsByTime>().ToDataTable(ListResult);
        DateTimeMethods dtm = new DateTimeMethods();

        //DataTable Newdt = new DataTable();
        //Newdt.Columns.Add("NewsCount", typeof(int));
        //Newdt.Columns.Add("Keyword", typeof(string));
        //Newdt.Columns.Add("NewsDay", typeof(string));

        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    //dt.Rows[i]["NewsDay"] = dtm.GetPersianDate((DateTime)dt.Rows[i]["NewsDay"]);
        //    DataRow dr = Newdt.NewRow();
        //    dr["NewsCount"] = dt.Rows[i]["NewsCount"];
        //    dr["Keyword"] = dt.Rows[i]["Keyword"];
        //    dr["NewsDay"] = dtm.GetPersianDate(Convert.ToDateTime(dt.Rows[i]["NewsDay"]));
        //    Newdt.Rows.Add(dr);
        //}

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["NewsDay"] = dtm.GetPersianDate(Convert.ToDateTime(dt.Rows[i]["NewsDay"]));
        }

        return dt;
    }
    public IQueryable<vLatestNews> GetLatestNewsByResourceCode(int ResourceSiteCode, int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);

        NewsDataContext dc = new NewsDataContext();
        return dc.vLatestNews.Where(p => p.ResourceSiteCode.Equals(ResourceSiteCode)).OrderByDescending(p => p.NewsDate).Skip(SkipCount).Take(PageSize);
    }
    public int GetLatestNewsByResourceCodeCount(int ResourceSiteCode)
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vLatestNews.Where(p => p.ResourceSiteCode.Equals(ResourceSiteCode)).Count();
    }
    public IQueryable<vNewsByKeywords> GetNewsByKeywordCode(int KeywordCode, int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);

        NewsDataContext dc = new NewsDataContext();
        return dc.vNewsByKeywords.Where(p => p.KeywordCode.Equals(KeywordCode)).OrderByDescending(p => p.NewsDate).Skip(SkipCount).Take(PageSize);
    }
    public int GetNewsByKeywordCodeCount(int KeywordCode)
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vNewsByKeywords.Where(p => p.KeywordCode.Equals(KeywordCode)).Count();
    }
    public IQueryable<vNewsflowNews> GetNewsNewsflows(int NewsflowCode, int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);

        NewsDataContext dc = new NewsDataContext();
        return dc.vNewsflowNews.Where(p => p.NewsFlowCode.Equals(NewsflowCode)).OrderByDescending(p=> p.NewsDate).Skip(SkipCount).Take(PageSize);
    }
    public int GetNewsNewsflowsCount(int NewsflowCode)
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vNewsflowNews.Where(p => p.NewsFlowCode.Equals(NewsflowCode)).Count();
    }
    public IQueryable<vNewsByCatCode> GetNewsByCatCode(int CatCode, int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);

        NewsDataContext dc = new NewsDataContext();
        return dc.vNewsByCatCodes.Where(p => p.CatCode.Equals(CatCode)).OrderByDescending(p => p.NewsDate).Skip(SkipCount).Take(PageSize);
    }
    public int GetNewsByCatCodeCount(int CatCode)
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vNewsByCatCodes.Where(p => p.CatCode.Equals(CatCode)).Count();
    }
    public object SearchNewsByKeyword(string Keyword, int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);
        Keyword = Keyword.Trim();
        while (Keyword.IndexOf("  ") >= 0)
        {
            Keyword = Keyword.Replace("  ", " ");
        }
        string[] KeywordArray = Keyword.Split(' ');
        string WhereClause = "";
        for (int i = 0; i < KeywordArray.Length; i++)
        {
            if (i == 0)
                WhereClause = "(CONTAINS(Title, N''" + KeywordArray[i] + "'') or CONTAINS(Contents, ''" + KeywordArray[i] + "''))";
            else
                WhereClause = WhereClause + " and (CONTAINS(Title, N''" + KeywordArray[i] + "'') or CONTAINS(Contents, ''" + KeywordArray[i] + "''))";
        }
        return dataContext.ExecuteQuery<vLatestNews>(string.Format("exec spSearchNews N'{0}'", WhereClause)).Skip(SkipCount).Take(PageSize);
    }
    public int SearchNewsByKeywordCount(string Keyword)
    {
        Keyword = Keyword.Trim();
        while (Keyword.IndexOf("  ") >= 0)
        {
            Keyword = Keyword.Replace("  ", " ");
        }
        string[] KeywordArray = Keyword.Split(' ');
        string WhereClause = "";
        for (int i = 0; i < KeywordArray.Length; i++)
        {
            if (i == 0)
                WhereClause = "(CONTAINS(Title, N''" + KeywordArray[i] + "'') or CONTAINS(Contents, ''" + KeywordArray[i] + "''))";
            else
                WhereClause = WhereClause + " and (CONTAINS(Title, N''" + KeywordArray[i] + "'') or CONTAINS(Contents, ''" + KeywordArray[i] + "''))";
        }

        return dataContext.ExecuteQuery<vLatestNews>(string.Format("exec spSearchNews N'{0}'", WhereClause)).Count();
    }

    public IQueryable<vLatestNews> GetLatestPicNews(int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);

        NewsDataContext dc = new NewsDataContext();
        return dc.vLatestNews.Where(p=> !p.PicName.Equals("")).OrderByDescending(p => p.NewsDate).Skip(SkipCount).Take(PageSize);
    }
    public IQueryable<vRandPicNews> GetRandPicNews(int PageSize, int PageNo)
    {
        int SkipCount = PageSize * (PageNo - 1);

        NewsDataContext dc = new NewsDataContext();
        return dc.vRandPicNews;
    }

    public int GetLatestPicNewsCount()
    {
        NewsDataContext dc = new NewsDataContext();
        return dc.vLatestNews.Where(p => !p.PicName.Equals("")).Count();
    }


    public int GetRelatedNewsCount(int NewsCode)
    {
        EntityRelationsDataContext dc = new EntityRelationsDataContext();
        return dc.vRelatedNews.Where(p => p.EntityCode.Equals(NewsCode)).Count();
    }

    public object GetNewsWithRelation(int StartIndex, int PageSize)
    {
        int SkipCount = (StartIndex - 1) * PageSize;
        EntityRelationsDataContext dc = new EntityRelationsDataContext();
        return dc.EntityRelations.Skip(SkipCount).Take(PageSize);
    }
}
