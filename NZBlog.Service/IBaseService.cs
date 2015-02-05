using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBlog.Service
{
    public interface IBaseService<T>
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Insert(T model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(T model);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        T GetModel(int blogId);
    }
}
