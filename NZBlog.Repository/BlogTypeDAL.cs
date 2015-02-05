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
    public partial class BlogTypeDAL : BaseDAL<BlogType>,  IBlogType
    {
        public BlogTypeDAL()
        {
            query.TableName = "BlogType";
        }

        public List<BlogType> GetList(BlogTypeParam param, int pageIndex, int pageSize, out int total)
        {
            query.Select("t.*,t2.[TypeName] ParentTypeName").LeftJoin("BlogType t2 on t.[ParentId]=t2.[TypeId]").OrderBy("t.TypeId desc");
            if (param.IsDefautlt)
            {
                query.Where("t.IsDefautlt=@IsDefautlt", new { IsDefautlt = param.IsDefautlt });
            }
            if (param.ParentId.HasValue)
            {
                query.Where("t.ParentId=@ParentId", new { ParentId = param.ParentId.Value });
            }
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            return PageList(out total);
        }

        public List<BlogType> GetList(BlogTypeParam param)
        {
            query.Select("t.TypeId,t.TypeName");
            if (param.IsDefautlt)
            {
                query.Where("IsDefautlt=@IsDefautlt", new { IsDefautlt = param.IsDefautlt });
            }
            return QuerySql<BlogType>(query.RawSql, query.SqlParameters);
        }

        public bool Delete(int typeId)
        {
            return Delete(new BlogType() { TypeId = typeId });
        }

        public BlogType GetModel(int typeId)
        {
            return Get(typeId);
        }
    }
}

