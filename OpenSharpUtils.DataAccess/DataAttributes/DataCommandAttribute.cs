using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSharpUtils.DataAccess.DataAttributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DataCommandAttribute : Attribute
    {
        public int Index { get; private set; }

        public string CommandText { get; private set; }

        public string[] CommandParameters { get; private set; }

        public bool IsProcedure { get; private set; }

        public DataCommandAttribute(int index, string commandText, bool isProcedure, params string[] commandParameters)
        {
            this.Index = index;
            this.CommandText = commandText;
            this.CommandParameters = commandParameters;
            this.IsProcedure = isProcedure;
        }
    }
}
