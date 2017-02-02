using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Services.Stats.Manager
{
    public enum StatValueType
    {
        Number,
        String
    }

    public class StatValue
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public StatValueType Type { get; set; }

        internal StatValue(string name, object value, StatValueType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public int AsInteger()
        {
            return (int)(double)Value;
        }
        public string AsString()
        {
            return (string)Value;
        }
        public double AsNumber()
        {
            return (double)Value;
        }

        internal void SetStat(object value, StatValueType type)
        {
            Value = value;
            Type = type;
        }
    }
}