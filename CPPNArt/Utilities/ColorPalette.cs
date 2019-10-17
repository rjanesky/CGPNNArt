using CPPNArt.SOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CPPNArt.Utilities
{
    public class ColorPalette
    {
        protected static IEnumerable<Color> GradientBetween(Color a, Color b, double steps)
        {
            List<Color> result = new List<Color>();

            var rStep = (b.R - a.R) / steps;
            var gStep = (b.G - a.G) / steps;
            var bStep = (b.B - a.B) / steps;

            for (var i = 0; i < steps; i++)
            {
                result.Add(Color.FromArgb(255,
                                            (byte)(a.R + (rStep * i)),
                                            (byte)(a.G + (gStep * i)),
                                            (byte)(a.B + (bStep * i))));
            }

            return result;
        }

        protected static Color Random()
        {
            return Color.FromArgb(255, (byte)ThreadSafeRandom.Next(255), (byte)ThreadSafeRandom.Next(255), (byte)ThreadSafeRandom.Next(255));
        }

        protected static Color Lerp(Color a, Color b, double t)
        {
            return Color.FromArgb(
                (byte)(a.A + (b.A - a.A) * t),
                (byte)(a.R + (b.R - a.R) * t),
                (byte)(a.G + (b.G - a.G) * t),
                (byte)(a.B + (b.B - a.B) * t));
        }

        public static List<Color> CreatePalette(List<Color> a_colors, int a_size)
        {
            List<Color> cPalette = new List<Color>();

            var stops = a_colors.
                GetRange(0, a_colors.Count()).
                Select(i => ThreadSafeRandom.Next(a_size)).
                OrderBy(i => i).
                ToList();

            stops.Add(a_size);

            var stop_index = 0;
            for (var i = 0; i < stops.Count(); i++)
            {
                cPalette.AddRange(GradientBetween(a_colors[i % a_colors.Count()],
                                                    a_colors[(i + 1) % a_colors.Count()],
                                                    stops[(i + 1) % stops.Count()] - stop_index));
                stop_index = stops[i];
            }

            return cPalette;
        }

        public static List<Color> FromConstant(Color c, int sample_count, int palette_size)
        {
            var r = new List<Color>();

            for(var i = 0; i < sample_count; i++)
            {
                r.Add(ColorPalette.Lerp(ColorPalette.Random(), c, .1));
            }

            return ColorPalette.CreatePalette(r, palette_size);
        }

        public static List<Color> FromImageSOM(string filepath, int color_samples, int palette_size)
        {
            WriteableBitmap w = new WriteableBitmap(new BitmapImage(new Uri(filepath)));

            byte[] pixels = new byte[(long)w.PixelHeight * (long)w.PixelWidth * (long)w.Format.BitsPerPixel / (long)8];

            w.CopyPixels(pixels, w.PixelWidth * w.Format.BitsPerPixel / 8, 0);

            var image_width = w.PixelWidth;
            var image_height = w.PixelHeight;

            var s = new SOMNetwork(3, color_samples);

            var t = 1.0;

            for (var i = 0; i < 10000; i++)
            {
                var cx = ThreadSafeRandom.Next(image_width);
                var cy = ThreadSafeRandom.Next(image_height);

                double[] c = new double[3];

                c[0] = pixels[4 * (cy * image_width + cx) + 2];
                c[1] = pixels[4 * (cy * image_width + cx) + 1];
                c[2] = pixels[4 * (cy * image_width + cx) + 0];

                s.Train(c, t);

                t -= 1 / 10000.0;
            }

            return ColorPalette.CreatePalette(
                s.Neurons.Select(i => Color.FromArgb(255, (byte)(i.weights[0]), (byte)(i.weights[1]), (byte)(i.weights[2]))).Distinct().ToList(), palette_size);
        }
    }
}
