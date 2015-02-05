using NZBlog.Entity;
using NZBlog.Provider;
using NZBlog.Website.BasicProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NZBlog.Website.Models
{
    public class BlogDefaultModel
    {
        private static BlogViewModel _blogViewModel;

        public static BlogViewModel BlogViewModel
        {
            get 
            {
                if (_blogViewModel == null)
                {
                    _blogViewModel = new Models.BlogViewModel();
                    int total;
                    _blogViewModel.BlogList = new BlogDetailProvider().GetBlogList(new BlogDetailParam(), 5, 1, out total);
                    _blogViewModel.PageBtn = MvcHtmlString.Create(new BlogManage().GetPageBtn(total, 1, "/"));
                    if (_blogViewModel.BlogList.Count > 0)
                    {
                        _blogViewModel.Lables = new LableProvider().GetNameList(_blogViewModel.BlogList.Select(b => b.BlogId).ToArray());
                    }
                }
                return _blogViewModel;
            }
        }

        public static void RemoveAll()
        {
            _blogViewModel = null;
        }

        public static void UpdateReadTimes(int id)
        {
            if (_blogViewModel != null && _blogViewModel.BlogList != null)
            {
                var model = _blogViewModel.BlogList.Find(b => b.BlogId == id);
                if (model != null)
                {
                    model.ReadTimes++;
                }
            }
        }
    }
}