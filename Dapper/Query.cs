using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Dapper
{
    public class Query
    {
        private SqlBuilder _sqlBuilder = new SqlBuilder();

        public SqlBuilder SqlBuilder { get { return _sqlBuilder; } }

        public string TableName { get; set; }

        public DynamicParameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        private string rawSql = string.Empty;

        public Query Select(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**select**/ "))
            {
                rawSql = "select /**select**/ from " + TableName + " t ";
            }
            _sqlBuilder.Select(sql, parameters);
            return this;
        }

        public Query Insert(string sql, dynamic paramerters = null)
        {
            string[] insertField = sql.Split(',');
            var fields = insertField.Select(s => "@" + s);
            string strfields = string.Join(",", fields.ToArray());
            rawSql = "Insert into " + TableName + " ( " + sql + " ) values (" + strfields + ")";
            _sqlBuilder.AddParameters(paramerters);
            return this;
        }

        public Query Update(string sql, dynamic paramerters = null)
        {
            string[] insertField = sql.Split(',');
            StringBuilder sb = new StringBuilder("");
            foreach (var field in insertField)
            {
                sb.AppendFormat(",{0}=@{0} ", field);
            }
            string setSql = sb.ToString().Substring(1);
            rawSql = "update " + TableName + " set " + setSql;
            _sqlBuilder.AddParameters(paramerters);
            return this;
        }

        public Query Delete(dynamic paramerters = null)
        {
            rawSql = "delete " + TableName + " ";
            _sqlBuilder.AddParameters(paramerters);
            return this;
        }

        public Query Intersect(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**intersect**/ "))
            {
                rawSql += " /**intersect**/ ";
            }
            _sqlBuilder.Intersect(sql, parameters);
            return this;
        }

        public Query InnerJoin(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**innerjoin**/ "))
            {
                rawSql += " /**innerjoin**/ ";
            }
            _sqlBuilder.InnerJoin(sql, parameters);
            return this;
        }

        public Query LeftJoin(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**leftjoin**/ "))
            {
                rawSql += " /**leftjoin**/ ";
            }
            _sqlBuilder.LeftJoin(sql, parameters);
            return this;
        }

        public Query RightJoin(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**rightjoin**/ "))
            {
                rawSql += " /**rightjoin**/ ";
            }
            _sqlBuilder.RightJoin(sql, parameters);
            return this;
        }

        public Query Where(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**where**/ "))
            {
                rawSql += " /**where**/ ";
            }
            _sqlBuilder.Where(sql, parameters);
            return this;
        }

        public Query AddWhere(string sql, object parameters = null)
        {
            seq++;
            if (!rawSql.Contains(" /**where" + seq + "**/ "))
            {
                rawSql += " /**where" + seq + "**/ ";
            }
            _sqlBuilder.Where(sql, seq, parameters);
            return this;
        }

        public Query OrWhere(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**where**/ "))
            {
                rawSql += " /**where**/ ";
            }
            _sqlBuilder.OrWhere(sql, parameters);
            return this;
        }

        public Query OrderBy(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**orderby**/ "))
            {
                rawSql += " /**orderby**/ ";
            }
            _sqlBuilder.OrderBy(sql, parameters);
            return this;
        }

        public Query AddParameters(dynamic parameters)
        {
            _sqlBuilder.AddParameters(parameters);
            return this;
        }

        public Query Join(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**join**/ "))
            {
                rawSql += " /**join**/ ";
            }
            _sqlBuilder.Join(sql, parameters);
            return this;
        }

        public Query GroupBy(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**groupby**/ "))
            {
                rawSql += " /**groupby**/ ";
            }
            _sqlBuilder.GroupBy(sql, parameters);
            return this;
        }

        public Query Having(string sql, dynamic parameters = null)
        {
            if (!rawSql.Contains(" /**having**/ "))
            {
                rawSql += " /**having**/ ";
            }
            _sqlBuilder.Having(sql, parameters);
            return this;
        }
       
        public Query SetPage()
        {
            int minNum = _pageSize*(_pageIndex - 1);
            int maxNum = _pageIndex*_pageSize + 1;
            rawSql = rawSql.Replace(" /**orderby**/ ", "");
            this.Select(" row_number() over (/**orderby**/) as rowNum");
            rawSql = "select * from (" + rawSql + ") a where rowNum>@minNum and rowNum<@maxNum";
            if (rawSql.Contains(" /**where**/ "))
            {
                rawSql += "\nselect @cnt= count(1) from " + TableName + " t /**where**/ ";
            }
            else
            {
                rawSql += "\nselect @cnt= count(1) from " + TableName + " ";
            }
            _parameters.Add("@minNum", minNum, DbType.Int32);
            _parameters.Add("@maxNum", maxNum, DbType.Int32);
            _parameters.Add("@cnt", dbType: DbType.Int32, direction: ParameterDirection.Output);
            _sqlBuilder.AddParameters(_parameters);
            return this;
        }

        public string RawSql
        {
            get
            {
                var template = _sqlBuilder.AddTemplate(rawSql);
                _sqlParameters = template.Parameters;
                rawSql = template.RawSql;
                return rawSql;
            }
        }

        private int seq = 0;
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private DynamicParameters _parameters = new DynamicParameters();
        private object _sqlParameters;

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value > 0)
                    _pageIndex = value;
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value > 0)
                    _pageSize = value;
            }
        }

        public object SqlParameters
        {
            get { return _sqlParameters; }
            set { _sqlParameters = value; }
        }
    }
}
