using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


	/// <summary>
	/// 
	/// </summary>
	public class ReqUtils:System.Web.UI.Page
	{
        public string CurrentSite = "";
        public string GetHTML(string url, Encoding encoding)
		{
			String result = "";
            url = url.Replace("//", "/");
            url = url.Replace("http:/", "http://");
            url = url.Replace("https:/", "https://");
        try
        {
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
| SecurityProtocolType.Tls11
| SecurityProtocolType.Tls12
| SecurityProtocolType.Ssl3;


            CurrentSite = url;
                WebProxy wproxy = new WebProxy("proxy", 8080);
                //wproxy.Credentials = new NetworkCredential("gorji", "omega2");
                //objRequest.Proxy = wproxy;
                objRequest.Method = "GET";
                objRequest.Timeout = 30000;
                //objRequest.Timeout = 18000000;
                objRequest.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
                //objRequest.Proxy = wproxy;

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
               
                using (StreamReader sr =
                           new StreamReader(objResponse.GetResponseStream(), encoding))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
            }
			catch(Exception e)
			{
			    //Response.Write(e.Message);
			}
			return result;

		}
		public Stream GetHTMLStream(string url)
		{
			String result = "";

			try
			{
				HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
                WebProxy wproxy = new WebProxy("proxy", 8080);
                wproxy.Credentials = new NetworkCredential("gorji", "omega2");
                //objRequest.Proxy = wproxy;
                objRequest.Method = "GET";
				objRequest.Timeout = 18000000;
                objRequest.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

				//			objRequest.Credentials = CredentialCache.DefaultCredentials;

				HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
				Stream str =  objResponse.GetResponseStream();
				Encoding encoding; 

				return str;
			}
			catch(Exception e)
			{
				return null;
			}

		}
		public decimal DoCompare(string Content1, string Content2)
		{
			if(Content2.Length > Content1.Length )
			{
				string TempStr = Content2 ;
				Content2 = Content1;
				Content1 = TempStr;
			}
			char[] Spliter = new char[1];
			string CurrentWord = "";
			Spliter[0] =  ' ';
			Content1 = Content1.ToLower();
			Content2 = Content2.ToLower();
			int Counter = 0;
			decimal ContentLen = 0;
			decimal CompVal = 0;

			string[] Content1Array =  Content1.Split(Spliter);
			for(int k = 0; k < Content1Array.Length ; k++)
			{
				CurrentWord = Content1Array[k];
				if(CurrentWord.Length > 3)
				{
					ContentLen++;
					if( Content2.IndexOf(CurrentWord, 0) >= 0 )
						Counter++;
				}
			}
			CompVal = Counter/ContentLen;

			return CompVal;
		}
		public ArrayList ExtractNewsLinks(string HtmlContent, string LinkPattern,string LinkDomainName)
		{
            string LinkUrl ="";
            string LinkText ="";
            string FinalLink = "";
			ArrayList NewsAL = new ArrayList();

            Regex r = new Regex(LinkPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match m = r.Match(HtmlContent);
            while (m.Success)
            {
                try
                {
                    LinkUrl = m.Groups["URL"].Captures[0].Value;
                    LinkText = m.Groups["TEXT"].Captures[0].Value;

                    LinkUrl = LinkUrl.Trim();
                    if (LinkUrl != "" && LinkUrl.ToLower().Substring(0, 7) != "http://")
                        LinkUrl = LinkDomainName + LinkUrl;

                    FinalLink = "<a href=\"" + LinkUrl + "\">" + LinkText + "</a>";
                    NewsAL.Add(FinalLink);
                }
                catch(Exception CrlExp)
                {
                    int aa = 1;
                    //throw new Exception("Crawler Error");
                }
                m = m.NextMatch();
            }
			return NewsAL;
		}
		public string ExtractLink (string LinkContent)
		{
			int StartHrefPos = 0;
			int EndHrefPos = 0;
			int CutLen = 0;
			int EndTagIndex = 0;
			int LinkStartIndex = 0;
			int LinkEndIndex = 0;
			string FinalLink = "";

			LinkContent = LinkContent.Replace ("<A ", "<a ");
			LinkContent = LinkContent.Replace ("HREF", "href");
			LinkContent = LinkContent.Replace ("href =", "href=");
			
            string LinkPattern = @"href=""(?<URL>[^>]*?)""|href='(?<URL>[^>]*?)'";
            Regex r = new Regex(LinkPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match m = r.Match(LinkContent);
            if (m.Success)
            {
                FinalLink = m.Groups["URL"].Captures[0].Value;
            }
            if (FinalLink == "")
            {
                LinkStartIndex = LinkContent.IndexOf("href=", 0);
                EndTagIndex = LinkContent.IndexOf(">", LinkStartIndex);

                LinkEndIndex = LinkContent.IndexOf(" ", LinkStartIndex);
                if (LinkEndIndex == -1)
                    LinkEndIndex = LinkContent.IndexOf(">", LinkStartIndex);
                if (LinkEndIndex > EndTagIndex)
                    LinkEndIndex = EndTagIndex;

                CutLen = LinkEndIndex - LinkStartIndex - 5;
                LinkContent = LinkContent.Substring(LinkStartIndex + 5, CutLen);
                FinalLink = LinkContent;
            }

            FinalLink = FinalLink.Replace("\"", "");
            FinalLink = FinalLink.Replace(">", "");
            FinalLink = FinalLink.Replace("'", "");
            FinalLink = FinalLink.Replace("&amp;", "&");
            FinalLink = FinalLink.Replace("../", "");
            FinalLink = FinalLink.Replace(" ", "");
            FinalLink = FinalLink.Trim();
			return FinalLink;
		}
		public string GetPath(string Url)
		{
			int SlashIndex = 0;
			string RealPath = "";
			SlashIndex = Url.LastIndexOf("/", Url.Length);
			if(SlashIndex >0)
				RealPath = Url.Substring(0, SlashIndex);

			return RealPath;


		}
		public string ExtractImageSource(string HtmlContent, string StartString, string EndString, string ImgSiteUrl, string ContentUrl)
		{
			string InnerContent = "";
			int CutLen = 0;
			int StartIndex = 0;
			int EndIndex = 0;
			int EndNewsIndex = 999999;
			int StartSrcIndex = 0;
			int EndSrcIndex = 0;
			string ImageSource = "";
			
			HtmlContent = HtmlContent.Replace("SRC", "src");
//			StartString = StartString.ToLower();
//			EndString = EndString.ToLower();

			StartIndex = HtmlContent.IndexOf (StartString,0);
			if( StartIndex > 0 )
			{
				EndIndex = HtmlContent.IndexOf (EndString,StartIndex + StartString.Length + 1);
				CutLen = EndIndex - StartIndex - StartString.Length;
				try
				{
					InnerContent = HtmlContent.Substring (StartIndex + StartString.Length ,CutLen);

					InnerContent = InnerContent.Replace("'", "\"");
					StartSrcIndex = InnerContent.IndexOf("src",0);
					EndSrcIndex = InnerContent.IndexOf("\"",StartSrcIndex + 6);
					CutLen = EndSrcIndex - StartSrcIndex - 5;
					ImageSource = InnerContent.Substring (StartSrcIndex + 5 ,CutLen);
					ImageSource = ImageSource.Replace("../", "");

					if(ImgSiteUrl != "")
						ImageSource = ImgSiteUrl + ImageSource;
					else
					{
						if( ImageSource.ToLower().IndexOf("http://", 0) < 0)
						{
							if( ImageSource.Substring(0, 1) == "/")
								ImageSource = GetPath(ContentUrl) + ImageSource;
							else
								ImageSource = GetPath(ContentUrl) + "/" + ImageSource;
						}
					}


				}
				catch(Exception e)
				{
				}
				StartIndex = HtmlContent.IndexOf (StartString,StartIndex + 1);

			}
			return ImageSource;

		}
        public string GetREGroup(string HtmlContent, string REDetail, string GroupName)
        {
            string Result = "";
            Regex r = new Regex(REDetail, RegexOptions.Singleline);
            Match m = r.Match(HtmlContent);
            if (m.Success)
            {
                try
                {
                    Result = m.Groups[GroupName].Captures[0].Value;
                    Result = Result.Replace("\n", "");
                    Result = Result.Replace("\t", "");
                }
                catch
                {
                }
            }
            return Result;
        }
        public string ExtractFullStory(string HtmlContent, string REDetail)
		{
            string Result = "";
            Regex r = new Regex(REDetail, RegexOptions.Singleline);
            Match m = r.Match(HtmlContent);
            if (m.Success)
            {
                try
                {
                    Result = m.Groups["CONTENT"].Captures[0].Value;
                    Result = Result.Replace("\n", "");
                    Result = Result.Replace("\t", "");
                }
                catch
                {
                }
            }
            return Result;
		}
		public string RemoveTags(string InputHtml,params string[] ExceptionTagList )
		{
			string InnerContent = InputHtml;
			int CutLen = 0;
			int StartIndex = 0;
			int EndIndex = 0;
			string TagContent = "";
			InnerContent = InnerContent.Replace("<SCRIPT", "<script");
			InnerContent = InnerContent.Replace("</SCRIPT", "</script");

			StartIndex = InnerContent.IndexOf ("<script",0);
			while ( StartIndex > 0 )
			{
				EndIndex = InnerContent.IndexOf ("</script>",StartIndex + 7);
				if(EndIndex > 0)
				{
					CutLen = EndIndex - StartIndex + 9;
					TagContent = InnerContent.Substring (StartIndex ,CutLen);
					InnerContent = InnerContent.Replace(TagContent , "");
					StartIndex = StartIndex - TagContent.Length  ;
				}
				else
					break;

				StartIndex = InnerContent.IndexOf ("<script",0);
			}

			InnerContent = InnerContent.Replace("<IFRAME", "<iframe");
			InnerContent = InnerContent.Replace("</IFRAME", "</iframe");

			StartIndex = InnerContent.IndexOf ("<iframe",0);
			while ( StartIndex > 0 )
			{
				EndIndex = InnerContent.IndexOf ("</iframe>",StartIndex + 7);
				if(EndIndex > 0)
				{
					CutLen = EndIndex - StartIndex + 9;
					TagContent = InnerContent.Substring (StartIndex ,CutLen);
					InnerContent = InnerContent.Replace(TagContent , "");
					StartIndex = StartIndex - TagContent.Length  ;
				}
				StartIndex = InnerContent.IndexOf ("<iframe",0);
			}

			StartIndex = InnerContent.IndexOf ("<",0);
			while ( StartIndex >= 0 )
			{
				EndIndex = InnerContent.IndexOf (">",StartIndex + 1);
				if(EndIndex > 0)
				{
					CutLen = EndIndex - StartIndex - 1;
					TagContent = InnerContent.Substring (StartIndex + 1 ,CutLen);
                    if (ExceptionTagList.Length == 0)
                        InnerContent = InnerContent.Replace("<" + TagContent + ">", "");
                    else
                    {
                        for (int i = 0; i < ExceptionTagList.Length; i++)
                        {
                            if (!TagContent.ToUpper().StartsWith(ExceptionTagList[i].ToUpper()))
                                InnerContent = InnerContent.Replace("<" + TagContent + ">", "");
                            else
                                InnerContent = InnerContent.Replace("<" + TagContent + ">", "\n");
                        }
                    }
					StartIndex = StartIndex - TagContent.Length  ;
				}
				else
					break;
				StartIndex = InnerContent.IndexOf ("<",0);
			}
            InnerContent = InnerContent.Replace("\r", "");
            InnerContent = InnerContent.Replace("\t", "");

            while (InnerContent.IndexOf("\n\n") >= 0)
            {
                InnerContent = InnerContent.Replace("\n\n", "\n");
                
            }
			return InnerContent;

		}

        internal void DownloadPicture(string ImageSource, string ImgFilePathandName, out int imgWidth, out int imgHeight)
        {
            System.Drawing.Image oI;
            Graphics oGraphics;
            int StaticVal = 300;
            int NewHeight = StaticVal;
            int NewWidth = StaticVal;
            System.Drawing.Image BulkBmp = null;

            try
            {
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(ImageSource);
                WebProxy wproxy = new WebProxy("proxy", 8080);
                objRequest.Method = "GET";
                objRequest.Timeout = 18000000;
                objRequest.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                Stream str = objResponse.GetResponseStream();

                oI = System.Drawing.Image.FromStream(str);
                System.Drawing.Image image = new Bitmap(oI);

                //			oI.Save(Response.OutputStream,ImageFormat.Jpeg);
                int width = oI.Width;
                int height = oI.Height;

                imgWidth = width;
                imgHeight = height;
                if (width > height)
                {
                    NewWidth = StaticVal;
                    NewHeight = (StaticVal * height) / width;
                }
                else
                {
                    NewHeight = StaticVal;
                    NewWidth = (StaticVal * width) / height;
                }

                BulkBmp = new Bitmap(NewWidth, NewHeight);

                oGraphics = Graphics.FromImage(BulkBmp);

                oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                oGraphics.DrawImage(
                    image,
                    new Rectangle(0, 0, NewWidth, NewHeight),
                    // destination rectangle 
                    0,
                    0,           // upper-left corner of source rectangle
                    width,       // width of source rectangle
                    height,      // height of source rectangle
                    GraphicsUnit.Pixel);

                BulkBmp.Save(ImgFilePathandName, ImageFormat.Jpeg);
                oI.Dispose();
                oGraphics.Dispose();
            }
            catch (Exception e)
            {
                imgWidth = 0;
                imgHeight = 0;

            }
            finally
            {
                BulkBmp.Dispose();
            }
        }
    }
