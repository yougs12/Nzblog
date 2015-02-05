using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NZBlog.Entity;
using NZBlog.Factory;
using NZBlog.Service;

namespace NZBlog.Provider
{
    public class BlogTypeProvider
    {
        private IBlogType blogType = new IFactory().CreateBlogType();

        public List<BlogType> GetPageList(BlogTypeParam param, int pageIndex, int pageSize, out int total)
        {
            return blogType.GetList(param, pageIndex, pageSize, out total);
        }

        public BlogType GetBlogType(int blogId)
        {
            return blogType.GetModel(blogId);
        }

        public int AddBlogType(BlogType model)
        {
            return blogType.Insert(model);
        }

        public bool UpdateBlogType(BlogType model)
        {
            return blogType.Update(model);
        }

        public bool DeleteBlogType(int id)
        {
            return blogType.Delete(id);
        }

        public List<BlogType> GetDefaultList()
        {
            return blogType.GetList(new BlogTypeParam { IsDefautlt = true });
        }
    }
}
