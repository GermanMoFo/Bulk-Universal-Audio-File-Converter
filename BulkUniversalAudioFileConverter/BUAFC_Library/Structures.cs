using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAFC_Library
{
    public class Progress
    {
        double value;

        public Progress(double d)
        {
            value = d;
        }

        public static implicit operator double(Progress d)
        {
            return d.value;
        }

        public static implicit operator Progress(double d)
        {
            return new Progress(d);
        }
    }

    public class MultiIdentifiedString
    {
        List<string> strs;

        public MultiIdentifiedString(string[] _strs)
        {
            strs = new List<string>(_strs);
        }

        public void AddIdentifier(string str)
        {
            strs.Add(str);
        }
    }

    public class MultiIdentifiedStringComparer : IEqualityComparer<MultiIdentifiedString>
    {
        public bool Equals(MultiIdentifiedString x, MultiIdentifiedString y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(MultiIdentifiedString obj)
        {
            throw new NotImplementedException();
        }
    }

    
}
