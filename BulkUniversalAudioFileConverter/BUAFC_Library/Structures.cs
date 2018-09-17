using System;
using System.Collections;
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

    public struct Pairing<A, B>
    {
        public A a;
        public B b;

        public Pairing(A a, B b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public class DoubleAssociativeList<A, B> : IEnumerable<Pairing<A, B>>
    {
        List<Pairing<A, B>> pairings = new List<Pairing<A, B>>();

        public A this[B b]
        {
            get
            {
                foreach (var pair in pairings)
                    if (pair.b.Equals(b))
                        return pair.a;
                throw new KeyNotFoundException();
            }
            set
            {
                for (int i = 0; i < pairings.Count; ++i)
                    if (pairings[i].Equals(b))
                    {
                        pairings[i] = new Pairing<A, B>(value, pairings[i].b);
                    }
            }
        }

        public B this[A a]
        {
            get
            {
                foreach (var pair in pairings)
                    if (pair.a.Equals(a))
                        return pair.b;
                throw new KeyNotFoundException();
            }
            set
            {
                for (int i = 0; i < pairings.Count; ++i)
                    if (pairings[i].Equals(a))
                    {
                        pairings[i] = new Pairing<A, B>(pairings[i].a, value);
                    }
            }
        }

        public void Add(A a, B b)
        {
            pairings.Add(new Pairing<A, B>(a, b));
        }

        public bool Remove(A a)
        {
            foreach (var pair in pairings)
                if (pair.a.Equals(a))
                {
                    pairings.Remove(pair);
                    return true;
                }
            return false;
        }

        public bool Remove(B b)
        {
            foreach (var pair in pairings)
                if (pair.b.Equals(b))
                {
                    pairings.Remove(pair);
                    return true;
                }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return pairings.GetEnumerator();
        }

        public IEnumerator<Pairing<A, B>> GetEnumerator()
        {
            return pairings.GetEnumerator();
        }
    }

    
}
