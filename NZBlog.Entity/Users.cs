using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Dapper;

namespace NZBlog.Entity
{
    //Users
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public string PassWord { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime CreatTime { get; set; }

        public string ReMark { get; set; }

        [Computed]
        public string ValiCode { get; set; }
    }

    public class UsersParam
    {
        public string UserName { get; set; }
    }
}

