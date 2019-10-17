using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Numerics;

using CPPNArt.Utilities;
using Microsoft.Win32;

namespace CPPNArt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CPPNetwork [,] population = new CPPNetwork[2,2];

        public MainWindow()
        {
            InitializeComponent();

            CPPNSettings.ThumbnailWidth = 360;
            CPPNSettings.ThumbnailHeight = 240;

            CPPNSettings.RenderWidth = 15360;
            CPPNSettings.RenderHeight = 8640;

            CPPNSettings.PaletteSize = 65535;

            CPPNSettings.UseColor = false;

            CPPNSettings.GenerateWeight = () => ThreadSafeRandom.NextUniform() * 4;

            //CPPNSettings.GenerateWeight = () => 1;

            CPPNSettings.Activations.Add(Activations.Tanh);
            CPPNSettings.Activations.Add(Activations.Gaussian);
            CPPNSettings.Activations.Add(Activations.Sin);
            //CPPNSettings.Activations.Add(Activations.Cos);
            //CPPNSettings.Activations.Add(Activations.ID);
            //CPPNSettings.Activations.Add(Activations.Sqrt);
            //CPPNSettings.Activations.Add(x => x.Magnitude);
            //CPPNSettings.Activations.Add(x => -x);
            //CPPNSettings.Activations.Add(x => SharpNoise.NoiseGenerator.GradientCoherentNoise3D(x, 1, 1));
            //CPPNSettings.Activations.Add(x => x < -1 ? -1 : x > 1 ? 1 : x);

            //CPPNSettings.Activations.Add(Activations.ArcTan);
            //CPPNSettings.Activations.Add(Activations.Sigmoid);
            //CPPNSettings.Activations.Add(Activations.AbsTanh);
            //CPPNSettings.Activations.Add(Activations.Abs);
            //CPPNSettings.Activations.Add(Activations.Sinc);

            CPPNSettings.Activations.Add(Activations.Square);
            CPPNSettings.Activations.Add(Activations.Cube);
            ////CPPNSettings.Activations.Add(Activations.Sqrt);
            //CPPNSettings.Activations.Add(Activations.Log);
            //CPPNSettings.Activations.Add(Activations.Recp);
            //CPPNSettings.Activations.Add(x => x % 1.0);

            CPPNSettings.Combinators.Add((x, y) => x + y);
            //CPPNSettings.Combinators.Add((x, y) => x * y);
            //CPPNSettings.Combinators.Add((x, y) => x % y);
            //CPPNSettings.Combinators.Add((x, y) => Math.Min(x.Magnitude, y.Magnitude));

            //CPPNSettings.Combinators.Add((x, y) => {
            //    var a = Math.Cos(x / (2 * Math.PI));
            //    var b = Math.Sin(x / (2 * Math.PI));

            //    var c = Math.Cos(y / (2 * Math.PI));
            //    var d = Math.Sin(y / (2 * Math.PI));

            //    var r = (a * c) - (b * d);
            //    var i = (a * d) + (c * b);

            //    return Math.Atan2(i, r);
            //});

            CPPNSettings.OutputTransform = Activations.ID;

            InitializePopulation();

            RenderPopulation();
        }

        public void InitializePopulation()
        {
            for (var x = 0; x < 2; x++)
                for (var y = 0; y < 2; y++)
                    population[x, y] = new CPPNetwork(5, 1);
        }

        protected void RenderPopulation()
        {
            Genome1.Source = CPPNRenderEngine.Render(population[0, 0], CPPNSettings.ThumbnailWidth, CPPNSettings.ThumbnailHeight);
            Genome2.Source = CPPNRenderEngine.Render(population[0, 1], CPPNSettings.ThumbnailWidth, CPPNSettings.ThumbnailHeight);
            Genome3.Source = CPPNRenderEngine.Render(population[1, 0], CPPNSettings.ThumbnailWidth, CPPNSettings.ThumbnailHeight);
            Genome4.Source = CPPNRenderEngine.Render(population[1, 1], CPPNSettings.ThumbnailWidth, CPPNSettings.ThumbnailHeight);
        }

        public void Genome1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            population[0, 1] = CPPNetwork.Mutate(population[0, 0]);
            population[1, 0] = CPPNetwork.Mutate(population[0, 0]);
            population[1, 1] = CPPNetwork.Mutate(population[0, 0]);

            RenderPopulation();
        }

        public void Genome2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            population[0, 0] = CPPNetwork.Mutate(population[0, 1]);
            population[1, 0] = CPPNetwork.Mutate(population[0, 1]);
            population[1, 1] = CPPNetwork.Mutate(population[0, 1]);

            RenderPopulation();
        }

        public void Genome3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            population[0, 0] = CPPNetwork.Mutate(population[1, 0]);
            population[0, 1] = CPPNetwork.Mutate(population[1, 0]);
            population[1, 1] = CPPNetwork.Mutate(population[1, 0]);

            RenderPopulation();
        }

        public void Genome4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            population[0, 0] = CPPNetwork.Mutate(population[1, 1]);
            population[0, 1] = CPPNetwork.Mutate(population[1, 1]);
            population[1, 0] = CPPNetwork.Mutate(population[1, 1]);

            RenderPopulation();
        }

        protected void InitializePopulationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InitializePopulation();

            RenderPopulation();
        }

        protected void Genome1_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            SaveImage(population[0,0]);
        }

        protected void Genome2_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            SaveImage(population[0, 1]);
        }

        protected void Genome3_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            SaveImage(population[1, 0]);
        }

        protected void Genome4_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SaveImage(population[1, 1]);
        }

        protected void SaveImage(CPPNetwork n)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            if (sfd.ShowDialog() ?? true)
            {
                IsEnabled = false;

                var encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CPPNRenderEngine.Render(n, CPPNSettings.RenderWidth, CPPNSettings.RenderHeight)));

                using (var stream = sfd.OpenFile())
                {
                    encoder.Save(stream);
                }

                IsEnabled = true;
            }
        }

        protected void CreatePalette_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() ?? true)
            {
                CPPNSettings.ColorPalette = ColorPalette.FromImageSOM(ofd.FileName, 16, CPPNSettings.PaletteSize);

                CPPNSettings.UseColor = true;

                RenderPopulation();
            }
        }

        protected void ToggleColorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CPPNSettings.UseColor = !CPPNSettings.UseColor;

            RenderPopulation();
        }
    }
}