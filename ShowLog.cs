using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrawlTools;
using System.Net.NetworkInformation;
using System.IO;
using libpdf;

namespace ParsetCrawler
{
    public partial class ShowLog : Form
    {
        Crawler Crl;
        string lastUrl;
        string currentUrl;
        int timeCounter;
        private void InitializeTimer()
        {
            tmrUpdate.Interval = 1800000 ; //60 minutes
            tmrUpdate.Enabled = true;

            tmrCurrency.Interval = 86400000;//1 day
            tmrCurrency.Enabled = true;
            //CrawlForCurrency();
            txtLog.Text = "Start Crawl at " + DateTime.Now;
            if (IsConnectedToInternet())
                CrawlSites();
            else
                txtLog.Text = "Not connected to internet " + DateTime.Now + "\r\n" + txtLog.Text;

        }

        public ShowLog()
        {
            Crl = new Crawler();
            InitializeComponent();
        }

        private void ShowLog_Load(object sender, EventArgs e)
        {
            InitializeTimer();
        }

        public static bool IsConnectedToInternet()
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send("4.2.2.1", 5000);
            return (reply.Status == IPStatus.Success);
        }
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                CrawlSites();
                txtLog.Text = "Start Crawl at " + DateTime.Now + "\r\n" + txtLog.Text;
            }
            else
            {
                txtLog.Text = "Not connected to internet " + DateTime.Now + "\r\n" + txtLog.Text;
            }
        }

        private void CrawlSites()
        {
            try
            {
                backgroundWorker1.RunWorkerAsync();
                tmrStatus.Enabled = true;

            }
            catch (Exception ex)
            {
                txtLog.Text = "Error Crawling on " + DateTime.Now + "  " + ex.Message + "\r\n" + txtLog.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtLog.Text = "Start Crawl on " + DateTime.Now + "\r\n" + txtLog.Text;
            CrawlSites();
        }

        private void TerminateProgram(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Crl.StartCrawl();
                txtLog.Text = "Crawl finished on " + DateTime.Now + "\r\n" + txtLog.Text;
            }
            catch (Exception ex)
            {
                int gg = 1;
                //txtLog.Text = "Exception occured: " + ex.Message + "\r\n" + txtLog.Text;
            }
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            timeCounter++;
            this.Text = "Time spending on this site :" + timeCounter;
            currentUrl = Crl.SiteAddress;
            if (currentUrl != lastUrl)
            {
                lastUrl = currentUrl;
                txtLog.Text = "Trying to get data from " + currentUrl + "- Time Spent (seconds):" + timeCounter + "\r\n" + txtLog.Text;
                timeCounter = 0;
            }

            if (timeCounter > 1800)
            {
                timeCounter = 0;
                Crl.RequestFinish();
                Crl = new Crawler();
                Crl.StartCrawl();
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void tmrCurrency_Tick(object sender, EventArgs e)
        {
            CrawlForCurrency();
        }

        private void CrawlForCurrency()
        {
            try
            {
                bgWorkerCurrency.RunWorkerAsync();
                tmrCurrency.Enabled = true;

            }
            catch (Exception ex)
            {
                txtLog.Text = "Error Crawling CURRENCY on " + DateTime.Now + "  " + ex.Message + "\r\n" + txtLog.Text;
            }
        }

        private void bgWorkerCurrency_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                //Crl.StartCrawlCurrency();
                txtLog.Text = "Crawl Currency on " + DateTime.Now + "\r\n" + txtLog.Text;
            }
            catch (Exception ex)
            {
            }

        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            CrawlForCurrency();
        }
    }
}
