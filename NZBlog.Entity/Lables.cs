using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Dapper;

namespace NZBlog.Entity
{
    //Lables
    public class Lables
    {
        [Key]
        public int LabelId { get; set; }

        public int BlogId { get; set; }

        public string LabName { get; set; }
    }

    public class LablesParam
    {
        public int BlogId { get; set; }
    }
}

