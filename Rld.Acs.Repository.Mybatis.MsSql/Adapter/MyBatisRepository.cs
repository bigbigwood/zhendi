using System;
using System.Data;
using log4net;
using Rld.Acs.Repository.Framework;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using System.Collections.Generic;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public abstract class MyBatisRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        protected ISqlMapper _sqlMapper;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MyBatisRepository()
        {
            SessionToPersistanceAdapter session = RepositoryManager.GetConnection() as SessionToPersistanceAdapter;
            _sqlMapper = session.GetUndelayingSession();
        }

        /// <summary>  
        /// 新增  
        /// </summary>  
        protected abstract string InsertStatement { get; }
        /// <summary>  
        /// 编辑  
        /// </summary>  
        protected abstract string UpdateStatement { get; }
        /// <summary>  
        /// 删除  
        /// </summary>  
        protected abstract string DeleteStatement { get; }
        /// <summary>  
        /// 单查  
        /// </summary>  
        protected abstract string GetByKeyStatement { get; }
        /// <summary>  
        /// Count  
        /// </summary>  
        protected abstract string QueryCountStatement { get; }
        /// <summary>  
        /// Full Query  
        /// </summary>  
        protected abstract string QueryStatement { get; }

        public virtual TEntity Insert(TEntity entity)
        {
            _sqlMapper.Insert(this.InsertStatement, entity);
            return entity;
        }

        public virtual bool Update(TEntity entity)
        {
            return _sqlMapper.Update(this.UpdateStatement, entity) > 0;
        }

        public virtual bool Delete(TKey key)
        {
            return _sqlMapper.Delete(this.DeleteStatement, key) > 0;
        }

        public virtual TEntity GetByKey(TKey key)
        {
            return _sqlMapper.QueryForObject<TEntity>(this.GetByKeyStatement, key);
        }

        public virtual IEnumerable<TEntity> Query(TEntity entityCondition)
        {
            return _sqlMapper.QueryForList<TEntity>(this.QueryStatement, entityCondition);
        }

        public DataSet SelectDS(string statementName, object paramObject)
        {
            DataSet ds = new DataSet();
            try
            {
                IMappedStatement statement = _sqlMapper.GetMappedStatement(statementName);
                RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, _sqlMapper.LocalSession);
                statement.PreparedCommand.Create(scope, _sqlMapper.LocalSession, statement.Statement, paramObject);

                IDbCommand command = _sqlMapper.LocalSession.CreateCommand(CommandType.Text);
                command.CommandText = scope.IDbCommand.CommandText;
                Log.Info(scope.IDbCommand.CommandText);

                _sqlMapper.LocalSession.CreateDataAdapter(command).Fill(ds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }
    }
}