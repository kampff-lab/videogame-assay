using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class Weighted<T>
    {
        public Weighted()
        {
        }

        public Weighted(T value, double weight)
        {
            Weight = weight;
            Value = value;
        }

        public double Weight { get; set; }

        public T Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Value, Weight);
        }
    }
}
