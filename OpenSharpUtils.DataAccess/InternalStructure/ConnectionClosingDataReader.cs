﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenSharpUtils.DataAccess.InternalStructure
{
    public class ConnectionClosingDataReader : IDataReader
    {

        #region >> Fields <<

        private readonly IDataReader reader;
        private readonly IDbConnection connection;

        #endregion >> Fields <<

        #region >> Properties <<

        public object this[int i] => reader[i];

        public object this[string name] => reader[name];

        public int Depth => reader.Depth;

        public bool IsClosed => reader.IsClosed;

        public int RecordsAffected => reader.RecordsAffected;

        public int FieldCount => reader.FieldCount;

        #endregion >> Properties <<

        #region >> CTOR <<

        public ConnectionClosingDataReader(IDataReader reader, IDbConnection connection)
        {
            this.reader = reader;
            this.connection = connection;
        }

        #endregion >> CTOR <<

        #region >> Commands <<

        #endregion >> Commands <<

        #region >> Public Methods <<

        public void Close()
        {
            reader.Close();
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        public void Dispose()
        {
            reader.Dispose();
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        public bool GetBoolean(int i)
        {
            return reader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return reader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return reader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return reader.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return reader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return reader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return reader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return reader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return reader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return reader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return reader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return reader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return reader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return reader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return reader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return reader.GetOrdinal(name);
        }

        public DataTable GetSchemaTable()
        {
            return reader.GetSchemaTable();
        }

        public string GetString(int i)
        {
            return reader.GetString(i);
        }

        public object GetValue(int i)
        {
            return reader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return reader.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return reader.IsDBNull(i);
        }

        public bool NextResult()
        {
            return reader.NextResult();
        }

        public bool Read()
        {
            return reader.Read();
        }

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        #endregion >> Events <<
    }
}
