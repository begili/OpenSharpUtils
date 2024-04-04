using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSharpUtils.DataAccess.DataAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExplicitDataClassAttribute : Attribute
    {
    }
}
