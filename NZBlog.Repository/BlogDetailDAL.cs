using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using Dapper;
using NZBlog.Entity;
using NZBlog.Service;

namespace NZBlog.Repository 
{
	 	//BlogDetail
    public partial class BlogDetailDAL : BaseDAL<BlogDetail>, IBlogDetail
    {
        public BlogDetailDAL()
        {
            query.TableName = "BlogDetail";
        }

        public List<BlogDetail> GetList(BlogDetailParam param, int pageIndex, int pageSize, out int total)
        {
            query.Select("t.BlogId,t.TypeId,t.Title,t.ReadTimes,t.IsSendDefault,t.CreatTimes,t.IsRec,t.SortNum,t1.TypeName").Join("BlogType t1 on t.TypeId=t1.TypeId").OrderBy("IsRec desc,SortNum desc,BlogId desc");
            if (param.TypeId != 0)
            {
                query.Where("t.TypeId=@TypeId", new { TypeId = param.TypeId });
            }
            
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            return PageList(out total);
        }

        public List<BlogDetail> GetNewList()
        {
            query.Select("top 10 BlogId,Title,ReadTimes,CreatTimes").Where("IsSendDefault=1").OrderBy("BlogId desc");
            return QuerySql<BlogDetail>(query.RawSql);
        }

        public List<BlogDetail> GetBlogList(BlogDetailParam param, int pageIndex, int pageSize, out int total)
        {
            query.Select("t.BlogId,t.TypeId,t.Title,t.BlogContent,t.ReadTimes,t.IsSendDefault,t.CreatTimes,t.IsRec,t.SortNum,t1.TypeName").Join("BlogType t1 on t.TypeId=t1.TypeId").LeftJoin("BlogType t2 on t2.TypeId=t1.ParentId").OrderBy("IsRec desc,SortNum desc,BlogId desc");
            if (param.TypeId != 0)
            {
                query.Where("t.TypeId=@TypeId or t2.TypeId=@TypeId", new { TypeId = param.TypeId });
            }

            if (!string.IsNullOrWhiteSpace(param.Title))
            {
                query.Where("Title like @Title", new { Title = "%" + param.Title + "%" });
            }

            query.PageIndex = pageIndex;
            query.PageSize = pageSize;
            return PageList(out total);
        }

        public List<BlogDetail> GetRssBlog()
        {
            query.Select(" top 10 t.BlogId,t.TypeId,t.Title,t.BlogContent,t.ReadTimes,t.IsSendDefault,t.CreatTimes,t.IsRec,t.SortNum,t1.TypeName").Join("BlogType t1 on t.TypeId=t1.TypeId").Where("IsSendDefault=1").OrderBy("BlogId desc");
            return QuerySql<BlogDetail>(query.RawSql, query.SqlParameters);
        }

        public bool Delete(int blogId)
        {
            return Delete(new BlogDetail() {BlogId = blogId});
        }

        public BlogDetail GetModel(int blogId)
        {
            return Get(blogId);
        }

        public List<BlogDetail> GetList(BlogDetailParam param)
        {
            query.Select("t.BlogId,t.TypeId,t.Title,t.ReadTimes,t.IsSendDefault,t.CreatTimes,t.IsRec,t.SortNum,t1.TypeName").Join("BlogType t1 on t.TypeId=t1.TypeId");
            if (param.TypeId != 0)
            {
                query.Where("TypeId=@TypeId", new {TypeId = param.TypeId});
            }
            if (param.IsRec)
            {
                query.Where("IsRec=@IsRec", new { IsRec = param.IsRec });
            }
            query.OrderBy("SortNum desc,CreatTimes desc");
            return QuerySql<BlogDetail>(query.RawSql, query.SqlParameters);
        }

        public override bool Update(BlogDetail model)
        {
            query.Update("TypeId,Title,BlogContent,IsSendDefault,IsRec,SortNum", model).Where("BlogId=@BlogId");
            return ExcuteQuery(query.RawSql, query.SqlParameters) > 0;
        }

        public bool UpdateReadTimes(int blogId)
        {
            return ExcuteQuery("update BlogDetail set ReadTimes=ReadTimes+1 where BlogId=@BlogId", new { BlogId = blogId }) > 0;
        }
    }
}

