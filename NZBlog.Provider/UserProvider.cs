using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NZBlog.Entity;
using NZBlog.Factory;
using NZBlog.Service;

namespace NZBlog.Provider
{
    public class UserProvider
    {
        private readonly IUsers _users = new IFactory().CreateUsers();

        public List<Users> GetPageList(UsersParam param, int pageIndex, int pageSize, out int total)
        {
            return _users.GetList(param, pageIndex, pageSize, out total);
        }

        public Users GetUser(int blogId)
        {
            return _users.GetModel(blogId);
        }

        public int AddUser(Users model)
        {
            return _users.Insert(model);
        }

        public bool UpdateUser(Users model)
        {
            return _users.Update(model);
        }

        public bool DeleteUser(int userId)
        {
            return _users.Delete(userId);
        }

        public bool ChangePassword(Users model)
        {
            return _users.ChangePassword(model);
        }

        public bool Login(Users model)
        {
            return _users.Login(model);
        }
    }
}
