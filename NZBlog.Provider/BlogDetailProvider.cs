using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NZBlog.Entity;
using NZBlog.Factory;
using NZBlog.Service;

namespace NZBlog.Provider
{
    public class BlogDetailProvider
    {
        private IBlogDetail blogDetail = new IFactory().CreateBlogDetail();

        public List<BlogDetail> GetPageList(BlogDetailParam param, int pageSize, int pageIndex, out int total)
        {
            return blogDetail.GetList(param, pageSize, pageIndex, out total);
        }

        /// <summary>
        /// 获取博客明细列表
        /// </summary>
        public List<BlogDetail> GetBlogList(BlogDetailParam param, int pageIndex, int pageSize, out int total)
        {
            return blogDetail.GetBlogList(param, pageSize, pageIndex, out total);
        }

        public BlogDetail GetBlogDetail(int blogId)
        {
            return blogDetail.GetModel(blogId);
        }

        public int AddBlog(BlogDetail model)
        {
            int blogId= blogDetail.Insert(model);
            List<Lables> lables = new List<Lables>();
            foreach (var item in model.Lables.Split(','))
            {
                lables.Add(new Lables { BlogId = blogId, LabName = item });
            }
            new LableProvider().AddLables(lables.ToArray());
            return blogId;
        }

        public bool UpdateBlog(BlogDetail model)
        {
            List<Lables> lables = new List<Lables>();
            foreach (var item in model.Lables.Split(','))
            {
                lables.Add(new Lables { BlogId = model.BlogId, LabName = item });
            }
            new LableProvider().AddLables(lables.ToArray());
            return blogDetail.Update(model);
        }

        public bool DeleteBlog(int blogId)
        {
            return blogDetail.Delete(blogId);
        }

        public List<BlogDetail> GetNewList()
        {
            return blogDetail.GetNewList();
        }

        public List<BlogDetail> GetRssBlog()
        {
            return blogDetail.GetRssBlog();
        }

        public bool UpdateReadTimes(int blogId)
        {
            return blogDetail.UpdateReadTimes(blogId);
        }
    }
}
