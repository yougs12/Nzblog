using NZBlog.Entity;
using NZBlog.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NZBlog.Website.Models
{
    public class BlogRightViewModel
    {
        private static List<BlogType> _blogRecList;
        public List<BlogType> BlogRecList 
        {
            get { if (_blogRecList == null)_blogRecList = new BlogTypeProvider().GetDefaultList(); return _blogRecList; }
        }

        private static List<BlogDetail> _blogNewList;
        public List<BlogDetail> BlogNewList 
        {
            get { if (_blogNewList == null)_blogNewList = new BlogDetailProvider().GetNewList(); ; return _blogNewList; }
        }

        public static void RemoveData()
        {
            _blogRecList = null;
            _blogNewList = null;
        }

        public static void UpdateReadTimes(int id)
        {
            if (_blogNewList != null)
            {
                var model = _blogNewList.Find(b => b.BlogId == id);
                if (model != null)
                {
                    model.ReadTimes++;
                }
            }
        }
    }
}