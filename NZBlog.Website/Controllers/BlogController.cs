using NZBlog.Website;
using NZBlog.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NZBlog.Entity;
using NZBlog.Common;
using NZBlog.Website.BasicProvider;

namespace NZBlog.Website.Controllers
{
    public class BlogController : Controller
    {

        public ActionResult Index(int? pageIndex, string word)
        {
            BlogViewModel viewModel = new BlogViewModel();
            if ((pageIndex == null || pageIndex == 1) && word.IsNullOrEmpty())
            {
                viewModel = BlogDefaultModel.BlogViewModel;
            }
            else
                viewModel = new BlogManage().GetBlogViewModel(new Entity.BlogDetailParam { Title = word }, pageIndex ?? 1, "/");
            return View(viewModel);
        }

        public ActionResult GetTypeList(int? typeid, int? pageIndex)
        {
            BlogViewModel viewModel = new BlogManage().GetBlogViewModel(new Entity.BlogDetailParam { TypeId = typeid ?? 0 }, pageIndex ?? 1, "/bloglist/" + (typeid ?? 0));
            return View("Index", viewModel);
        }

        public ActionResult BlogDetail(int? id)
        {
            int _id = id ?? 1;
            Entity.BlogDetail detail = new Provider.BlogDetailProvider().GetBlogDetail(_id);
            Entity.Lables[] lables = new Provider.LableProvider().GetNameList(new int[] { _id }).ToArray();
            detail.Lables = lables.Select(l => l.LabName).SumToString("|");
            //记录访问次数
            HttpCookie readTimes = Request.Cookies["readTimes"];
            bool isRead = false;
            if (readTimes == null)
            {
                isRead = true;
                readTimes = new HttpCookie("readTimes", _id.ToString());
            }
            else
            {
                string _readTimesValue = readTimes.Value;
                List<string> ids = _readTimesValue.Split('|').ToList();
                if (!ids.Contains(_id.ToString()))  
                {
                    isRead = true;
                    ids.Add(_id.ToString());
                    readTimes.Value = ids.SumToString("|");
                }
            }
            Response.Cookies.Add(readTimes);
            if (isRead)
            {
                new Provider.BlogDetailProvider().UpdateReadTimes(_id);
                BlogDefaultModel.UpdateReadTimes(_id);
                BlogRightViewModel.UpdateReadTimes(_id);
            }
            return View(detail);
        }

        [NonAction]
        public ActionResult Rss(RssEntity rss)
        {
            RssResult result = new RssResult();
            result.Data = rss;
            return result;
        }

        public ActionResult Rss()
        {
            RssEntity rss = new Provider.BlogDetailProvider().GetRssBlog().ToList().ToDefaultRss();
            return Rss(rss);
        }
    }
}
