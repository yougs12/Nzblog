using System;
using System.Collections.Generic;
using System.Data;
using NZBlog.Entity;

namespace NZBlog.Service
{
    /// <summary>
    /// 接口层BlogDetail
    /// </summary>
    public interface IBlogDetail : IBaseService<BlogDetail>
    {
        /// <summary>
        /// 删除数据
        /// </summary>
        bool Delete(int blogId);

        /// <summary>
        /// 根据分页获得数据列表
        /// </summary>
        List<BlogDetail> GetList(BlogDetailParam param, int pageIndex, int pageSize, out int total);

        List<BlogDetail> GetBlogList(BlogDetailParam param, int pageIndex, int pageSize, out int total);

        List<BlogDetail> GetNewList();

        List<BlogDetail> GetRssBlog();

        bool UpdateReadTimes(int blogId);
    }
}