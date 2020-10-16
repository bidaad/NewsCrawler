using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Net;
using Parset.Web.UI;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using ParsetCrawler.DAL;
using System.Diagnostics;
using libpdf;

namespace CrawlTools
{
    public class SinleItemContents
    {
        private int privateSiteCode;
        private string privateNewsTitle;
        private string privateNewsUrl;
        private string privateImgSiteUrl;
        private string privateREDetail;
        private string privateREImage;
        private string privateREVideo;
        private string privateLinkDomainName;
        private int privateLanguageCode;
        private int? privateCatCode;
        private int privateHCEntityTypeCode;
        private int? privateZoneCode;

        private string DomainName;
        private string SitePath;

        int privateEncodingTypeCode = 1;

        public SinleItemContents(int HCEntityTypeCode, int SiteCode, int? CatCode, int? ZoneCode, string NewsUrl, string NewsTitle, string REDetail, string REImage, string REVideo, string LinkDomainName, int EncodingTypeCode, int LanguageCode)
        {
            privateSiteCode = SiteCode;
            privateNewsUrl = NewsUrl;
            privateNewsTitle = NewsTitle;
            privateREDetail = REDetail;
            privateREImage = REImage;
            privateREVideo = REVideo;
            privateLinkDomainName = LinkDomainName;
            privateEncodingTypeCode = EncodingTypeCode;
            privateLanguageCode = LanguageCode;
            privateCatCode = CatCode;
            privateHCEntityTypeCode = HCEntityTypeCode;
            privateZoneCode = ZoneCode;

        }
        //private string ExtractKeywords(string NewsContent)
        //{
        //    Tools tools = new Tools();
        //    ArrayList OneLenList = tools.GenLenKeywords(1, 2, @"(\w+)(\w+)", NewsContent);
        //    ArrayList TwoLenList = tools.GenLenKeywords(2, 2, @"(\w+)(\w+)", NewsContent);
        //    ArrayList TreeLenList = tools.GenLenKeywords(3, 1, @"(\w+)(\w+)", NewsContent);
        //    ArrayList FourLenList = tools.GenLenKeywords(4, 1, @"(\w+)(\w+)", NewsContent);
        //    List<String> FinalKeywords = new List<String>();

        //    string[] OneLenListArray = (String[])OneLenList.ToArray(typeof(string));
        //    string[] TwoLenListArray = (String[])TwoLenList.ToArray(typeof(string));
        //    string[] TreeLenListArray = (String[])TreeLenList.ToArray(typeof(string));
        //    string[] FourLenListArray = (String[])FourLenList.ToArray(typeof(string));

        //    IEnumerable<String> FullKeyList;
        //    IEnumerable<String> TempFullKeyList;
        //    //TempFullKeyList = OneLenListArray.Union(TwoLenListArray).Union(TreeLenListArray).Union(FourLenListArray);
        //    TempFullKeyList = FourLenListArray.Union(TreeLenListArray).Union(TwoLenListArray).Union(OneLenListArray);
        //    for (int j = 0; j < TempFullKeyList.Count(); j++)
        //    {
        //        string CurrentKeyword = TempFullKeyList.GetEnumerator().Current;
        //        if (TempFullKeyList.Contains(CurrentKeyword))
        //        {
        //            IEnumerable<String> ContainList = TempFullKeyList.Where(p => p.Contains(CurrentKeyword));
        //            string CurKeyword = ContainList.GetEnumerator().Current;
        //            string[] ContainListArray = CurKeyword.Split(' ');
        //            FinalKeywords.Add(ContainListArray[0]);
        //        }
        //        else
        //            FinalKeywords.Add(CurrentKeyword);

        //    }

        //    return Tools.GetkeywordCodes(FinalKeywords);


        //}

        private string ExtractKeywords(string NewsContent)
        {
            BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();

            Tools tools = new Tools();
            ArrayList OneLenList = tools.GenLenKeywords(1, 2, @"(\w+)(\w+)", NewsContent);
            ArrayList TwoLenList = tools.GenLenKeywords(2, 2, @"(\w+)(\w+)", NewsContent);
            ArrayList TreeLenList = tools.GenLenKeywords(3, 1, @"(\w+)(\w+)", NewsContent);
            ArrayList FourLenList = tools.GenLenKeywords(4, 1, @"(\w+)(\w+)", NewsContent);
            List<String> FinalKeywords = new List<String>();

            string[] OneLenListArray = (String[])OneLenList.ToArray(typeof(string));
            string[] TwoLenListArray = (String[])TwoLenList.ToArray(typeof(string));
            string[] TreeLenListArray = (String[])TreeLenList.ToArray(typeof(string));
            string[] FourLenListArray = (String[])FourLenList.ToArray(typeof(string));

            IEnumerable<String> TempFullKeyList;
            //TempFullKeyList = OneLenListArray.Union(TwoLenListArray).Union(TreeLenListArray).Union(FourLenListArray);
            TempFullKeyList = FourLenListArray.Union(TreeLenListArray).Union(TwoLenListArray).Union(OneLenListArray);
            for (int j = 0; j < TempFullKeyList.Count(); j++)
            {
                string CurrentKeyword = TempFullKeyList.ElementAt(j);
                IEnumerable<String> ContainList = TempFullKeyList.Where(p => p.Contains(CurrentKeyword));
                if (!CurrentKeyword.Contains(" "))
                    FinalKeywords.Add(CurrentKeyword);
                else if (ContainList.Count() == 1 || !CurrentKeyword.Contains(" "))
                    FinalKeywords.Add(ContainList.ElementAt(0));

            }

            return Tools.GetkeywordCodes(FinalKeywords);

        }

        public void SaveToDB()
        {
            BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();
            try
            {


            ReqUtils gn = new ReqUtils();
            string NewsContentHtml = "";
            string FullStory = "";
            string ImageSource = "";
            string VideoSource = "";
            string TextTitle = "";
            string GeneratedFileName = "";
            string FileName;
            string FirstChar = "";
            string SavePath = "";
            string FullPath = "";
            string PreFixPath = "";
            int imgWidth = 0;
            int imgHeight = 0;

            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            if (privateEncodingTypeCode != 1)
            {
                IBaseBOL<DataTable> BolHardCode = new BOLHardCode();
                BolHardCode.QueryObjName = "HCEncodingTypes";
                DataTable dt = BolHardCode.GetDetails(privateEncodingTypeCode);
                enc = System.Text.Encoding.GetEncoding(dt.Rows[0]["Description"].ToString());
            }

            NewsContentHtml = gn.GetHTML(privateNewsUrl, enc);
            if (!string.IsNullOrEmpty(privateREDetail))
                FullStory = gn.GetREGroup(NewsContentHtml, privateREDetail, "CONTENT");
            if (!string.IsNullOrEmpty(privateREImage))
                ImageSource = gn.GetREGroup(NewsContentHtml, privateREImage, "IMAGE");
            if (!string.IsNullOrEmpty(privateREVideo))
                VideoSource = gn.GetREGroup(NewsContentHtml, privateREVideo, "VIDEO");

            if (FullStory.Length < 10)
                return;
            if (!ImageSource.StartsWith("http://") && !ImageSource.StartsWith("https://") && ImageSource != "")
                ImageSource = privateLinkDomainName + ImageSource;

            if (!ImageSource.EndsWith(".jpg") && !ImageSource.EndsWith(".gif"))
                ImageSource = "";

            string SavedFullStory = FullStory;
            FullStory = gn.RemoveTags(FullStory, "img");
            //string FullStoryWithImages = Regex.Replace(SavedFullStory, @"(?i)<(?!img|/img).*?>", String.Empty);
            //FullStoryWithImages = ReplaceImgWithItsFileName(FullStoryWithImages);//correct src paths
            if (!privateNewsUrl.StartsWith("http://") && !privateNewsUrl.StartsWith("https://"))
                privateNewsUrl = "http://" + privateNewsUrl;

            FullStory = FullStory.Replace("'", "").Replace("\"", "").Replace("&nbsp;", " ");
            TextTitle = TextTitle.Replace("&nbsp;", " ");
            TextTitle = TextTitle.Replace("&quot;", " ");

            if (TextTitle.Length > 150)
                TextTitle = TextTitle.Substring(0, 149);

            FullStory = Tools.PersianTextCorrection(FullStory);
            TextTitle = Tools.PersianTextCorrection(TextTitle.Trim());

            FullStory = gn.RemoveTags(FullStory, "br");
            FullStory = FullStory.Replace("'", "");
            FullStory = FullStory.Replace("\"", "");
            //FullStory = FullStory.Replace("\r", "");
            FullStory = FullStory.Replace("\t", "");
            FullStory = FullStory.Replace("&nbsp;", " ");

            ImageSource = ImageSource.Replace("[", "%5B");
            ImageSource = ImageSource.Replace("]", "%5D");

            VideoSource = VideoSource.Replace("[", "%5B");
            VideoSource = VideoSource.Replace("]", "%5D");



            if (ImageSource != "") //Save News File
            {
                int SlashPos = ImageSource.LastIndexOf("/");
                FileName = ImageSource.Substring(SlashPos + 1, ImageSource.Length - SlashPos - 1);
                GeneratedFileName = Tools.GetRandomFileName(FileName);
                string CurMonth = DateTime.Now.Month.ToString();
                if (CurMonth.Length == 1)
                    CurMonth = "0" + CurMonth;
                string CurDay = DateTime.Now.Day.ToString();
                if (CurDay.Length == 1)
                    CurDay = "0" + CurDay;

                PreFixPath = DateTime.Now.Year + "" + CurMonth + "" + CurDay;
                if (!Directory.Exists(ConfigurationManager.AppSettings["SaveImagePath"] + PreFixPath))
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["SaveImagePath"] + PreFixPath);
                //FirstChar = GeneratedFileName.Substring(0, 1);
                FullPath = ConfigurationManager.AppSettings["SaveImagePath"] + PreFixPath + "/" + GeneratedFileName;
                SavePath = PreFixPath + "/" + GeneratedFileName;
            }
            else
            {
                FullPath = "";
                SavePath = "";
            }

            TextTitle = gn.RemoveTags(privateNewsTitle).Trim();
            if (TextTitle.Length >= 400)
                TextTitle = TextTitle.Substring(0, 399);

                if (privateHCEntityTypeCode == 1)
                {
                    int NewsCode;
                    BOLNews NewsBOL = new BOLNews();

                    if (ImageSource != "") //Save News File
                    {
                        try
                        {
                            //WebClient WebCl = new WebClient();
                            //WebCl.DownloadFile(ImageSource, ConfigurationManager.AppSettings["SaveImagePath"] + GeneratedFileName);
                            //gn.DownloadPicture(ImageSource, ConfigurationManager.AppSettings["SaveImagePath"] + GeneratedFileName);

                            gn.DownloadPicture(ImageSource, FullPath, out imgWidth, out imgHeight);
                        }
                        catch (Exception SaveExp)
                        {
                            ErrorLogsBOL.InsertLog(SaveExp.Message + "Inner Exception: " + SaveExp.InnerException, "Save News Picture");
                            //throw SaveExp;
                            GeneratedFileName = null;
                        }
                    }


                    NewsCode = NewsBOL.InsertfromResourceSites(privateZoneCode, privateSiteCode, privateCatCode, TextTitle, privateNewsUrl, DateTime.Now, FullStory, ImageSource, VideoSource, SavePath, privateLanguageCode, imgWidth, imgHeight);

                    #region save news Images
                    try
                    {
                        NewsDataContext dcImages = new NewsDataContext();
                        string _pattern = @"<img.*?src\s*=\s*\\*""(.+?)\\*""\s*.*?>";
                        Regex r = new Regex(_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match m = r.Match(SavedFullStory);
                        while (m.Success)
                        {
                            string CurImage = m.Groups[1].Captures[0].ToString();
                            if (!CurImage.StartsWith("data:image") && CurImage.IndexOf("loading") == -1)
                            {
                                if (!CurImage.StartsWith("http://") && !CurImage.StartsWith("https://") && CurImage != "")
                                {
                                    if(CurImage.StartsWith("//"))
                                    {
                                        CurImage = "http:" + CurImage;
                                    }
                                    else if(!string.IsNullOrEmpty( privateLinkDomainName))
                                        CurImage = privateLinkDomainName + CurImage;
                                    else
                                    {
                                        int HTTPIndex = privateNewsUrl.IndexOf("http");
                                        int FirstSlashIndex = privateNewsUrl.IndexOf("/", HTTPIndex + 8);
                                        string MainLink = privateNewsUrl.Substring(0, FirstSlashIndex);
                                        CurImage = MainLink + "/" + CurImage;
                                    }
                                }

                                NewsImages NewImage = new NewsImages();
                                NewImage.NewsCode = NewsCode;
                                NewImage.ImgUrl = CurImage;
                                dcImages.NewsImages.InsertOnSubmit(NewImage);
                                dcImages.SubmitChanges();
                            }
                            m = m.NextMatch();
                        }
                    }
                    catch
                    {

                    }
                    #endregion



                    #region Save Keyword Codes
                    string KeywordCodeList = ExtractKeywords(FullStory);
                    BOLEntityKeywords EntityKeywordsBOL = new BOLEntityKeywords();

                    EntityKeywordsBOL.SaveKeywordList(NewsCode, KeywordCodeList, 1);
                    BOLEntityRelations EntityRelationsBOL = new BOLEntityRelations();
                    EntityRelationsBOL.SetAllRelations(NewsCode, 1, 4);
                    NewsBOL.PutInRelatedNewsFlows(NewsCode);
                    #endregion
                }

            }
            catch (Exception exp)
            {
                ErrorLogsBOL.InsertLog(exp.Message, "Save News");
            }
            finally
            {
                //Thread.CurrentThread.Abort();
                //gn.Dispose();

            }



        }


        private string ReplaceImgWithItsFileName(string input)
        {
            string tmpNewsUrl = privateNewsUrl.Replace("http://", "").Replace("https://", "");
            DomainName = GetDomainName();
            SitePath = GetSitePath(tmpNewsUrl);

            Regex regex = new Regex("<img.*?src=[\"|'](?<src>.*?)[\"|'].*?>",
                                    RegexOptions.IgnoreCase
                                    | RegexOptions.CultureInvariant
                                    | RegexOptions.IgnorePatternWhitespace
                                    | RegexOptions.Compiled
                                    );

            return regex.Replace(input, new MatchEvaluator(ReplaceImageName));


        }

        private string GetDomainName()
        {
            int FirstSlashPos = privateNewsUrl.IndexOf("/");
            if (FirstSlashPos == -1)
                return privateNewsUrl;
            else
            {
                return privateNewsUrl.Substring(0, FirstSlashPos);
            }
        }

        private string GetSitePath(string SiteUrl)
        {
            int FirstSlashPos = SiteUrl.IndexOf("/");
            if (FirstSlashPos == -1)
                return SiteUrl;
            string PathAfterDomainName = SiteUrl.Substring(FirstSlashPos + 1, SiteUrl.Length - FirstSlashPos - 1);
            if (PathAfterDomainName.IndexOf(".") != -1)
            {
                int LastSlashPos = SiteUrl.LastIndexOf("/");
                return SiteUrl.Substring(0, LastSlashPos);
            }
            else
                return SiteUrl;
        }

        private string ReplaceImageName(Match match)
        {
            string Result = "";
            string FoundSRC = match.Groups[1].ToString();
            if (FoundSRC.StartsWith("http://"))
                Result = FoundSRC;
            else if (FoundSRC.StartsWith("/"))
                Result = DomainName + FoundSRC;
            else if (FoundSRC.IndexOf("../") == -1)
                Result = SitePath + "/" + FoundSRC;
            else
            {
                while (FoundSRC.IndexOf("../") != -1)
                {
                    int LastIndex = SitePath.LastIndexOf("/");
                    SitePath = SitePath.Substring(0, LastIndex);
                    FoundSRC = FoundSRC.Substring(3, FoundSRC.Length - 3);
                }
                Result = SitePath + "/" + FoundSRC;
            }

            if (FoundSRC.StartsWith("http://"))
                Result = "<br /><img src=\"" + Result + "\" />";
            else
                Result = "<br /><img src=\"http://" + Result + "\" />";
            return Result;
        }

    }

    public class CrawlSingleSite
    {
        // State information used in the task.
        private int privateSiteCode;
        private int privateLimitCount;

        // The constructor obtains the state information.
        public CrawlSingleSite(int SiteCode, int LimitCount)
        {
            privateSiteCode = SiteCode;
            privateLimitCount = LimitCount;
        }

        public ArrayList GetNewsList(string rssAddress)
        {
            //string rssAddress = "http://rss.msnbc.msn.com/id/3032091/device/rss/rss.xml";
            ArrayList NewsAL = new ArrayList();
            try
            {
                object objRss = Tools.ConsumeRss(rssAddress, null);
                if (objRss != null)
                {
                    DataView dv = (DataView)objRss;
                    for (int i = 0; i < dv.Table.Rows.Count; i++)
                    {
                        DataRow dr = dv.Table.Rows[i];
                        NewsAL.Add("<a href='" + dr["link"] + "'>" + dr["title"] + "</a>");
                    }
                }
            }
            catch
            {
            }
            return NewsAL;
        }
        public void StartCrawl()
        {
            try
            {
                int Count = 1;
                string NewsTitle = "";
                string HtmlContent = "";

                SinleItemContents dbt;

                BOLResourseSiteCats ResourceSiteCatsBOL = new BOLResourseSiteCats(1);
                vResourseSiteCats SingleSite = ResourceSiteCatsBOL.GetSingleSite(privateSiteCode);

                BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();
                int SiteCode = SingleSite.Code;
                //int CatCode = (int)SingleSite.CatCode;
                int HCEntityTypeCode = SingleSite.HCEntityTypeCode;
                int? ZoneCode = SingleSite.ZoneCode;

                string SiteUrl = SingleSite.Url;
                int? CatCode = SingleSite.CatCode;
                int? EncodingTypeCode = SingleSite.HCEncodingTypeCode;
                int? LanguageCode = SingleSite.LanguageCode;
                string BaseURL = SingleSite.BaseURL;

                string RELink = SingleSite.RELink;
                string REDetail = SingleSite.REDetail;
                string REImage = SingleSite.REImage;
                string REVideo = SingleSite.REVideo;

                ReqUtils gn = new ReqUtils();
                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                if (EncodingTypeCode != 1)
                {
                    IBaseBOL<DataTable> BolHardCode = new BOLHardCode();
                    BolHardCode.QueryObjName = "HCEncodingTypes";
                    DataTable dt = BolHardCode.GetDetails((int)EncodingTypeCode);
                    enc = System.Text.Encoding.GetEncoding(dt.Rows[0]["Description"].ToString());
                }
                ArrayList NewList;

                int LastSlash = SiteUrl.LastIndexOf("/");
                string LinkDomainName = "";

                if (BaseURL != null && BaseURL != "")
                    LinkDomainName = BaseURL;
                else if (LastSlash != -1)
                    LinkDomainName = SiteUrl.Substring(0, LastSlash);

                if (!(bool)SingleSite.RssIsActive)
                {
                    HtmlContent = gn.GetHTML(SiteUrl, enc);
                    NewList = gn.ExtractNewsLinks(HtmlContent, RELink, LinkDomainName);
                }
                else
                {
                    NewList = GetNewsList(SingleSite.RssUrl);
                }

                IEnumerator NewENum = NewList.GetEnumerator();

                gn = new ReqUtils();
                Thread NewThread;
                while (NewENum.MoveNext())
                {
                    NewsTitle = NewENum.Current.ToString();
                    gn.RemoveTags(NewsTitle);
                    BOLNews NewsBOl = new BOLNews();

                    string RealLink = gn.ExtractLink(NewsTitle);
                    if (!NewsBOl.CheckNewsExists(RealLink, privateSiteCode))
                    {
                        dbt = new SinleItemContents(HCEntityTypeCode, SiteCode, CatCode, ZoneCode, RealLink, NewsTitle, REDetail, REImage, REVideo, LinkDomainName, (int)EncodingTypeCode, (int)LanguageCode);
                        dbt.SaveToDB();
                        //NewThread = new Thread(new ThreadStart(dbt.SaveToDB));
                        //NewThread.Start();
                    }

                    Count++;
                    if (privateLimitCount != 0)
                        if (privateLimitCount == Count)
                            break;
                }
            }
            catch (Exception errMain)
            {
                BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();
                ErrorLogsBOL.InsertLog(errMain.Message, "MainErrora");

            }
        }
    }


    public class Crawler
    {
        public bool Abort
        {
            set
            {
                _abort = value;
            }
        }
        protected bool _abort = false;
        public string SiteAddress { get; set; }
        public int StartCrawl()
        {
            //return -1;
            BOLCrawler CrawlerBOL = new BOLCrawler();
            CrawlerBOL.InsertNewCrawl();

            CrawlSingleSite tws;
            Thread NewThread;

            BOLResourseSiteCats ResourceSiteCatsBOL = new BOLResourseSiteCats(1);
            IQueryable<vResourseSiteCats> ActiveSiteList = ResourceSiteCatsBOL.GetActiveSites();
            foreach (var Site in ActiveSiteList)
            {
                if (_abort)
                {
                    Thread.CurrentThread.Abort();
                    return -1;
                }
                SiteAddress = Site.Url + "(" + Site.Name + ")";
                int SiteCode = Site.Code;
                int LimitCount = 10;

                tws = new CrawlSingleSite(SiteCode, LimitCount);
                tws.StartCrawl();
                Thread.Sleep(1000);
                //NewThread = new Thread(new ThreadStart(tws.StartCrawl));
                //NewThread.Start();
            }
            int ActiveSiteCount = ActiveSiteList.Count();
            return ActiveSiteCount;
        }

        internal void RequestFinish()
        {
            _abort = true;
        }

        internal void StartCrawlCurrency()
        {
            try
            {
                GetNewspaperFirstPages();
                //GetLatestCurrency();
                SendNewsEmails();
            }
            catch (Exception err)
            {
                int gg = 1;
            }
        }

        private void GetNewspaperFirstPages()
        {

            try
            {
                DateTimeMethods dtm = new DateTimeMethods();
                string PersianDate = dtm.GetPersianDate(Tools.GetIranDate());

                WebClient WebCl = new WebClient();
                ReqUtils Reqs = new ReqUtils();
                NewspapersDataContext dc = new NewspapersDataContext();
                var Result = dc.Newspapers;
                foreach (var item in Result)
                {
                    try
                    {
                        string PicUrlPath = "";
                        string DomainUrl = "";
                        string MainUrl = item.URL;

                        if (dc.NPFirstPages.Where(p => p.PersianDate.Equals(PersianDate) && p.NewspaperCode.Equals(item.Code)).Count() > 0)
                            continue;
                        string CurUrl = item.URL;
                        string PageHTML = Reqs.GetHTML(CurUrl, System.Text.Encoding.UTF8);

                        if (!string.IsNullOrEmpty(item.NextPageRE))
                        {
                            CurUrl = Reqs.GetREGroup(PageHTML, item.NextPageRE, "NextUrl");
                            if (!CurUrl.StartsWith("http://"))
                            {
                                int FirstSlashPos = MainUrl.IndexOf("/", 9);
                                if (FirstSlashPos != -1)
                                    DomainUrl = MainUrl.Substring(0, FirstSlashPos);
                                else
                                    DomainUrl = MainUrl;

                                CurUrl = DomainUrl + "/" + CurUrl;
                            }
                            PageHTML = Reqs.GetHTML(CurUrl, System.Text.Encoding.UTF8);
                        }

                        string PicUrl = Reqs.GetREGroup(PageHTML, item.REPic, "PIC");
                        #region SavePic
                        if (!PicUrl.StartsWith("http://") && PicUrl != "")
                        {
                            CurUrl = CurUrl.Replace("http://", "");
                            PicUrl = PicUrl.Replace("../", "");

                            if (string.IsNullOrEmpty(item.BaseUrl))
                            {
                                int LastSlashPos = CurUrl.LastIndexOf("/");
                                if (LastSlashPos != -1)
                                    PicUrlPath = CurUrl.Substring(0, LastSlashPos);
                                else
                                    PicUrlPath = CurUrl;
                                PicUrl = "http://" + PicUrlPath + "/" + PicUrl;
                            }
                            else
                            {
                                PicUrlPath = item.BaseUrl;
                                PicUrl = PicUrlPath + "/" + PicUrl;
                                DomainUrl = MainUrl;
                            }

                            string CurMonth = DateTime.Now.Month.ToString();
                            if (CurMonth.Length == 1)
                                CurMonth = "0" + CurMonth;
                            string CurDay = DateTime.Now.Day.ToString();
                            if (CurDay.Length == 1)
                                CurDay = "0" + CurDay;

                            string PreFixPath = DateTime.Now.Year + "" + CurMonth + "" + CurDay;
                            if (!Directory.Exists(ConfigurationManager.AppSettings["SaveImagePath"] + PreFixPath))
                                Directory.CreateDirectory(ConfigurationManager.AppSettings["SaveImagePath"] + PreFixPath);

                            DomainUrl = DomainUrl.Replace("http://", "");
                            string GeneratedFileName = DomainUrl.Replace("/", "_") + "_" + PersianDate.Replace("/", "_") + ".jpg";
                            WebCl.DownloadFile(PicUrl, ConfigurationManager.AppSettings["SaveImagePath"] + PreFixPath + "/" + GeneratedFileName);

                            NPFirstPages NewRecord = new NPFirstPages();
                            dc.NPFirstPages.InsertOnSubmit(NewRecord);
                            NewRecord.PicUrl = ConfigurationManager.AppSettings["SaveImagePath"] + PreFixPath + "/" + GeneratedFileName;
                            NewRecord.PersianDate = PersianDate;
                            NewRecord.NewspaperCode = item.Code;
                            dc.SubmitChanges();

                            int ff = 1;
                        }
                        #endregion

                    }
                    catch (Exception errNPPic)
                    {
                        BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();
                        ErrorLogsBOL.InsertLog(errNPPic.Message, "Newspapers");
                    }


                }
            }
            catch (Exception errMainNewspapers)
            {
                BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();
                ErrorLogsBOL.InsertLog(errMainNewspapers.Message, "Newspapers");
            }
        }

        private static void GetLatestCurrency()
        {
            try
            {
                ItemPricesDataContext dc = new ItemPricesDataContext();

                string Url = "http://widgets.farsnews.com/currency/";
                string ItemName = "";
                string ItemVal = "";
                ReqUtils Reqs = new ReqUtils();
                string Result = Reqs.GetHTML(Url, System.Text.Encoding.UTF8);

                string _pattern = @"<tr class=""(table-row-alt|table-row)"">\s*<td>\s*(.*?)\s*</td>\s*<td>\s*(.*?)\s*</td>\s*<td>(.*?)\s*</td>\s*</tr>";
                Regex r = new Regex(_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m = r.Match(Result);
                while (m.Success)
                {
                    ItemName = m.Groups[3].Captures[0].ToString();
                    ItemVal = m.Groups[4].Captures[0].ToString();

                    ItemName = ItemName.Trim();
                    ItemVal = ItemVal.Trim();

                    Items CurItem = dc.Items.SingleOrDefault(p => p.Title.Equals(ItemName));
                    if (CurItem != null)
                    {
                        ItemVal = Tools.UnChageEnc(ItemVal);
                        ItemVal = ItemVal.Replace(",", "");
                        ItemPrices NewPrice = new ItemPrices();
                        dc.ItemPrices.InsertOnSubmit(NewPrice);
                        NewPrice.ItemCode = CurItem.Code;
                        NewPrice.Val = ItemVal;
                        NewPrice.CreateDate = DateTime.Now;
                        dc.SubmitChanges();
                    }

                    m = m.NextMatch();
                }
            }
            catch
            {
            }
        }

        public void SendNewsEmails()
        {
            try
            {
                BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();

                NewsDataContext Newsdc = new NewsDataContext();
                DateTimeMethods dtm = new DateTimeMethods();
                string PersianDate = dtm.GetPersianDate(Tools.GetIranDate());
                var EmailList = Newsdc.spGetNotSentNewsEmails(PersianDate);
                EmailLists newSend = new EmailLists();
                newSend.PersianDate = PersianDate;
                newSend.SendDate = Tools.GetIranDate();
                newSend.HCSendTypeCode = 1;
                Newsdc.EmailLists.InsertOnSubmit(newSend);
                Newsdc.SubmitChanges();
                int EmailListCode = newSend.Code;

                var NewsList = Newsdc.vNewsEmailItems.Take(25);
                string strNewsList = "";
                string FirstTitle = "";

                int NewsCounter = 0;
                foreach (var CurNews in NewsList)
                {
                    if (NewsCounter == 0)
                        FirstTitle = CurNews.Title;
                    strNewsList += @"<tr><td>&nbsp;&nbsp;<b>»</b>&nbsp;&nbsp;</td><td style=""padding:3px;border-bottom:1px solid #eee;font:11px Tahoma;line-height:22px;width:100%"">&nbsp;<a href=""http://www.parset.com/News/ShowNews.aspx?Code=" + CurNews.Code + @""" style=""color:#003562;text-decoration:none;font:11px Tahoma"" target=""_blank"">" + CurNews.Title + "</a></td></tr>";
                    NewsCounter++;
                }
                //int EmailCount = EmailList.Count();
                EmailTemplatesDataContext dcTemplate = new EmailTemplatesDataContext();
                EmailTemplates CurTemplate = dcTemplate.EmailTemplates.SingleOrDefault(p => p.Title.Equals("NewsEmailItem"));
                if (CurTemplate != null)
                {
                    int InnerCounter = 0;
                    int Counter = 0;
                    string BCCList = "";
                    string MailTemplate = CurTemplate.Template;

                    foreach (var CurEmail in EmailList)
                    {
                        InnerCounter++;
                        if (BCCList == "")
                            BCCList = CurEmail.Email;
                        else
                            BCCList += "," + CurEmail.Email;
                        if (InnerCounter >= 50)
                        {
                            try
                            {
                                Counter += 50;
                                string MailBody = MailTemplate;
                                MailBody = MailTemplate.Replace("[NewsList]", strNewsList);
                                MailBody = MailBody.Replace("[Email]", CurEmail.Email);
                                MailBody = MailBody.Replace("[ID]", CurEmail.ID);
                                MailSender ms = new MailSender(MailBody, FirstTitle, "\"Parset News\" <noreply@parset.com>", CurEmail.Email, EmailListCode, CurEmail.Code, BCCList);
                                ms.SendSingleEmail();
                                Thread.Sleep(10000);
                            }
                            catch (Exception err)
                            {
                                ErrorLogsBOL.InsertLog("SendingEmail", err.Message);
                            }
                            finally
                            {
                                InnerCounter = 0;
                                BCCList = "";
                            }
                        }
                    }
                    ErrorLogsBOL.InsertLog("EmailSendCount", Counter + " Emails Sent");

                    //ms.SendSingleEmail();
                }
                else
                {
                    ErrorLogsBOL.InsertLog("NoTemplate", "Email");

                }
            }
            catch (Exception errMailEmail)
            {
                BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();
                ErrorLogsBOL.InsertLog(errMailEmail.Message, "Email");
            }

        }

    }

    public class MailSender
    {
        string _mailBody;
        string _subject;
        string _fromEmail;
        string _toEmail;
        string _toBCC;
        int _emailListCode;
        int _emailCode;


        public MailSender(string MailBody, string Subject, string FromEmail, string ToEmail, int EmailListCode, int EmailCode, string ToBCC)
        {
            _mailBody = MailBody;
            _subject = Subject;
            _fromEmail = FromEmail;
            _toEmail = ToEmail;
            _emailListCode = EmailListCode;
            _emailCode = EmailCode;
            _toBCC = ToBCC;
        }
        public bool SendSingleEmail()
        {
            try
            {
                System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();
                message.From = _fromEmail;
                message.To = _toEmail;
                message.Bcc = _toBCC;
                message.Subject = _subject;
                message.Body = _mailBody;
                message.BodyFormat = System.Web.Mail.MailFormat.Html;
                message.BodyEncoding = new System.Text.UTF8Encoding();
                System.Web.Mail.SmtpMail.SmtpServer = "46.4.104.67";

                message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);
                message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 25);

                message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "info@parset.com");
                message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "Alireza12#$");
                System.Web.Mail.SmtpMail.Send(message);

                NewsDataContext Newsdc = new NewsDataContext();
                EmailListSends newSendEmail = new EmailListSends();
                Newsdc.EmailListSends.InsertOnSubmit(newSendEmail);
                newSendEmail.EmailListCode = _emailListCode;
                newSendEmail.Email = _toEmail;
                Newsdc.SubmitChanges();

                string[] ToBCCArray = _toBCC.Split(',');
                for (int i = 0; i < ToBCCArray.Length; i++)
                {
                    newSendEmail = new EmailListSends();
                    Newsdc.EmailListSends.InsertOnSubmit(newSendEmail);
                    newSendEmail.EmailListCode = _emailListCode;
                    newSendEmail.Email = ToBCCArray[i];
                    Newsdc.SubmitChanges();
                }

                return true;
            }
            catch (Exception errEmail)
            {
                BOLErrorLogs ErrorLogsBOL = new BOLErrorLogs();
                ErrorLogsBOL.InsertLog(errEmail.Message + _toBCC, "EmailWithThread");
                return false;
            }


        }

    }
}
/// <summary>
/// Summary description for Crawler
/// </summary>


