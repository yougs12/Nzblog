using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NZBlog.Common;
using NZBlog.Provider;
using NZBlog.Entity;

namespace NZBlog.Website.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VerifyImage()
        {
            ValidateCode validateBo = new ValidateCode();
            string validateCode = validateBo.CreateValidateCode(5);
            Session["validateCode"] = validateCode;
            var imgFrom = validateBo.CreateValidateGraphic(validateCode);
            return File(imgFrom, "image/png");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string PassWord)
        {
            if (Session["nzuser"] != null)
            {
                Users user = new Users { UserName = Session["nzuser"].ToString() };
                user.PassWord = PassWord;
                if (new UserProvider().ChangePassword(user))
                    return Json(new { Status = 1, Msg = "修改成功！" });
            }
            return Json(new { Status = 0, Msg = "修改失败！" });
        }

        [HttpPost]
        public ActionResult Login(Users user)
        {
            if (Session["ValidateCode"] != null && Session["ValidateCode"].ToString() == user.ValiCode)
            {
                var isLogin = new UserProvider().Login(user);
                if (isLogin)
                {
                    Session["nzuser"] = user.UserName;
                    return Json(new { Status = 1, Msg = "登录成功！" });
                }
                else
                {
                    Session["ValidateCode"] = null;
                }
            }
            else
            {
                Session["ValidateCode"] = null;
                return Json(new { Status = 0, Msg = "验证码错误！" });
            }
            return Json(new { Status = 0, Msg = "登录失败！" });
        }

        public ActionResult LoginOut()
        {
            Session["nzuser"] = null;
            return Redirect("/account/Index");
        }
    }
}
