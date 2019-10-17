using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPNArt.Extensions
{
    public static class ArrayExtensions
    {
        public static double [] LERP(this double [] a, double [] b, double factor)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Dimension Mismatch");

            var result = new double[a.Length];

            for (var i = 0; i < a.Length; i++)
                result[i] = a[i] + (b[i] - a[i]) * factor;

            return result;
        }
    }
}
