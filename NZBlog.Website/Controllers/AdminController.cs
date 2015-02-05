using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NZBlog.Common;
using NZBlog.Entity;
using NZBlog.Provider;
using NZBlog.Website.Models;

namespace NZBlog.Website.Controllers
{
    public class AdminController : BaseController
    {
        #region 保持登录
        public ActionResult Index()
        {
            return View("BlogDetailList");
        }

        public ActionResult KeepLogin()
        {
            return Content("1");
        } 
        #endregion

        #region 用户管理
        public ActionResult UserList()
        {
            return View();
        }

        public ActionResult GetUserList(UsersParam param)
        {
            return GetAjaxList(param, new UserProvider().GetPageList, r => new
                {
                    UserId = r.UserId,
                    UserName = r.UserName,
                    RealName = r.RealName,
                    Remark = r.ReMark,
                    LastLoginTime = r.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatTime = r.CreatTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
        }

        [HttpPost]
        public ActionResult SubmitUser(Users user)
        {
            user.CreatTime = DateTime.Now;
            user.LastLoginTime = DateTime.Now;
            UserProvider bll = new UserProvider();
            return SubmitData(user.UserId != 0, user, bll.UpdateUser, bll.AddUser);
        }

        public ActionResult DeleteUser(string ids)
        {
            UserProvider bll = new UserProvider();
            return DeleteData(ids, bll.DeleteUser);
        }
        #endregion

        #region 站点片段
        public ActionResult ZInfoMationList()
        {
            return View();
        }

        public ActionResult GetZInfoMationList(ZInfoMationParam param)
        {
            return GetAjaxList(param, new ZInfoMationProvider().GetPageList);
        }

        public ActionResult ZInfoMationEdit(int? id)
        {
            ZInfoMation model = new ZInfoMation();
            if (id.HasValue)
            {
                model = new ZInfoMationProvider().GetZInfoMation(id.Value);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitZInfoMation(ZInfoMation zInfoMation)
        {
            ZInfoMationProvider bll = new ZInfoMationProvider();
            return SubmitData(zInfoMation.ZId != 0, zInfoMation, bll.UpdateZInfoMation, bll.AddZInfoMation);
        }

        public ActionResult DeleteZInfoMation(string ids)
        {
            ZInfoMationProvider bll = new ZInfoMationProvider();
            return DeleteData(ids, bll.DeleteZInfoMation);
        }

        public ActionResult GenerateZinfo(int zid)
        {
            return ProcessAction(zid, a => 
            {
                int zId = a.ToInt32();
                ZInfoMation zinfo = new ZInfoMationProvider().GetZInfoMation(zid);
                string path = Server.MapPath("~/Views/Shared/" + zinfo.ZCode + ".cshtml");
                System.IO.File.WriteAllText(path, zinfo.ZContent,System.Text.Encoding.Default);
                return true; 
            });
        }
        #endregion

        #region 博客分类
        public ActionResult BlogTypeList()
        {
            return View();
        }

        public ActionResult GetBlogTypeList(BlogTypeParam param)
        {
            return GetAjaxList(param, new BlogTypeProvider().GetPageList, a => { a.ParentTypeName = a.ParentTypeName ?? ""; return a; });
        }

        public ActionResult BlogTypeEdit(int? id)
        {
            BlogType model = new BlogType();
            if (id.HasValue)
            {
                model = new BlogTypeProvider().GetBlogType(id.Value);
            }
            int total;
            var ParentTypeList = new BlogTypeProvider().GetPageList(new BlogTypeParam { ParentId = 0 }, 1, 1000, out total).Select(b => new SelectListItem { Text = b.TypeName, Value = b.TypeId.ToString() }).ToList();
            ParentTypeList.Remove(ParentTypeList.Find(a => a.Value == model.TypeId.ToString()));//移除自己的类别ID
            ParentTypeList.Insert(0, new SelectListItem { Text = "请选择父类", Value = "0" });
            ParentTypeList.Find(a => a.Value == model.ParentId.ToString()).Selected = true;
            ViewBag.ParentId = ParentTypeList;
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitBlogType(BlogType blogType)
        {
            BlogTypeProvider bll = new BlogTypeProvider();
            return SubmitData(blogType.TypeId != 0, blogType, bll.UpdateBlogType, bll.AddBlogType);
        }

        public ActionResult DeleteBlogType(string ids)
        {
            BlogTypeProvider bll = new BlogTypeProvider();
            return DeleteData(ids, bll.DeleteBlogType);
        }
        #endregion

        #region 博客管理
        public ActionResult BlogDetailList()
        {
            return View();
        }

        public ActionResult GetBlogDetailList(BlogDetailParam param)
        {
            return GetAjaxList(param, new BlogDetailProvider().GetPageList, a => { return a; });
        }

        public ActionResult BlogDetailEdit(int? id)
        {
            BlogDetail model = new BlogDetail();
            if (id.HasValue)
            {
                model = new BlogDetailProvider().GetBlogDetail(id.Value);
            }
            int total;
            var ParentTypeList = new BlogTypeProvider().GetPageList(new BlogTypeParam { }, 1, 1000, out total).Select(b => new SelectListItem { Text = b.TypeName, Value = b.TypeId.ToString() }).ToList();
            ParentTypeList.Insert(0, new SelectListItem { Text = "请选择分类", Value = "0" });
            ParentTypeList.Find(a => a.Value == model.TypeId.ToString()).Selected = true;
            ViewBag.TypeId = ParentTypeList;
            int total1;
            model.Lables = new LableProvider().GetPageList(new LablesParam { BlogId = model.BlogId }, 1, 10, out total1).Select(l=>l.LabName).SumToString();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitBlogDetail(BlogDetail blogDetail)
        {
            blogDetail.Lables = Request.Form["Lables"];
            BlogDetailProvider bll = new BlogDetailProvider();
            return SubmitData(blogDetail.TypeId != 0, blogDetail, bll.UpdateBlog, bll.AddBlog);
        }

        public ActionResult DeleteBlogDetail(string ids)
        {
            BlogDetailProvider bll = new BlogDetailProvider();
            return DeleteData(ids, bll.DeleteBlog);
        }

        public ActionResult UploadImage()
        {
            return Content(new UploadFile().UploadImg(Response, Request));
        }

        public void RemoveData()
        {
            NZBlog.Website.Models.BlogRightViewModel.RemoveData();
            BlogDefaultModel.RemoveAll();
        }
        #endregion
    }
}