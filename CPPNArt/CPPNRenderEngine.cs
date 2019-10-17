using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CPPNArt.Utilities;

namespace CPPNArt
{
    public static class CPPNRenderEngine
    {
        public static WriteableBitmap Render(CPPNArt.CPPNetwork c, int image_width, int image_height)
        {
            c.BuildConnectionCache();

            var wb = new WriteableBitmap(image_width, image_height, 300, 300, PixelFormats.Bgra32, null);

            byte [] pixels = new byte[(long)wb.PixelHeight * (long)wb.PixelWidth * (long)wb.Format.BitsPerPixel / (long)8];

            double [] raw_output = new double[wb.PixelHeight * wb.PixelWidth];

            float xstep = (float)(4.0 / image_width);
            float ystep = (float)(4.0 / image_height);

            Parallel.For(0, image_width, x =>
            {
                for (var y = 0; y < image_height; y++)
                {
                    double dx = (float)((xstep * x) - 2.0);
                    double dy = (float)((ystep * y) - 2.0);

                    var r = Math.Sqrt(dx * dx + dy * dy);

                    var n = SharpNoise.NoiseGenerator.GradientCoherentNoise3D(4 * dx, 4 * dy, 0);

                    var u = new SharpNoise.Modules.RidgedMulti();

                    n = u.GetValue(dx / 1, dy / 1, 0.0);

                    var t = dx == 0  ? (dy < 0 ? .5 * Math.PI : .5 * Math.PI) : Math.Atan2(Math.Abs(dy), Math.Abs(dx));

                    //var o = c.Evaluate(new double[] { dx, dy, r, t, .05 * n, 1.0 });

                    var o = c.Evaluate(new Complex [] { dx, dy, r, -1.0, 1.0 });

                    raw_output[y * image_width + x] = o[0].Magnitude;
                }
            });

            double max_value = raw_output.Max();

            Parallel.For(0, image_width, x =>
            {
                for (var y = 0; y < image_height; y++)
                {
                    if (CPPNSettings.UseColor) {
                        var value = (int)(raw_output[y * image_width + x] / max_value * (CPPNSettings.PaletteSize - 1));

                        var color = CPPNSettings.ColorPalette[value];

                        pixels[4 * (y * image_width + x) + 0] = (byte)(color.B);
                        pixels[4 * (y * image_width + x) + 1] = (byte)(color.G);
                        pixels[4 * (y * image_width + x) + 2] = (byte)(color.R);
                        pixels[4 * (y * image_width + x) + 3] = 0xff;
                    }
                    else
                    {
                        var value = raw_output[y * image_width + x] / max_value * 255.0;

                        pixels[4 * (y * image_width + x) + 0] = (byte)(value);
                        pixels[4 * (y * image_width + x) + 1] = (byte)(value);
                        pixels[4 * (y * image_width + x) + 2] = (byte)(value);
                        pixels[4 * (y * image_width + x) + 3] = 0xff;
                    }
                }
            });

            wb.WritePixels(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight), pixels,
                 wb.PixelWidth * wb.Format.BitsPerPixel / 8, 0);

            return wb;
        }
    }
}