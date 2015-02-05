using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Dapper;

namespace NZBlog.Entity
{
    public class ZInfoMation
    {
        [Key]
        public int ZId { get; set; }
        public string ZCode { get; set; }

        public string ZTitle { get; set; }

        public string ZContent { get; set; }
    }

    public class ZInfoMationParam
    {
        
    }
}

