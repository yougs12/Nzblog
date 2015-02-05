using System;
using System.Collections.Generic;
using System.Data;
using NZBlog.Entity;
using NZBlog.Service;

namespace NZBlog.Service
{
    /// <summary>
    /// 接口层BlogType
    /// </summary>
    public interface IBlogType : IBaseService<BlogType>
    {
        /// <summary>
        /// 删除数据
        /// </summary>
        bool Delete(int typeId);

        List<BlogType> GetList(BlogTypeParam param, int pageIndex, int pageSize, out int total);

        List<BlogType> GetList(BlogTypeParam param);
    }
}