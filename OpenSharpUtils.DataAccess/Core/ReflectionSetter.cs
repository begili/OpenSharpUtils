using OpenSharpUtils.DataAccess.DataAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenSharpUtils.DataAccess.Core
{
    [Serializable]
    public class ReflectionSetter<T> where T : new()
    {
        #region >> Fields <<

        private const int BOOL_INDEX = 0;
        private const int BOOLN_INDEX = 1;
        private const int BYTE_INDEX = 2;
        private const int BYTEN_INDEX = 3;
        private const int SHORT_INDEX = 4;
        private const int SHORTN_INDEX = 5;
        private const int INT_INDEX = 6;
        private const int INTN_INDEX = 7;
        private const int LONG_INDEX = 8;
        private const int LONGN_INDEX = 9;
        private const int FLOAT_INDEX = 10;
        private const int FLOATN_INDEX = 11;
        private const int DOUBLE_INDEX = 12;
        private const int DOUBLEN_INDEX = 13;
        private const int DECIMAL_INDEX = 14;
        private const int DECIMALN_INDEX = 15;
        private const int CHAR_INDEX = 16;
        private const int CHARN_INDEX = 17;
        private const int STRING_INDEX = 18;
        private const int DATETIME_INDEX = 19;
        private const int DATETIMEN_INDEX = 20;
        private const int BYTEARR_INDEX = 21;
        private const int TIMESPAN_INDEX = 22;
        private const int TIMESPANN_INDEX = 23;
        private const int GUID_INDEX = 24;
        private const int GUIDN_INDEX = 25;

        private Dictionary<string, int> nameToIndexMapping;

        private int[] indexToTypeMapping;
        private Action<T, bool>[] boolAccessors;
        private Action<T, bool?>[] boolNAccessors;
        private Action<T, byte>[] byteAccessors;
        private Action<T, byte?>[] byteNAccessors;
        private Action<T, short>[] shortAccessors;
        private Action<T, short?>[] shortNAccessors;
        private Action<T, int>[] intAccessors;
        private Action<T, int?>[] intNAccessors;
        private Action<T, long>[] longAccessors;
        private Action<T, long?>[] longNAccessors;
        private Action<T, float>[] floatAccessors;
        private Action<T, float?>[] floatNAccessors;
        private Action<T, double>[] doubleAccessors;
        private Action<T, double?>[] doubleNAccessors;
        private Action<T, decimal>[] decimalAccessors;
        private Action<T, decimal?>[] decimalNAccessors;
        private Action<T, char>[] charAccessors;
        private Action<T, char?>[] charNAccessors;
        private Action<T, string>[] stringAccessors;
        private Action<T, DateTime>[] datetimeAccessors;
        private Action<T, DateTime?>[] datetimeNAccessors;
        private Action<T, byte[]>[] bytearrAccessors;
        private Action<T, TimeSpan>[] timespanAccessors;
        private Action<T, TimeSpan?>[] timespanNAccessors;
        private Action<T, Guid>[] guidAccessors;
        private Action<T, Guid?>[] guidNAccessors;

        private bool isInitialized;

        #endregion >> Fields <<

        #region >> Properties <<

        #endregion >> Properties <<

        #region >> CTOR <<

        public ReflectionSetter()
        {
            nameToIndexMapping = new Dictionary<string, int>();
        }

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public IEnumerable<T> CreateListFromReader(IDataReader reader, bool jumpToNextReader = true)
        {
            BuildupType();
            List<T> ret = new List<T>();
            int?[] ordinalMapping = new int?[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                if (nameToIndexMapping.ContainsKey(name))
                {
                    ordinalMapping[i] = nameToIndexMapping[name];
                }
            }
            while (reader.Read())
            {
                T instance = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (!ordinalMapping[i].HasValue)
                    {
                        continue;
                    }
                    SetValue(instance, ordinalMapping[i].Value, reader, i);
                }
                ret.Add(instance);
            }
            if (jumpToNextReader)
            {
                reader.NextResult();
            }
            return ret;
        }

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        private void SetValue(T instance, int index, IDataReader reader, int column)
        {
            switch (indexToTypeMapping[index])
            {
                case BOOL_INDEX:
                    boolAccessors[index](instance, reader.GetBoolean(column));
                    break;
                case BOOLN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        boolNAccessors[index](instance, reader.GetBoolean(column));
                    }
                    else
                    {
                        boolNAccessors[index](instance, (bool?)null);
                    }
                    break;
                case BYTE_INDEX:
                    byteAccessors[index](instance, reader.GetByte(column));
                    break;
                case BYTEN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        byteNAccessors[index](instance, reader.GetByte(column));
                    }
                    else
                    {
                        byteNAccessors[index](instance, (byte?)null);
                    }
                    break;
                case SHORT_INDEX:
                    shortAccessors[index](instance, reader.GetInt16(column));
                    break;
                case SHORTN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        shortNAccessors[index](instance, reader.GetInt16(column));
                    }
                    else
                    {
                        shortNAccessors[index](instance, (short?)null);
                    }
                    break;
                case INT_INDEX:
                    intAccessors[index](instance, reader.GetInt32(column));
                    break;
                case INTN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        intNAccessors[index](instance, reader.GetInt32(column));
                    }
                    else
                    {
                        intNAccessors[index](instance, (int?)null);
                    }
                    break;
                case LONG_INDEX:
                    longAccessors[index](instance, reader.GetInt64(column));
                    break;
                case LONGN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        longNAccessors[index](instance, reader.GetInt64(column));
                    }
                    else
                    {
                        longNAccessors[index](instance, (long?)null);
                    }
                    break;
                case FLOAT_INDEX:
                    floatAccessors[index](instance, reader.GetFloat(column));
                    break;
                case FLOATN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        floatNAccessors[index](instance, reader.GetFloat(column));
                    }
                    else
                    {
                        floatNAccessors[index](instance, (float?)null);
                    }
                    break;
                case DOUBLE_INDEX:
                    doubleAccessors[index](instance, reader.GetDouble(column));
                    break;
                case DOUBLEN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        doubleNAccessors[index](instance, reader.GetDouble(column));
                    }
                    else
                    {
                        doubleNAccessors[index](instance, (double?)null);
                    }
                    break;
                case DECIMAL_INDEX:
                    decimalAccessors[index](instance, reader.GetDecimal(column));
                    break;
                case DECIMALN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        decimalNAccessors[index](instance, reader.GetDecimal(column));
                    }
                    else
                    {
                        decimalNAccessors[index](instance, (decimal?)null);
                    }
                    break;
                case CHAR_INDEX:
                    charAccessors[index](instance, reader.GetChar(column));
                    break;
                case CHARN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        charNAccessors[index](instance, reader.GetChar(column));
                    }
                    else
                    {
                        charNAccessors[index](instance, (char?)null);
                    }
                    break;
                case STRING_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        stringAccessors[index](instance, reader.GetString(column));
                    }
                    else
                    {
                        stringAccessors[index](instance, (string)null);
                    }
                    break;
                case DATETIME_INDEX:
                    datetimeAccessors[index](instance, reader.GetDateTime(column));
                    break;
                case DATETIMEN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        datetimeNAccessors[index](instance, reader.GetDateTime(column));
                    }
                    else
                    {
                        datetimeNAccessors[index](instance, (DateTime?)null);
                    }
                    break;
                case BYTEARR_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        bytearrAccessors[index](instance, (byte[])reader.GetValue(column));
                    }
                    else
                    {
                        bytearrAccessors[index](instance, (byte[])null);
                    }
                    break;
                case TIMESPAN_INDEX:
                    timespanAccessors[index](instance, (TimeSpan)reader.GetValue(column));
                    break;
                case TIMESPANN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        timespanNAccessors[index](instance, (TimeSpan)reader.GetValue(column));
                    }
                    else
                    {
                        timespanNAccessors[index](instance, (TimeSpan?)null);
                    }
                    break;
                case GUID_INDEX:
                    guidAccessors[index](instance, reader.GetGuid(column));
                    break;
                case GUIDN_INDEX:
                    if (!reader.IsDBNull(column))
                    {
                        guidNAccessors[index](instance, reader.GetGuid(column));
                    }
                    else
                    {
                        guidNAccessors[index](instance, (Guid?)null);
                    }
                    break;
            }
        }

        private void BuildupType()
        {
            if (!isInitialized)
            {
                Dictionary<string, PropertyInfo> pi = new Dictionary<string, PropertyInfo>();
                bool isExplicitClass = typeof(T).GetCustomAttribute(typeof(ExplicitDataClassAttribute)) != null;
                foreach (var item in typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    DataPropertyAttribute dpa = (DataPropertyAttribute)item.GetCustomAttributes(typeof(DataPropertyAttribute), true).FirstOrDefault();
                    if (dpa != null)
                    {
                        if (dpa.IsExcluded)
                        {
                            continue;
                        }
                        pi.Add(dpa.Name != null ? dpa.Name : item.Name, item);
                    }
                    else if (!isExplicitClass)
                    {
                        pi.Add(item.Name, item);
                    }
                }
                indexToTypeMapping = new int[pi.Count];
                boolAccessors = new Action<T, bool>[pi.Count];
                boolNAccessors = new Action<T, bool?>[pi.Count];
                byteAccessors = new Action<T, byte>[pi.Count];
                byteNAccessors = new Action<T, byte?>[pi.Count];
                shortAccessors = new Action<T, short>[pi.Count];
                shortNAccessors = new Action<T, short?>[pi.Count];
                intAccessors = new Action<T, int>[pi.Count];
                intNAccessors = new Action<T, int?>[pi.Count];
                longAccessors = new Action<T, long>[pi.Count];
                longNAccessors = new Action<T, long?>[pi.Count];
                floatAccessors = new Action<T, float>[pi.Count];
                floatNAccessors = new Action<T, float?>[pi.Count];
                doubleAccessors = new Action<T, double>[pi.Count];
                doubleNAccessors = new Action<T, double?>[pi.Count];
                decimalAccessors = new Action<T, decimal>[pi.Count];
                decimalNAccessors = new Action<T, decimal?>[pi.Count];
                charAccessors = new Action<T, char>[pi.Count];
                charNAccessors = new Action<T, char?>[pi.Count];
                stringAccessors = new Action<T, string>[pi.Count];
                datetimeAccessors = new Action<T, DateTime>[pi.Count];
                datetimeNAccessors = new Action<T, DateTime?>[pi.Count];
                bytearrAccessors = new Action<T, byte[]>[pi.Count];
                timespanAccessors = new Action<T, TimeSpan>[pi.Count];
                timespanNAccessors = new Action<T, TimeSpan?>[pi.Count];
                guidAccessors = new Action<T, Guid>[pi.Count];
                guidNAccessors = new Action<T, Guid?>[pi.Count];
                int cnt = 0;
                foreach (var item in pi)
                {
                    nameToIndexMapping.Add(item.Key, cnt);
                    if (item.Value.PropertyType == typeof(bool))
                    {
                        boolAccessors[cnt] = (Action<T, bool>)Delegate.CreateDelegate(typeof(Action<T, bool>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = BOOL_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(bool?))
                    {
                        boolNAccessors[cnt] = (Action<T, bool?>)Delegate.CreateDelegate(typeof(Action<T, bool?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = BOOLN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(byte))
                    {
                        byteAccessors[cnt] = (Action<T, byte>)Delegate.CreateDelegate(typeof(Action<T, byte>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = BYTE_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(byte?))
                    {
                        byteNAccessors[cnt] = (Action<T, byte?>)Delegate.CreateDelegate(typeof(Action<T, byte?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = BYTEN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(short))
                    {
                        shortAccessors[cnt] = (Action<T, short>)Delegate.CreateDelegate(typeof(Action<T, short>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = SHORT_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(short))
                    {
                        shortNAccessors[cnt] = (Action<T, short?>)Delegate.CreateDelegate(typeof(Action<T, short?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = SHORTN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(int) || item.Value.PropertyType.IsEnum)
                    {
                        intAccessors[cnt] = (Action<T, int>)Delegate.CreateDelegate(typeof(Action<T, int>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = INT_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(int?))
                    {
                        intNAccessors[cnt] = (Action<T, int?>)Delegate.CreateDelegate(typeof(Action<T, int?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = INTN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(long))
                    {
                        longAccessors[cnt] = (Action<T, long>)Delegate.CreateDelegate(typeof(Action<T, long>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = LONG_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(long?))
                    {
                        longNAccessors[cnt] = (Action<T, long?>)Delegate.CreateDelegate(typeof(Action<T, long?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = LONGN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(float))
                    {
                        floatAccessors[cnt] = (Action<T, float>)Delegate.CreateDelegate(typeof(Action<T, float>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = FLOAT_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(float?))
                    {
                        floatNAccessors[cnt] = (Action<T, float?>)Delegate.CreateDelegate(typeof(Action<T, float?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = FLOATN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(double))
                    {
                        doubleAccessors[cnt] = (Action<T, double>)Delegate.CreateDelegate(typeof(Action<T, double>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = DOUBLE_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(double?))
                    {
                        doubleNAccessors[cnt] = (Action<T, double?>)Delegate.CreateDelegate(typeof(Action<T, double?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = DOUBLEN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(decimal))
                    {
                        decimalAccessors[cnt] = (Action<T, decimal>)Delegate.CreateDelegate(typeof(Action<T, decimal>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = DECIMAL_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(decimal?))
                    {
                        decimalNAccessors[cnt] = (Action<T, decimal?>)Delegate.CreateDelegate(typeof(Action<T, decimal?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = DECIMALN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(char))
                    {
                        charAccessors[cnt] = (Action<T, char>)Delegate.CreateDelegate(typeof(Action<T, char>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = CHAR_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(char?))
                    {
                        charNAccessors[cnt] = (Action<T, char?>)Delegate.CreateDelegate(typeof(Action<T, char?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = CHARN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(string))
                    {
                        stringAccessors[cnt] = (Action<T, string>)Delegate.CreateDelegate(typeof(Action<T, string>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = STRING_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(DateTime))
                    {
                        datetimeAccessors[cnt] = (Action<T, DateTime>)Delegate.CreateDelegate(typeof(Action<T, DateTime>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = DATETIME_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(DateTime?))
                    {
                        datetimeNAccessors[cnt] = (Action<T, DateTime?>)Delegate.CreateDelegate(typeof(Action<T, DateTime?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = DATETIMEN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(byte[]))
                    {
                        bytearrAccessors[cnt] = (Action<T, byte[]>)Delegate.CreateDelegate(typeof(Action<T, byte[]>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = BYTEARR_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(TimeSpan))
                    {
                        timespanAccessors[cnt] = (Action<T, TimeSpan>)Delegate.CreateDelegate(typeof(Action<T, TimeSpan>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = TIMESPAN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(TimeSpan?))
                    {
                        timespanNAccessors[cnt] = (Action<T, TimeSpan?>)Delegate.CreateDelegate(typeof(Action<T, TimeSpan?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = TIMESPANN_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(Guid))
                    {
                        guidAccessors[cnt] = (Action<T, Guid>)Delegate.CreateDelegate(typeof(Action<T, Guid>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = GUID_INDEX;
                    }
                    else if (item.Value.PropertyType == typeof(Guid?))
                    {
                        guidNAccessors[cnt] = (Action<T, Guid?>)Delegate.CreateDelegate(typeof(Action<T, Guid?>), item.Value.GetSetMethod());
                        indexToTypeMapping[cnt] = GUIDN_INDEX;
                    }
                    cnt++;
                }
                isInitialized = true;
            }
        }

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        #endregion >> Events <<
    }
}
