using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace PrescientSecuritiesAssessment {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            
            
           List<int> yearsList = new List<int> {2021};
            downloadFoldersAsync(yearsList);
           //DownloadFiles("https://clientportal.jse.co.za/downloadable-files?RequestNode=/YieldX/Derivatives/Docs_DMTM", @"C:\Users\nothilem\Downloads\clemy.txt", yearsList);
           DownloadFiles("https://clientportal.jse.co.za/downloadable-files?RequestNode=/YieldX/Derivatives/Docs_DMTM", @"C:\Users\nothilem\Downloads\clemy.txt");
           
        }

        private async Task DownloadFiles(string fileurls, string path) {
            //string fileurls = "https://clientportal.jse.co.za/downloadable-files?RequestNode=/YieldX/Derivatives/Docs_DMTM";

            bool testing = urlValidation(fileurls);
            if (testing) {
                WebClient web1 = new WebClient();
                web1.DownloadFile(fileurls, path);
                //HtmlNode[] nodes= ExtractUrls(@"C:\Users\nothilem\Downloads\clemy.txt");
                HtmlNode[] nodes= ExtractUrls(path);
                foreach (HtmlNode item in nodes) {
                    if (item.InnerHtml.StartsWith("2021") || item.InnerHtml.StartsWith("2022")) {
                        if (!File.Exists(@"C:\" + $"{ item.InnerHtml}")) {
                            WebClient web = new WebClient();
                            await web.DownloadFileTaskAsync(fileurls + "/" + $"{ item.InnerHtml}", @"C:\" + $"{ item.InnerHtml}");
                            Console.WriteLine(item.InnerHtml);
                        }
                    }
                    //else if(item.InnerHtml.StartsWith("2022") && item.InnerHtml.EndsWith("pdf")){
                    //    web1.DownloadFileTaskAsync("https://clientportal.jse.co.za/_layouts/15/DownloadHandler.ashx?FileName=/YieldX/Derivatives/Docs_DMTM/" + $"{ item.InnerHtml}", @"C:\Users\nothilem\Downloads\"+$"{ item.InnerHtml}" );

                    //}

                }

            } 

           


        }


            private async Task downloadFoldersAsync(List<int> years) {
            
            foreach (int item in years) {
                string uri= "https://clientportal.jse.co.za/downloadable-files?RequestNode=/YieldX/Derivatives/Docs_DMTM/"+ $"{item}";

                  //DownloadFiles(uri, @"C:\Users\nothilem\Downloads\clementine.txt",years);
                await  DownloadFiles(uri, @"C:\Users\nothilem\Downloads\clementine.txt");
            }
        }

        private bool urlValidation(string url) {
            bool Test = true;
            Uri urlCheck = new Uri(url);
            WebRequest request = WebRequest.Create(urlCheck);
            request.Timeout = 15000;

            WebResponse response;
            try {
                response = request.GetResponse();
            } catch (Exception) {
                Test= false; //url does not exist
            }

            return Test;
        }

        private HtmlNode[] ExtractUrls(string path) {

            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument document = web.Load("http://www.c-sharpcorner.com");
            HtmlAgilityPack.HtmlDocument document2 = new HtmlAgilityPack.HtmlDocument();
          //  document2.Load(@"C:\Users\nothilem\Downloads\clemy.txt");
            document2.Load(path);

            HtmlNode[] nodes= document2.DocumentNode.SelectNodes(" //a[@class='inline']").ToArray();
            return nodes; 
        }

    }

}

 