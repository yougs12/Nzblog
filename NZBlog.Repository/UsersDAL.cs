using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using NZBlog.Common;
using NZBlog.Entity;
using NZBlog.Service;

namespace NZBlog.Repository
{
    //BlogDetail
    public partial class UsersDAL : BaseDAL<Users>, IUsers
    {
        public UsersDAL()
        {
            query.TableName = "Users";
        }

        public List<Users> GetList(UsersParam param, int pageIndex, int pageSize, out int total)
        {
            query.Select("t.*").OrderBy("UserId desc");
            if (!param.UserName.IsNullOrEmpty())
            {
                query.Where(" userName like @userName or RealName like @userName", new { userName = "%" + param.UserName.Trim() + "%" });
            }

            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            return PageList(out total);
        }

        public bool Delete(int userId)
        {
            return Delete(new Users() { UserId = userId });
        }

        public Users GetModel(int userId)
        {
            return Get(userId);
        }

        public override int Insert(NZBlog.Entity.Users model)
        {
            model.PassWord = (model.UserName + "666666").SHA1();
            query.Select("count(1) cnt").Where("UserName=@UserName", new { UserName = model.UserName });
            var cnt = ExecuteScalar(query.RawSql, query.SqlParameters).ToInt32();
            if (cnt == 0)
            {
                return base.Insert(model);
            }
            return 0;
        }

        public override bool Update(Users model)
        {
            query.Update("UserName,RealName,ReMark", model).Where("UserId=@UserId", new { UserId = model.UserId });
            return ExcuteQuery(query.RawSql, query.SqlParameters) > 0;
        }

        public bool ChangePassword(Users model)
        {
            query.Update("PassWord", new { PassWord = (model.UserName + model.PassWord).SHA1() }).Where("UserName=@UserName", new { UserName = model.UserName });
            return ExcuteQuery(query.RawSql, query.SqlParameters) > 0;
        }

        public bool Login(Users user)
        {
            query.Select("count(1) cnt").Where("UserName=@UserName and PassWord=@PassWord", new { UserName = user.UserName, PassWord = (user.UserName + user.PassWord).SHA1() });
            var cnt = ExecuteScalar(query.RawSql, query.SqlParameters).ToInt32();
            if (cnt > 0)
            {
                Dapper.Query queryU = new Dapper.Query() { TableName = "Users" };
                queryU.Update("LastLoginTime", new { LastLoginTime = DateTime.Now }).Where("UserName=@UserName", new { UserName = user.UserName });
                ExcuteQuery(queryU.RawSql, queryU.SqlParameters);
                return true;
            }
            return false;
        }
    }
}

