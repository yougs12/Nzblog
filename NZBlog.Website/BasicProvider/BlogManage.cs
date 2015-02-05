using NZBlog.Entity;
using NZBlog.Provider;
using NZBlog.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NZBlog.Website.BasicProvider
{
    public class BlogManage
    {
        public BlogViewModel GetBlogViewModel(BlogDetailParam param, int pageIndex, string urlPre)
        {
            BlogViewModel viewModel = new BlogViewModel();
            int total;
            viewModel.BlogList = new BlogDetailProvider().GetBlogList(param, 5, pageIndex, out total);
            viewModel.PageBtn = MvcHtmlString.Create(GetPageBtn(total, pageIndex, urlPre));
            if (viewModel.BlogList.Count > 0)
            {
                viewModel.Lables = new LableProvider().GetNameList(viewModel.BlogList.Select(b => b.BlogId).ToArray());
            }
            return viewModel;
        }

        public BlogRightViewModel GetBlogRightViewModel()
        {
            BlogRightViewModel viewModel = new BlogRightViewModel();
            return viewModel;
        }

        public string GetPageBtn(int total, int pageIndex, string preUrl)
        {
            string pageBtn = @"<nav>
    <ul class=""pager"">
        {0}
        {1}
        <li> {2} / {3} 页</li>
    </ul>
</nav>";
            int pageTotal = (int)Math.Ceiling(total / 5.00);
            string pagePre = "";
            int page = pageIndex;
            if (pageIndex <= 1)
            {
                pagePre = "<li class=\"disabled\"><a href=\"javascript:void(0);\">上一页</a></li>";
            }
            else
            {
                pageIndex--;
                pagePre = "<li><a href=\"" + preUrl + pageIndex + "\">上一页</a></li>";
            }
            string nextPage = "";
            pageIndex = page;
            if (pageTotal <= pageIndex)
            {
                nextPage = "<li class=\"disabled\"><a href=\"javascript:void(0);\">下一页</a></li>";
            }
            else
            {
                pageIndex++;
                nextPage = "<li><a href=\"" + preUrl + pageIndex + "\">下一页</a></li>";
            }

            return string.Format(pageBtn, pagePre, nextPage, page, pageTotal);
        }

        
    }
}