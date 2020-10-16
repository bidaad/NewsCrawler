using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;
using ParsetCrawler.DAL;

namespace Parset.Web.UI
{
    public class BOLCrawler 
    {
        public DateTime? GetLatestCrawlTime()
        {
            DBToolsDataContext dc = new DBToolsDataContext();
            var LatestCrawl = dc.CrawlManagers.Where(p => p.IsCrawled.Equals(true)).OrderByDescending(p => p.CheckTime).FirstOrDefault();
            if (LatestCrawl != null)
                return LatestCrawl.CheckTime;
            else
                 return null;
        }

        public void InsertCheckTime(bool IsCrawled)
        {
            DBToolsDataContext dc = new DBToolsDataContext();
            CrawlManager ObjNewRec = new CrawlManager();
            ObjNewRec.CheckTime = DateTime.Now;
            ObjNewRec.IsCrawled = IsCrawled;
            dc.CrawlManagers.InsertOnSubmit(ObjNewRec);
            dc.SubmitChanges();
        }
        public void InsertNewCrawl()
        {
            try
            {
                DBToolsDataContext dc = new DBToolsDataContext();
                CrawlLog ObjNewRec = new CrawlLog();
                ObjNewRec.CrawlTime = DateTime.Now;
                dc.CrawlLogs.InsertOnSubmit(ObjNewRec);
                dc.SubmitChanges();
            }
            catch(Exception err1)
            {
                int aa = 1;
            }
        }

    }

}