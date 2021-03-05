using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
            string htmlString = GetWebContent(@"https://web.pcc.gov.tw/tps/pss/tender.do?searchMode=common&searchType=basic&isSpdt=&method=search&pageIndex=1");
            //老師寫的爬蟲方法

            HtmlDocument htmlDocument = new HtmlDocument();
            string htmlStrArranged = htmlString.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            htmlDocument.LoadHtml(htmlStrArranged); //載入Html(整理過的)

            HtmlNode nodes = htmlDocument.DocumentNode.SelectSingleNode(@"html/body/table/tr[2]/td[2]/table[1]/tr[5]/td/table[1]/tr[3]/td/table[1]/tbody/tr[1]/td/div/table[1]");
            //第一個table之後的table[這邊都有數字] 只有一個也是

            foreach (HtmlNode node in nodes.ChildNodes)
            {
                //Attributes["href"].Value;
                if (node.SelectNodes("td") != null)
                {
                    var n_id = node.SelectNodes("td")[0].InnerHtml.Replace("&nbsp;", "");
                    var n_organization = node.SelectNodes("td")[1].InnerHtml;

                    var n_project = node.SelectNodes("td")[2].InnerHtml.Replace("../", "https://web.pcc.gov.tw/tps/");
                    //var thirdurl = node.SelectNodes("td")[2].SelectSingleNode("a").Attributes["href"].Value;
                    //這個td要拆成兩個

                    var n_transferedTime = node.SelectNodes("td")[3].InnerHtml.Replace("&nbsp;", "");
                    var n_method = node.SelectNodes("td")[4].InnerHtml;
                    var n_purchaseCategory = node.SelectNodes("td")[5].InnerHtml;
                    var n_announceDate = node.SelectNodes("td")[6].InnerHtml;
                    var n_submitDeadline = node.SelectNodes("td")[7].InnerHtml;
                    var n_budget = node.SelectNodes("td")[8].InnerHtml;
                    //Attributes["href"].Value; //可以直接抓整個連結 不須處理字串
                }
            }
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