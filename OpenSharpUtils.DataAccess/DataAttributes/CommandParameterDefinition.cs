using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSharpUtils.DataAccess.DataAttributes
{
    public class CommandParameterDefinition
    {
        public string Name { get; private set; }

        public Type PropertyType { get; private set; }

        public CommandParameterDefinition(string name, Type propertyType)
        {
            this.Name = name;
            this.PropertyType = propertyType;
        }
    }
}
