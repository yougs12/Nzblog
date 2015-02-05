using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper
{
    public static class SqlBuilderExtensoin
    {
        public static SqlBuilder Where(this SqlBuilder sqlBuilder, string sql, int seq, object parameters = null)
        {
            sqlBuilder.AddClause("where" + seq, sql, parameters, " AND ", prefix: "WHERE ", postfix: "\n");
            return sqlBuilder;
        }

        
    }
}
