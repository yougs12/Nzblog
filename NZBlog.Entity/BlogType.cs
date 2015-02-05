using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Dapper;

namespace NZBlog.Entity
{
    //BlogType
    public class BlogType
    {
        [Key]
        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public int ParentId { get; set; }

        [Computed]
        public string ParentTypeName { get; set; }

        public bool IsDefautlt { get; set; }
    }

    public class BlogTypeParam
    {
        public int? ParentId { get; set; }
        public bool IsDefautlt { get; set; }
    }
}

