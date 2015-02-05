using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using NZBlog.Entity;
using NZBlog.Service;

namespace NZBlog.Repository
{
    //BlogDetail
    public partial class ZInfoMationDAL : BaseDAL<ZInfoMation>, IZInfoMation
    {
        public ZInfoMationDAL()
        {
            query.TableName = "ZInfoMation";
        }

        public List<ZInfoMation> GetList(ZInfoMationParam param, int pageIndex, int pageSize, out int total)
        {
            query.Select("t.*").OrderBy("zid desc");

            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            return PageList(out total);
        }

        public bool Delete(int id)
        {
            return Delete(new ZInfoMation() { ZId = id });
        }

        public ZInfoMation GetModel(int userId)
        {
            return Get(userId);
        }
    }
}

