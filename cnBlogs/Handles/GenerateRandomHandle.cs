using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cnBlogs.Mvc.Handles
{
    public class GenerateRandomHandle : IHttpHandler
    {
        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int i = 0;
            int j = 5 / i;
        }
    }
}