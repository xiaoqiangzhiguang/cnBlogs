using cnBlogs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cnBlogs.Mvc.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService = new BlogService();

        // GET: Blog
        public ActionResult Index()
        {
            var blogs = _blogService.GetAllBlogs();
            return View(blogs);
        }
    }
}