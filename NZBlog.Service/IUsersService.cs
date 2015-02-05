using System;
using System.Collections.Generic;
using System.Data;
using NZBlog.Entity;

namespace NZBlog.Service
{
	/// <summary>
	/// 接口层Users
	/// </summary>
    public interface IUsers : IBaseService<Users>
	{
		/// <summary>
		/// 删除数据
		/// </summary>
		bool Delete(int userId);

        List<Users> GetList(UsersParam param, int pageIndex, int pageSize, out int total);

        bool Login(Users user);

        bool ChangePassword(Users model);
	} 
}