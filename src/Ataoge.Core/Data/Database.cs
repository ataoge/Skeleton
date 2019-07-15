using System;
using System.Data;
using System.Data.Common;

namespace Ataoge.Data
{
    public class Database
    {
        public Database(string connectionString, DbProviderFactory factory)
        {
            this.ConnectionString = connectionString;
            this.Factory = factory;
        }

        public string ConnectionString {get;}

        public DbProviderFactory Factory {get;}

        /// <summary>
        /// 生成数据库连接
        /// </summary>
        /// <returns>数据库连接<see cref="DbConnection"/></returns>        
        public DbConnection CreateConnection()
        {
            return CreateConnection(true);
        }

        /// <summary>
        /// 生成数据库连接
        /// </summary>
        /// <param name="opened">是否打开连接</param>
        /// <returns>数据库连接<see cref="DbConnection"/></returns>
        protected virtual DbConnection CreateConnection(bool opened)
        {
            DbConnection connection = null;
            try
            {
                try
                {
                    connection = Factory.CreateConnection();
                    connection.ConnectionString = ConnectionString;
                    if (opened)
                        connection.Open();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            catch
            {
                try
                {
                    if (connection != null)
                        connection.Close();
                }
                catch
                {
                }
                throw;
            }
            return connection;
        }

        internal DbConnection GetNewOpenConnection()
        {
            DbConnection connection = null;
            try
            {
                try
                {
                    connection = CreateConnection(false);
                    connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch
            {
                if (connection != null)
                    connection.Close();

                throw;
            }

            return connection;
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <param name="connection">事务的连接<see cref="DbConnection"/></param>
        /// <returns> 开始的事务</returns>
        public DbTransaction BeginTransaction(DbConnection connection)
        {
            DbTransaction tran = connection.BeginTransaction();
            return tran;
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns> 开始的事务</returns>
        public DbTransaction BeginTransaction(DbConnection connection, IsolationLevel isolateLevel)
        {
            DbTransaction tran = connection.BeginTransaction(isolateLevel);
            return tran;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="tran">事务</param>
        public void Commit(DbTransaction tran)
        {
            tran.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="tran">事务<see cref="IDbTransaction"/></param>
        public void Rollback(DbTransaction tran)
        {
            tran.Rollback();
        }
    }
}