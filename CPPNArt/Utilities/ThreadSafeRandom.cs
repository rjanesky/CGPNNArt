using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CPPNArt.Utilities
{
    public class ThreadSafeRandom
    {
        private static Random random = new Random();

        public static int Next()
        {
            lock (random)
            {
                return random.Next();
            }
        }

        public static int Next(int upper_bound)
        {
            lock (random)
            {
                return random.Next(upper_bound);
            }
        }

        public static int Next(int lower_bound, int upper_bound)
        {
            lock (random)
            {
                return random.Next(lower_bound, upper_bound);
            }
        }

        public static Complex NextComplex()
        {
            lock (random)
            {
                var theta = random.NextDouble() * Math.PI * 2;

                var x = Math.Cos(theta);
                var y = Math.Sin(theta);

                return new Complex(x,y);
            }
        }

        public static double NextDouble()
        {
            lock (random)
            {
                return random.NextDouble();
            }
        }

        public static Complex NextUniform()
        {
            lock (random)
            {
                return NextDouble();
            }
        }

        public static Complex NextGaussian(Complex mean, Complex stddev)
        {
            lock (random)
            {
                double x1 = 1 - random.NextDouble();
                double x2 = 1 - random.NextDouble();

                double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);

                return y1 * stddev + mean;
            }
        }
    }
}
