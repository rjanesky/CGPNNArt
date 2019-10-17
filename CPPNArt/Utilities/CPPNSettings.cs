using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using System.Numerics;

namespace CPPNArt.Utilities
{
    public static class CPPNSettings
    {
        public static int ThumbnailWidth;
        public static int ThumbnailHeight;

        public static int RenderWidth;
        public static int RenderHeight;

        public static List<Color> ColorPalette;

        public static int PaletteSize;

        public static bool UseColor;

        public static Func<Complex> GenerateWeight;

        public static Func<Complex, Complex> ID = x => x;

        public static Func<Complex, Complex> OutputTransform;

        public static List<Func<Complex, Complex>> Activations = new List<Func<Complex, Complex>>();

        public static Func<Func<Complex, Complex>> RandomActivation = () =>
        {
             return CPPNSettings.Activations[ThreadSafeRandom.Next(CPPNSettings.Activations.Count)];
        };

        public static List<Func<Complex, Complex, Complex>> Combinators = new List<Func<Complex, Complex, Complex>>();

        public static Func<Func<Complex, Complex, Complex>> RandomCombination = () =>
        {
            return CPPNSettings.Combinators[ThreadSafeRandom.Next(CPPNSettings.Combinators.Count)];
        };
    }
}
