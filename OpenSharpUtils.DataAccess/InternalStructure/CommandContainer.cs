using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.DataAccess.InternalStructure
{
    [Serializable]
    internal class CommandContainer<C, P, R> where C : IDbCommand, new() where P : IDataParameter, new() where R : IDataReader
    {
        #region >> Fields <<

        private readonly Func<C, Task<R>> queryAsyncFunc;
        private readonly Func<C, Task> nonQueryAsyncFunc;
        private readonly Func<C, Task<object>> scalarAsyncFunc;
        private string commandString;
        private string[] parameterDefinitions;
        private bool isProcedure;

        #endregion >> Fields <<

        #region >> Properties <<

        #endregion >> Properties <<

        #region >> CTOR <<

        public CommandContainer(string commandString, bool isProcedure, Func<C, Task<R>> queryAsyncFunc, Func<C, Task> nonQueryAsyncFunc, Func<C, Task<object>> scalarAsyncFunc, params string[] parameterDefinitions)
        {
            this.commandString = commandString;
            this.isProcedure = isProcedure;
            this.parameterDefinitions = parameterDefinitions;
        }

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public IDataReader InvokeReader(IDbConnection connection, params object[] parameters)
        {
            return CreateCommand(connection, parameters).ExecuteReader();
        }

        public async Task<IDataReader> InvokeReaderAsync(IDbConnection connection, params object[] parameters)
        {
            if (queryAsyncFunc != null)
            {
                return await queryAsyncFunc(CreateCommand(connection, parameters));
            }
            else
            {
                return await Task.Run(() => { return CreateCommand(connection, parameters).ExecuteReader(); });
            }
        }

        public void Invoke(IDbConnection connection, params object[] parameters)
        {
            CreateCommand(connection, parameters).ExecuteNonQuery();
        }

        public Task InvokeAsync(IDbConnection connection, params object[] parameters)
        {
            if (nonQueryAsyncFunc != null)
            {
                return nonQueryAsyncFunc(CreateCommand(connection, parameters));
            }
            else
            {
                return Task.Run(() => { CreateCommand(connection, parameters).ExecuteNonQuery(); });
            }
        }

        public object InvokeScalar(IDbConnection connection, params object[] parameters)
        {
            return CreateCommand(connection, parameters).ExecuteScalar();
        }

        public Task<object> InvokeScalarAsync(IDbConnection connection, params object[] parameters)
        {
            if (scalarAsyncFunc != null)
            {
                return scalarAsyncFunc(CreateCommand(connection, parameters));
            }
            else
            {
                return Task.Run(() => { return CreateCommand(connection, parameters).ExecuteScalar(); });
            }
        }

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        private C CreateCommand(IDbConnection connection, params object[] parameters)
        {
            C command = new C();
            command.Connection = connection;
            command.CommandText = commandString;
            command.CommandType = isProcedure ? System.Data.CommandType.StoredProcedure : System.Data.CommandType.Text;
            for (int i = 0; i < parameters.Length; i++)
            {
                command.Parameters.Add(new P() { ParameterName = parameterDefinitions[i], Value = parameters[i] ?? DBNull.Value });
            }
            return command;
        }

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        #endregion >> Events <<
    }
}
