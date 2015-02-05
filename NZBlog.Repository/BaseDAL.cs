using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using NZBlog.Entity;

namespace NZBlog.Repository
{
    public class BaseDAL<T> where T : class
    {
        //protected string tableName = "";
        //protected SqlBuilder sqlBuilder = new SqlBuilder();
        //protected string preSql = "";
        protected Query query = new Query();
        protected IDbConnection GetOpenConnection()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }
        public IDbDataAdapter CreateAdapter(IDbCommand comm)
        {
            SqlDataAdapter sda = new SqlDataAdapter((SqlCommand)comm);
            return sda;
        }

        public virtual int Insert(T model)
        {
            using (var connection = GetOpenConnection())
            {
                long id = connection.Insert(model);
                return (int) id;
            }
        }

        public virtual bool Update(T model)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Update(model);
            }
        }

        public bool Delete(T model)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Delete(model);
            }
        }

        public T Get(object id)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Get<T>(id);
            }
        }

        public int ExcuteQuery(string querySql, object parameters = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Execute(querySql, parameters);
            }
        }

        public int ExcuteQueryProc(string querySql, object parameters = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Execute(querySql, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IDataReader ExecuteReader(string querySql, object parameters = null)
        {
            var connection = GetOpenConnection();
            return connection.ExecuteReader(querySql, parameters);
        }

        public object ExecuteScalar(string querySql, object parameters = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.ExecuteScalar(querySql, parameters);
            }
        }

        public TResult ExecuteScalar<TResult>(string querySql, object parameters = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.ExecuteScalar<TResult>(querySql, parameters);
            }
        }

        public List<TResult> QuerySql<TResult>(string querySql, object parameters = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Query<TResult>(querySql, parameters).ToList();
            }
        }

        public List<TResult> QueryProc<TResult>(string querySql, object parameters = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Query<TResult>(querySql, parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public DataTable QueryTable(string querySql, params IDbDataParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (var connection = GetOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = querySql;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    IDbDataAdapter dat = CreateAdapter(command);
                    dat.Fill(ds);
                }
            }
            return ds.Tables[0];
        }

        public List<T> PageList(out int recordCount)
        {
            query.SetPage();
            var result = QuerySql<T>(query.RawSql, query.SqlParameters);
            recordCount = query.Parameters.Get<int>("@cnt");
            return result;
        }
    }
}
