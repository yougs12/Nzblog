using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NZBlog.Repository;
using NZBlog.Service;

namespace NZBlog.Factory
{
    public class IFactory
    {
        public IBlogDetail CreateBlogDetail()
        {
            return new BlogDetailDAL();
        }

        public IBlogType CreateBlogType()
        {
            return new BlogTypeDAL();
        }

        public ILables CreateLables()
        {
            return new LablesDAL();
        }

        public IUsers CreateUsers()
        {
            return new UsersDAL();
        }

        public IZInfoMation CreateZInfoMation()
        {
            return new ZInfoMationDAL();
        }
    }
}
