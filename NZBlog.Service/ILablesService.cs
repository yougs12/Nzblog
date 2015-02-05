using System;
using System.Collections.Generic;
using System.Data;
using NZBlog.Entity;

namespace NZBlog.Service
{
	/// <summary>
	/// 接口层Lables
	/// </summary>
    public interface ILables : IBaseService<Lables>
	{
		/// <summary>
		/// 删除数据
		/// </summary>
		bool Delete(int labelId);

        List<Lables> GetList(LablesParam param, int pageIndex, int pageSize, out int total);

        ///// <summary>
        ///// 删掉指定博客所有标签
        ///// </summary>
        //bool DeleteByBlogId(int blogId);

        void AddLables(Lables[] lables);

        List<Lables> GetNameList(int[] blogIds);
	} 
}