using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSharpUtils.DataAccess.DataAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataPropertyAttribute : Attribute
    {
        public string Name { get; private set; }

        public bool IsExcluded { get; private set; }

        public DataPropertyAttribute()
        {

        }

        public DataPropertyAttribute(string name)
        {
            this.Name = name;
        }

        public DataPropertyAttribute(bool isExcluded)
        {
            this.IsExcluded = isExcluded;
        }
    }
}
