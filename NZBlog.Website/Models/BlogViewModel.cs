using NZBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NZBlog.Website.Models
{
    public class BlogViewModel
    {
        public List<BlogDetail> BlogList { get; set; }

        public List<Lables> Lables { get; set; }

        public MvcHtmlString PageBtn { get; set; }
    }
}