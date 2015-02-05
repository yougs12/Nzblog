using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NZBlog.Common;

namespace NZBlog.Website.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(Session["nzuser"]==null)
            {
                filterContext.Result = Redirect("~/account/");
            }
            base.OnActionExecuting(filterContext);
        }

        protected event Action<object> PreLoad;
        #region 获取列表数据公共方法
        /// <summary>
        /// 获取列表数据公共方法
        /// </summary>
        /// <typeparam name="TParm">GetPageData方法的TParm参数类型，将查询条件封装到该类型实体对象里</typeparam>
        /// <typeparam name="TModel">要返回列表的数据实体类型</typeparam>
        /// <param name="parm">GetPageData方法的TParm参数类型的对象</param>
        /// <param name="GetPageData">获取分页数据的方法</param>
        /// <param name="ConvertTOResultModel">将实体转换成页面需要的对象方法,可以不传此参数（默认会数据原样返回)</param>
        /// <returns></returns>
        public ActionResult GetAjaxList<TParm, TModel>(TParm parm, GetPageListData<TParm, TModel> GetPageData, Func<TModel, object> ConvertTOResultModel = null)
        {
            var list = new List<object>();
            var Msg = new object();
            Msg = "暂无记录";
            int total = 0;
            try
            {
                int pageIndex = Request["pageIndex"].ToInt32();
                int pageSize = Request["pageSize"].ToInt32();
                List<TModel> TModelList = GetPageData(parm, pageIndex, pageSize, out total);
                if (PreLoad != null) PreLoad(TModelList);
                if (TModelList != null && TModelList.Count > 0)
                {
                        if (ConvertTOResultModel != null)
                        {
                            list = TModelList.Select(ConvertTOResultModel).ToList();
                        }
                        else
                        {
                            list = TModelList.Select(t => (object)t).ToList();
                        }
                }
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
            }
            return Json(new { list, Msg, total }, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 提交数据
        public ActionResult SubmitData<T>(bool isUpdate, T model, Func<T, bool> updateAction, Func<T, int> addAction)
        {
            try
            {
                if (isUpdate)
                {
                    if (updateAction(model))
                    {
                        return Json(new { Status = 1, Msg = "更新成功！" });
                    }
                    else
                    {
                        return Json(new { Status = 0, Msg = "更新失败！" });
                    }
                }
                else
                {
                    if (addAction(model) > 0)
                    {
                        return Json(new { Status = 1, Msg = "添加成功！" });
                    }
                    else
                    {
                        return Json(new { Status = 0, Msg = "添加失败！" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Msg = "Exception：" + ex.Message });
            }
        }

        public ActionResult AddData<T>(T model, Func<T, int> addAction)
        {
            return SubmitData<T>(false, model, null, addAction);
        }
        public ActionResult UpdateData<T>(T model, Func<T, bool> updateAction)
        {
            return SubmitData<T>(true, model, updateAction, null);
        }

        public ActionResult DeleteData(string Ids, Func<int, bool> deleteAction)
        {
            try
            {
                foreach (var item in Ids.Split(','))
                {
                    if (!deleteAction(item.ToInt32()))
                    {
                        return Json(new { Status = 0, Msg = "删除失败！" });
                    }
                }
                return Json(new { Status = 1, Msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Msg = "Exception：" + ex.Message });
            }
        }

        public ActionResult ProcessAction(object obj, Func<object, bool> doAction)
        {
            try
            {
                if (!doAction(obj))
                {
                    return Json(new { Status = 0, Msg = "执行失败！" });
                }
                return Json(new { Status = 1, Msg = "执行成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Msg = "Exception：" + ex.Message });
            }
        }
        #endregion
    }
    /// <summary>
    /// 分页方法对应的委托
    /// </summary>
    public delegate List<TModel> GetPageListData<TParm, TModel>(TParm parm, int pageIndex, int PageSize, out int total);
}
