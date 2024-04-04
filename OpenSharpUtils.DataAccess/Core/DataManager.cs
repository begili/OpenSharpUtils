using OpenSharpUtils.DataAccess.DataAttributes;
using OpenSharpUtils.DataAccess.InternalStructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.DataAccess.Core
{
    [Serializable]
    public class DataManager<S, C, P, R> where S : IDbConnection, new() where C : IDbCommand, new() where P : IDataParameter, new() where R : IDataReader
    {

        #region >> Fields <<

        private readonly Func<C, Task<R>> queryAsyncFunc;
        private readonly Func<C, Task> nonQueryAsyncFunc;
        private readonly Func<C, Task<object>> scalarAsyncFunc;

        private Dictionary<Type, object> reflectionSetters;

        #endregion >> Fields <<

        #region >> Properties <<

        public string ConnectionString { get; private set; }

        #endregion >> Properties <<

        #region >> CTOR <<

        public DataManager()
        {
            this.reflectionSetters = new Dictionary<Type, object>();
        }

        public DataManager(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.reflectionSetters = new Dictionary<Type, object>();
        }

        public DataManager(Func<C, Task<R>> queryAsyncFunc, Func<C, Task> nonQueryAsyncFunc, Func<C, Task<object>> scalarAsyncFunc) : this()
        {
            this.queryAsyncFunc = queryAsyncFunc;
            this.nonQueryAsyncFunc = nonQueryAsyncFunc;
            this.scalarAsyncFunc = scalarAsyncFunc;
        }

        public DataManager(string connectionString, Func<C, Task<R>> queryAsyncFunc, Func<C, Task> nonQueryAsyncFunc, Func<C, Task<object>> scalarAsyncFunc) : this(connectionString)
        {
            this.queryAsyncFunc = queryAsyncFunc;
            this.nonQueryAsyncFunc = nonQueryAsyncFunc;
            this.scalarAsyncFunc = scalarAsyncFunc;
        }

        #endregion >> CTOR <<

        #region >> Public Methods <<

        #region >> Read-Methods <<

        public IDataReader ExecuteReader(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            S conn = new S() { ConnectionString = ConnectionString };
            try
            {
                conn.Open();
                using (C command = new C() { Connection = conn, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    return new ConnectionClosingDataReader(command.ExecuteReader(), conn);
                }
            }
            catch
            {
                conn.Close();
            }
            return null;
        }

        public IDataReader ExecuteReader(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { Connection = connection, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                return command.ExecuteReader();
            }
        }

        public async Task<IDataReader> ExecuteReaderAsync(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            S conn = new S() { ConnectionString = ConnectionString };
            try
            {
                conn.Open();
                using (C command = new C() { Connection = conn, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    return new ConnectionClosingDataReader(await ExecuteQueryAsync(command), conn);
                }
            }
            catch
            {
                conn.Close();
            }
            return null;
        }

        public async Task<IDataReader> ExecuteReaderAsync(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { Connection = connection, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                return await ExecuteQueryAsync(command);
            }
        }

        public ResultSet<R1> ExecuteReader<R1>(string commandString, bool isProcedure, params IDataParameter[] parameters) where R1 : new()
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                Type type = typeof(R1);
                using (C command = new C() { Connection = conn, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (!reflectionSetters.ContainsKey(type))
                        {
                            reflectionSetters.Add(type, new ReflectionSetter<R1>());
                        }
                        return new ResultSet<R1>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader));
                    }
                }
            }
        }

        public ResultSet<R1> ExecuteReader<R1>(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters) where R1 : new()
        {
            Type type = typeof(R1);
            using (C command = new C() { Connection = connection, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (!reflectionSetters.ContainsKey(type))
                    {
                        reflectionSetters.Add(type, new ReflectionSetter<R1>());
                    }
                    return new ResultSet<R1>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader));
                }
            }
        }

        public async Task<ResultSet<R1>> ExecuteReaderAsync<R1>(string commandString, bool isProcedure, params IDataParameter[] parameters) where R1 : new()
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                Type type = typeof(R1);
                using (C command = new C() { Connection = conn, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    using (IDataReader reader = await ExecuteQueryAsync(command))
                    {
                        if (!reflectionSetters.ContainsKey(type))
                        {
                            reflectionSetters.Add(type, new ReflectionSetter<R1>());
                        }
                        return new ResultSet<R1>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader));
                    }
                }
            }
        }

        public async Task<ResultSet<R1>> ExecuteReaderAsync<R1>(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters) where R1 : new()
        {
            Type type = typeof(R1);
            using (C command = new C() { Connection = connection, CommandText = commandString, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                using (IDataReader reader = await ExecuteQueryAsync(command))
                {
                    if (!reflectionSetters.ContainsKey(type))
                    {
                        reflectionSetters.Add(type, new ReflectionSetter<R1>());
                    }
                    return new ResultSet<R1>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader));
                }
            }
        }

        public ResultSet<R1, R2> ExecuteReader<R1, R2>(string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new()
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                Type type = typeof(R1);
                Type type2 = typeof(R2);
                using (C command = new C() { CommandText = commandString, Connection = conn })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (!reflectionSetters.ContainsKey(type))
                        {
                            reflectionSetters.Add(type, new ReflectionSetter<R1>());
                        }
                        if (!reflectionSetters.ContainsKey(type2))
                        {
                            reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                        }
                        return new ResultSet<R1, R2>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader));
                    }
                }
            }
        }

        public ResultSet<R1, R2> ExecuteReader<R1, R2>(S connection, string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new()
        {
            Type type = typeof(R1);
            Type type2 = typeof(R2);
            using (C command = new C() { CommandText = commandString, Connection = connection })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (!reflectionSetters.ContainsKey(type))
                    {
                        reflectionSetters.Add(type, new ReflectionSetter<R1>());
                    }
                    if (!reflectionSetters.ContainsKey(type2))
                    {
                        reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                    }
                    return new ResultSet<R1, R2>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader));
                }
            }
        }

        public async Task<ResultSet<R1, R2>> ExecuteReaderAsync<R1, R2>(string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new()
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                Type type = typeof(R1);
                Type type2 = typeof(R2);
                using (C command = new C() { CommandText = commandString, Connection = conn })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    using (IDataReader reader = await ExecuteQueryAsync(command))
                    {
                        if (!reflectionSetters.ContainsKey(type))
                        {
                            reflectionSetters.Add(type, new ReflectionSetter<R1>());
                        }
                        if (!reflectionSetters.ContainsKey(type2))
                        {
                            reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                        }
                        return new ResultSet<R1, R2>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader));
                    }
                }
            }
        }

        public async Task<ResultSet<R1, R2>> ExecuteReaderAsync<R1, R2>(S connection, string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new()
        {
            Type type = typeof(R1);
            Type type2 = typeof(R2);
            using (C command = new C() { CommandText = commandString, Connection = connection })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                using (IDataReader reader = await ExecuteQueryAsync(command))
                {
                    if (!reflectionSetters.ContainsKey(type))
                    {
                        reflectionSetters.Add(type, new ReflectionSetter<R1>());
                    }
                    if (!reflectionSetters.ContainsKey(type2))
                    {
                        reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                    }
                    return new ResultSet<R1, R2>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader));
                }
            }
        }

        public ResultSet<R1, R2, R3> ExecuteReader<R1, R2, R3>(string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new() where R3 : new()
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                Type type = typeof(R1);
                Type type2 = typeof(R2);
                Type type3 = typeof(R3);
                using (C command = new C() { CommandText = commandString, Connection = conn })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (!reflectionSetters.ContainsKey(type))
                        {
                            reflectionSetters.Add(type, new ReflectionSetter<R1>());
                        }
                        if (!reflectionSetters.ContainsKey(type2))
                        {
                            reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                        }
                        if (!reflectionSetters.ContainsKey(type3))
                        {
                            reflectionSetters.Add(type3, new ReflectionSetter<R3>());
                        }
                        return new ResultSet<R1, R2, R3>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader), ((ReflectionSetter<R3>)reflectionSetters[type3]).CreateListFromReader(reader));
                    }
                }
            }
        }

        public ResultSet<R1, R2, R3> ExecuteReader<R1, R2, R3>(S connection, string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new() where R3 : new()
        {
            Type type = typeof(R1);
            Type type2 = typeof(R2);
            Type type3 = typeof(R3);
            using (C command = new C() { CommandText = commandString, Connection = connection })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (!reflectionSetters.ContainsKey(type))
                    {
                        reflectionSetters.Add(type, new ReflectionSetter<R1>());
                    }
                    if (!reflectionSetters.ContainsKey(type2))
                    {
                        reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                    }
                    if (!reflectionSetters.ContainsKey(type3))
                    {
                        reflectionSetters.Add(type3, new ReflectionSetter<R3>());
                    }
                    return new ResultSet<R1, R2, R3>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader), ((ReflectionSetter<R3>)reflectionSetters[type3]).CreateListFromReader(reader));
                }
            }
        }

        public async Task<ResultSet<R1, R2, R3>> ExecuteReaderAsync<R1, R2, R3>(string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new() where R3 : new()
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                Type type = typeof(R1);
                Type type2 = typeof(R2);
                Type type3 = typeof(R3);
                using (C command = new C() { CommandText = commandString, Connection = conn })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    using (IDataReader reader = await ExecuteQueryAsync(command))
                    {
                        if (!reflectionSetters.ContainsKey(type))
                        {
                            reflectionSetters.Add(type, new ReflectionSetter<R1>());
                        }
                        if (!reflectionSetters.ContainsKey(type2))
                        {
                            reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                        }
                        if (!reflectionSetters.ContainsKey(type3))
                        {
                            reflectionSetters.Add(type3, new ReflectionSetter<R3>());
                        }
                        return new ResultSet<R1, R2, R3>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader), ((ReflectionSetter<R3>)reflectionSetters[type3]).CreateListFromReader(reader));
                    }
                }
            }
        }

        public async Task<ResultSet<R1, R2, R3>> ExecuteReaderAsync<R1, R2, R3>(S connection, string commandString, params IDataParameter[] parameters) where R1 : new() where R2 : new() where R3 : new()
        {
            Type type = typeof(R1);
            Type type2 = typeof(R2);
            Type type3 = typeof(R3);
            using (C command = new C() { CommandText = commandString, Connection = connection })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                using (IDataReader reader = await ExecuteQueryAsync(command))
                {
                    if (!reflectionSetters.ContainsKey(type))
                    {
                        reflectionSetters.Add(type, new ReflectionSetter<R1>());
                    }
                    if (!reflectionSetters.ContainsKey(type2))
                    {
                        reflectionSetters.Add(type2, new ReflectionSetter<R2>());
                    }
                    if (!reflectionSetters.ContainsKey(type3))
                    {
                        reflectionSetters.Add(type3, new ReflectionSetter<R3>());
                    }
                    return new ResultSet<R1, R2, R3>(((ReflectionSetter<R1>)reflectionSetters[type]).CreateListFromReader(reader), ((ReflectionSetter<R2>)reflectionSetters[type2]).CreateListFromReader(reader), ((ReflectionSetter<R3>)reflectionSetters[type3]).CreateListFromReader(reader));
                }
            }
        }

        #endregion >> Read-Methods <<

        #region >> Execute-Methods <<

        public void ExecuteNonQuery(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ExecuteNonQuery(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                command.ExecuteNonQuery();
            }
        }

        public Task ExecuteNonQueryAsync(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    return ExecuteNonQueryAsync(command);
                }
            }
        }

        public Task ExecuteNonQueryAsync(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                return ExecuteNonQueryAsync(command);
            }
        }

        public T ExecuteNonQuery<T>(string commandString, DbType retvalType, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    P retPar = new P() { ParameterName = "@retvalgenerated", DbType = retvalType, Direction = ParameterDirection.ReturnValue };
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    command.Parameters.Add(retPar);
                    command.ExecuteNonQuery();
                    return (T)retPar.Value;
                }
            }
        }

        public T ExecuteNonQuery<T>(S connection, string commandString, DbType retvalType, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                P retPar = new P() { ParameterName = "@retvalgenerated", DbType = retvalType, Direction = ParameterDirection.ReturnValue };
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                command.Parameters.Add(retPar);
                command.ExecuteNonQuery();
                return (T)retPar.Value;
            }
        }

        public async Task<T> ExecuteNonQueryAsync<T>(string commandString, DbType retvalType, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    P retPar = new P() { ParameterName = "@retvalgenerated", DbType = retvalType, Direction = ParameterDirection.ReturnValue };
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    command.Parameters.Add(retPar);
                    await ExecuteNonQueryAsync(command);
                    return (T)retPar.Value;
                }
            }
        }

        public async Task<T> ExecuteNonQueryAsync<T>(S connection, string commandString, DbType retvalType, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                P retPar = new P() { ParameterName = "@retvalgenerated", DbType = retvalType, Direction = ParameterDirection.ReturnValue };
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                command.Parameters.Add(retPar);
                await ExecuteNonQueryAsync(command);
                return (T)retPar.Value;
            }
        }

        public object ExecuteScalar(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    return command.ExecuteScalar();
                }
            }
        }

        public object ExecuteScalar(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                return command.ExecuteScalar();
            }
        }

        public Task<object> ExecuteScalarAsync(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    return ExecuteScalarAsync(command);
                }
            }
        }

        public Task<object> ExecuteScalarAsync(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                return ExecuteScalarAsync(command);
            }
        }

        public T ExecuteScalar<T>(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    var ret = command.ExecuteScalar();
                    return (T)ret;
                }
            }
        }

        public T ExecuteScalar<T>(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                var ret = command.ExecuteScalar();
                return (T)ret;
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (S conn = new S() { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (C command = new C() { CommandText = commandString, Connection = conn, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
                {
                    foreach (var item in parameters)
                    {
                        command.Parameters.Add(item);
                    }
                    var ret = await ExecuteScalarAsync(command);
                    return (T)ret;
                }
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(S connection, string commandString, bool isProcedure, params IDataParameter[] parameters)
        {
            using (C command = new C() { CommandText = commandString, Connection = connection, CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item);
                }
                var ret = await ExecuteScalarAsync(command);
                return (T)ret;
            }
        }

        #endregion >> Execute-Methods <<

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        private async Task<IDataReader> ExecuteQueryAsync(C command)
        {
            if (queryAsyncFunc != null)
            {
                return (await queryAsyncFunc(command));
            }
            else
            {
                return await Task.Run(() => { return command.ExecuteReader(); });
            }
        }

        private Task ExecuteNonQueryAsync(C command)
        {
            if (nonQueryAsyncFunc != null)
            {
                return nonQueryAsyncFunc(command);
            }
            else
            {
                return Task.Run(() => { command.ExecuteNonQuery(); });
            }
        }

        private Task<object> ExecuteScalarAsync(C command)
        {
            if (scalarAsyncFunc != null)
            {
                return scalarAsyncFunc(command);
            }
            else
            {
                return Task.Run(() => { return command.ExecuteScalar(); });
            }
        }

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        #endregion >> Events <<

    }
}
