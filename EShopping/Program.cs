using EShopping.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace EShopping
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int page = 1;
            string main = "";
            string content_str = "";
            Model1 db = new Model1();

            while (main != "無符合條件資料")
            {
                string htmlString = GetWebContent(@"https://web.pcc.gov.tw/tps/pss/tender.do?searchMode=common&searchType=basic&isSpdt=&method=search&pageIndex=" + $"{page}");
                //string htmlString = GetWebContent(@"https://web.pcc.gov.tw/tps/pss/tender.do?searchMode=common&searchType=basic&isSpdt=&method=search&pageIndex=1");
                HtmlDocument htmlDocument = new HtmlDocument(); //HtmlAgilityPack
                string htmlStrArranged = htmlString.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                htmlDocument.LoadHtml(htmlStrArranged);
                //HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(@"html/body/table/tr[2]/td[2]/table");
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(@"html/body/table/tr[2]/td[2]/table[1]/tr[5]/td/table[1]/tr[3]/td/table[1]/tbody/tr[1]/td/div/table");
                //HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(@"html/body/table/tr[2]/td[2]/table/tr/td[2]/table");

                //Appconfig 關鍵字
                string keyWord = ConfigurationManager.AppSettings["keyWord"];

                foreach (HtmlNode iNode in node.ChildNodes)//node
                {
                    if (iNode.ChildNodes.Count > 1)
                    {
                        Purchase purchase = new Purchase();
                        if (iNode.SelectSingleNode("td") != null && !iNode.SelectNodes("td")[0].InnerText.Contains("第一頁") && iNode.SelectNodes("td")[5].InnerText.Contains(keyWord))
                        {
                            content_str += @"<table width=""100%"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""word-break:break-all"">";
                            content_str += @"<th width=""5%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">項次&nbsp;</th><th width=""15%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">機關&nbsp;<br>名稱&nbsp;</th><th width=""26%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">標案案號&nbsp;<br>標案名稱&nbsp;</th><th width=""5%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">傳輸&nbsp;<br>次數&nbsp;</th><th width=""10%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">招標&nbsp;<br>方式&nbsp;</th><th width=""8%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">採購&nbsp;<br>性質&nbsp;</th><th width=""9%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">公告&nbsp;<br>日期&nbsp;</th><th width=""9%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">截止&nbsp;<br>投標&nbsp;</th><th width=""13%"" align=""center"" bgcolor=""#E0EDF1"" class=""T12b"" nowrap="""">預算&nbsp;<br>金額&nbsp;</th>";
                            content_str += $"<tr>{iNode.InnerHtml.Replace("..", "https://web.pcc.gov.tw/tps")}</tr>";
                            purchase.name = iNode.SelectNodes("td")[1].InnerHtml.Replace("&nbsp;", "");

                            var third = iNode.SelectNodes("td")[2].InnerHtml.Replace("&nbsp;", "");
                            purchase.project = third.Replace("<br>", ",").Replace("&nbsp;", "").Replace("..", "https://web.pcc.gov.tw/tps");
                            //電子採購.bidName= iNode.NextSibling.SelectNodes("td")[2].SelectSingleNode("a").Attributes["href"].Value.Replace("..", "https://web.pcc.gov.tw/tps");
                            purchase.counter = iNode.SelectNodes("td")[3].InnerHtml.Replace("&nbsp;", "").Replace("..", "https://web.pcc.gov.tw/tps");
                            purchase.method = iNode.SelectNodes("td")[4].InnerHtml.Replace("&nbsp;", "");
                            purchase.category = iNode.SelectNodes("td")[5].InnerHtml.Replace("&nbsp;", "");

                            string s = iNode.SelectNodes("td")[6].InnerHtml.Replace("&nbsp;", "").Replace("/", "");
                            DateTime startDate = DateTime.ParseExact(s, "yyyMMdd", CultureInfo.InvariantCulture).AddYears(1911);
                            purchase.announce_date = startDate;

                            string e = iNode.SelectNodes("td")[7].InnerHtml.Replace("&nbsp;", "").Replace("/", "");
                            DateTime endDate = DateTime.ParseExact(e, "yyyMMdd", CultureInfo.InvariantCulture).AddYears(1911);
                            purchase.submit_deadline = endDate;

                            if (!string.IsNullOrEmpty(iNode.SelectNodes("td")[8].InnerHtml))
                            {
                                if (Convert.ToInt64(iNode.SelectNodes("td")[8].InnerHtml) > 0)
                                {
                                    purchase.budget = Convert.ToInt64(iNode.SelectNodes("td")[8].InnerHtml.Replace("&nbsp;", ""));
                                }
                                else
                                {
                                    purchase.budget = -1;
                                }
                            }
                            content_str += "</table><br>";
                            //db.電子採購s.Add(電子採購);
                            db.Purchases.Add(purchase);
                        }
                    }
                    else
                    {
                        main = iNode.SelectNodes("td")[0].InnerText;
                        break;
                    }
                }

                page++;
            }
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            //    int page = 1;
            //    string htmlString = GetWebContent(@"https://web.pcc.gov.tw/tps/pss/tender.do?searchMode=common&searchType=basic&isSpdt=&method=search&pageIndex=" + $"{page}");
            //    //老師寫的爬蟲方法

            //    HtmlDocument htmlDocument = new HtmlDocument();
            //    string htmlStrArranged = htmlString.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            //    htmlDocument.LoadHtml(htmlStrArranged); //載入Html(整理過的)

            //    HtmlNode nodes = htmlDocument.DocumentNode.SelectSingleNode(@"html/body/table/tr[2]/td[2]/table[1]/tr[5]/td/table[1]/tr[3]/td/table[1]/tbody/tr[1]/td/div/table[1]");
            //    //第一個table之後的table[這邊都有數字] 只有一個也是

            //    //if (第一頁)
            //    //{
            //    //}

            //    foreach (HtmlNode node in nodes.ChildNodes)
            //    {
            //        //Attributes["href"].Value;
            //        if (node.SelectNodes("td") != null)
            //        {
            //            var n_id = node.SelectNodes("td")[0].InnerHtml.Replace("&nbsp;", "");
            //            var n_organization = node.SelectNodes("td")[1].InnerHtml.Replace("&nbsp;", "");

            //            //string second = node.SelectNodes("td")[2].InnerHtml.Replace("&nbsp;", "");  //用string來裝 方便處理
            //            //var n_project = second.Replace("<br>", ",").Replace("&nbsp;", "").Replace("..", "https://web.pcc.gov.tw/tps"); //把<br>刪去 連結補齊

            //            var n_project = node.SelectNodes("td")[2].InnerHtml.Replace("../", "https://web.pcc.gov.tw/tps/").Replace("<br>", "");
            //            //var thirdurl = node.SelectNodes("td")[2].SelectSingleNode("a").Attributes["href"].Value;
            //            //這個td要拆成兩個

            //            var n_transferedTime = node.SelectNodes("td")[3].InnerHtml.Replace("&nbsp;", "");
            //            var n_method = node.SelectNodes("td")[4].InnerHtml;
            //            var n_purchaseCategory = node.SelectNodes("td")[5].InnerHtml;
            //            var n_announceDate = node.SelectNodes("td")[6].InnerHtml;
            //            var n_submitDeadline = node.SelectNodes("td")[7].InnerHtml;
            //            var n_budget = node.SelectNodes("td")[8].InnerHtml;
            //            //Attributes["href"].Value; //可以直接抓整個連結 不須處理字串
            //        }
            //    }
            //    page++;
            //
        }

        private static string GetWebContent(string Url)
        {
            var uri = new Uri(Url);
            var request = WebRequest.Create(Url) as HttpWebRequest;
            // If required by the server, set the credentials.
            request.UserAgent = "*/*";
            request.Credentials = CredentialCache.DefaultCredentials;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            // 重點是修改這行
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls1.2;
                                                                              // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}