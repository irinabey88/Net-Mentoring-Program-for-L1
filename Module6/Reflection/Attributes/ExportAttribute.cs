using System;

namespace Attributes
{
    public class ExportAttribute : Attribute
    {
        public ExportAttribute()
        {
        }

        public ExportAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}