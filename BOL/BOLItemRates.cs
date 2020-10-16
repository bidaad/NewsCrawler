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


    public class BOLItemRates
    {
        public ItemRates GetDetail(int ItemCode, int HCEntityCode)
        {
            DBToolsDataContext dc = new DBToolsDataContext();
            return dc.ItemRates.SingleOrDefault(p => p.ItemCode.Equals(ItemCode) && p.HCEntityCode.Equals(HCEntityCode));
        }

        public void UpdateVal(int ItemCode, int HCEntityCode, decimal RateVal, int RateCount)
        {
            DBToolsDataContext dc = new DBToolsDataContext();
            ItemRates CurRate = dc.ItemRates.SingleOrDefault(p => p.ItemCode.Equals(ItemCode) && p.HCEntityCode.Equals(HCEntityCode));
            if (CurRate == null)
            {
                CurRate = new ItemRates();
                dc.ItemRates.InsertOnSubmit(CurRate);
            }
            CurRate.ItemCode = ItemCode;
            CurRate.HCEntityCode = HCEntityCode;
            CurRate.RateVal = RateVal;
            CurRate.RateCount = RateCount;
            dc.SubmitChanges();
        }
    }

