using CPPNArt.Extensions;
using CPPNArt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPNArt.SOM
{
    public class SOMNetwork
    {
        public SOMNeuron[] Neurons;

        public SOMNetwork(int input_count, int neuron_count)
        {
            Neurons = new SOMNeuron[neuron_count];

            for (var i = 0; i < neuron_count; i++)
                Neurons[i] = new SOMNeuron(input_count);
        }

        public void Train(double [] sample, double t)
        {
            var results = Neurons.Select(i => i.Evaluate(sample)).ToArray();

            var min_index = 0;

            for (var i = 0; i < Neurons.Count();i++)
            {
                if (results[i] < results[min_index])
                {
                    min_index = i;
                }
            }

            Neurons[min_index].weights = Neurons[min_index].weights.LERP(sample, t);
        }
    }

    public class SOMNeuron
    {
        public double[] weights;

        public SOMNeuron(int input_count)
        {
            weights = new double[input_count];

            for (var i = 0; i < input_count; i++)
                weights[i] = ThreadSafeRandom.NextDouble();
        }

        public double Evaluate(double [] inputs)
        {
            return Math.Sqrt(Enumerable.Zip(inputs, weights, (x, y) => (x - y) * (x - y)).Aggregate((x, y) => x + y));
        }
    }
}
