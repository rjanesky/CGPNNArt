using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace CPPNArt.Utilities
{
    public static class Activations
    {
        public static Complex ID(Complex x)
        {
            return x;
        }

        public static Complex Square(Complex x)
        {
            return x * x;
        }

        public static Complex Cube(Complex x)
        {
            return x * x * x;
        }

        public static Complex Sqrt(Complex x)
        {
            return Complex.Sqrt(x);
        }

        public static Complex Log(Complex x)
        {
            return Complex.Log(x);
        }

        public static Complex Tanh(Complex x)
        {
            return Complex.Tanh(x / 4.0);
        }

        public static Complex AbsTanh(Complex x)
        {
            return Complex.Abs(Complex.Tanh(x));
        }

        public static Complex Sin(Complex x)
        {
            return Complex.Sin(x);
        }

        public static Complex Cos(Complex x)
        {
            return Complex.Cos(x);
        }

        public static Complex Gaussian(Complex x)
        {
            return Complex.Exp(-(2 * x * x)) * 2.0 - 1.0;
        }

        public static Complex Recp(Complex x)
        {
            return x == 0 ? 0.0 : 1.0 / x;
        }

        public static Complex Sinc(Complex x)
        {
            return x == 0 ? 0.0 : Complex.Sin(x * 2 * Math.PI) / x;
        }

        public static Complex Sigmoid(Complex x)
        {
            return (1.0 / (1 + Complex.Exp(-2 * x))) * 2 - 1;
        }

        public static Complex ArcTan(Complex x)
        {
            return Complex.Atan(x) / (Math.PI / 2);
        }

        public static Complex Abs(Complex x)
        {
            return Complex.Abs(x);
        }
    }
}
