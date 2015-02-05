using System;
using Dapper;

namespace NZBlog.Entity
{
    public class BlogDetail
    {
        [Key]
        public int BlogId { get; set; }

        public int TypeId { get; set; }

        public string Title { get; set; }

        public string BlogContent { get; set; }

        public int CreatUserId { get; set; }

        public int ReadTimes { get; set; }

        public bool IsSendDefault { get; set; }

        public DateTime CreatTimes { get; set; }

        public bool IsRec { get; set; }

        public int SortNum { get; set; }

        public string TypeName { get; set; }

        [Computed]
        public string Lables { get; set; }
    }

    public class BlogDetailParam
    {
        public int TypeId { get; set; }

        public bool IsRec { get; set; }

        public string Title { get; set; }
    }
}