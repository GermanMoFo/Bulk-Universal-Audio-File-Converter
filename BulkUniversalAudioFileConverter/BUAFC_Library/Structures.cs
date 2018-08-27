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
}
