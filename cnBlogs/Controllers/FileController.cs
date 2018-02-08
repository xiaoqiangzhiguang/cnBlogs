using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace cnBlogs.Mvc.Controllers
{
    public class FileController : Controller
    {
        private const int ThreadNum = 5;
        private const string DownloadDestination = "F:\\data";


        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public async void Download(string path)
        {
            HttpWebRequest request = WebRequest.CreateHttp(path);
            request.ContentType = "application/octet-stream";
            var response  = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            long originFileLength = responseStream.Length;
            if (originFileLength > 0)
            {
                long partSize = originFileLength / ThreadNum;
                for (var i = 1; i <= ThreadNum; i++)
                {
                    Thread t = new Thread(new ParameterizedThreadStart(DownLoadPart));
                    var partInfo = new DownloadPartInfo()
                    {
                        Path = path,
                        StartIndex = (i - 1) * partSize,
                        EndIndex = i * partSize - 1 ,
                        CurrentIndex = i
                    };
                    if (i == ThreadNum)
                    {
                        partInfo.EndIndex += originFileLength % ThreadNum;
                    }
                    t.Start(partInfo);
                }
            }
        }

        public void DownLoadPart(object part)
        {
            if(part == null || ! (part is DownloadPartInfo))
            {
                return;
            }
            var downloadData = (DownloadPartInfo)part;
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(downloadData.Path);
                request.ContentType = "application/octet-stream";
                request.Headers.Add(string.Format("Range", "bytes={0}-{1}", downloadData.StartIndex, downloadData.EndIndex));
                var response = request.GetResponse();
                var responseStream = response.GetResponseStream();

                Response.ContentType = "application/octet-stream";
                Response.Headers.Add("Content-Disposition", "attachment;filename=" + downloadData.CurrentIndex + ".tmp");

                byte[] buffer = new byte[downloadData.EndIndex - downloadData.StartIndex + 1];
                responseStream.Read(buffer,0, (int)responseStream.Length);

                Response.Write(buffer);
            }
            catch (Exception ex)
            {

            }
        }

        public class DownloadPartInfo
        {
            public string Path { get; set; }

            public long StartIndex { get; set; }

            public long EndIndex { get; set; }

            public int CurrentIndex { get; set; }
        }
    }
}