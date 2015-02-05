using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using NZBlog.Entity;
using NZBlog.Service;
using System.Linq;
using NZBlog.Common;

namespace NZBlog.Repository
{
    //BlogDetail
    public partial class LablesDAL : BaseDAL<Lables>, ILables
    {
        public LablesDAL()
        {
            query.TableName = "Lables";
        }

        public List<Lables> GetList(LablesParam param, int pageIndex, int pageSize, out int total)
        {
            query.Select("t.*").OrderBy("LabelId desc");
            if (param.BlogId != 0)
            {
                query.Where(" BlogId = @BlogId ", new { BlogId = param.BlogId });
            }
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            return PageList(out total);
        }

        public List<Lables> GetNameList(int[] blogIds)
        {
            query.Select("t.*");
            query.Where(" BlogId in(" + blogIds.SumToString() + ") ");
                return QuerySql<Lables>(query.RawSql, query.SqlParameters);
        }

        public bool Delete(int lableId)
        {
            return Delete(new Lables() { LabelId = lableId });
        }

        public bool DeleteByBlogId(int blogId)
        {
            query.Delete().Where("BlogId=@BlogId", new { BlogId=blogId });
            return ExcuteQuery(query.RawSql, query.SqlParameters) > 0;
        }

        public void AddLables(Lables[] lables)
        {
            DeleteByBlogId(lables.First().BlogId);
            foreach (var item in lables)
            {
                Insert(item);
            }
        }

        public Lables GetModel(int lableId)
        {
            return Get(lableId);
        }
    }
}

