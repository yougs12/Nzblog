using System;
using System.Collections.Generic;
using System.Data;
using NZBlog.Entity;

namespace NZBlog.Service
{
	/// <summary>
	/// 接口层ZInfoMation
	/// </summary>
    public interface IZInfoMation : IBaseService<ZInfoMation>
	{
		/// <summary>
		/// 删除数据
		/// </summary>
		bool Delete(int zid);

        List<ZInfoMation> GetList(ZInfoMationParam param, int pageIndex, int pageSize, out int total);
	} 
}