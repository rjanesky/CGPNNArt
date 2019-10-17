using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

using CPPNArt.Utilities;

namespace CPPNArt
{
    public class CPPNetwork
    {
        public List<Node> Nodes;

        public List<Connection> Connections;

        public List<Node> Outputs;
        public List<Node> Inputs;

        public CPPNetwork(int input_count, int output_count)
        {
            Nodes = new List<Node>();

            Inputs = new List<Node>();
            Outputs = new List<Node>();

            Connections = new List<Connection>();

            for (var i = 0; i < input_count; i++)
            {
                Inputs.Add(new InputNode(i));
                Nodes.Add(Inputs[i]);
            }

            for (var i = 0; i < output_count; i++)
            {
                Outputs.Add(new OutputNode(CPPNSettings.OutputTransform, (Complex x, Complex y) => x + y));
                Nodes.Add(Outputs[i]);
            }

            foreach (Node n in Outputs)
            {
                Connections.Add(new Connection(Inputs[0], n));
                Connections.Add(new Connection(Inputs[1], n));
            }
        }

        protected CPPNetwork(CPPNetwork s)
        {
            Nodes = new List<Node>();

            Connections = new List<Connection>();

            Inputs = new List<Node>();
            Outputs = new List<Node>();

            foreach (Node n in s.Inputs)
                Inputs.Add(n);

            foreach (Node n in s.Outputs)
                Outputs.Add(n);

            for (var i = 0; i < s.Nodes.Count; i++)
                Nodes.Add(s.Nodes[i]);

            for (var i = 0; i < s.Connections.Count; i++)
                Connections.Add(new Connection(s.Connections[i]));
        }

        public void BuildConnectionCache()
        {
            foreach (Node n in Nodes)
            {
                n.ClearConnectionCache();
                n.BuildConnectionCache(Connections);
            }
        }

        public Complex [] Evaluate(Complex [] input)
        { 
            return Outputs.Select(x => x.Evaluate(input, Connections)).ToArray();
        }

        public static CPPNetwork Mutate(CPPNetwork s)
        {
            var t = new CPPNetwork(s);

            var c = Utilities.ThreadSafeRandom.Next(t.Connections.Count);

            var w = t.Connections[c].Weight + Utilities.ThreadSafeRandom.NextUniform();

            t.Connections[c] = Connection.UpdateWeight(t.Connections[c], w);

            if (Utilities.ThreadSafeRandom.NextDouble() < .4)
            {
                if (Utilities.ThreadSafeRandom.NextDouble() < .5)
                {
                    t.AddNode();
                }
                else
                {
                    t.AddConnection();
                }
            }

            return t;
        }

        public void AddConnection()
        {
            var validConnections = Nodes
                .Join(Nodes, x => true, y => true, (x, y) => new { x, y })
                .Where(a => a.x.NetworkLevel < a.y.NetworkLevel)
                .Where(a => Connections.Where(b => b.Input == a.x && b.Output == a.y).Count() == 0)
                .ToList();

            if (validConnections.Count() > 0)
            {
                var  n = validConnections.ElementAt(Utilities.ThreadSafeRandom.Next(validConnections.Count()));
                Connections.Add(new Connection(n.x, n.y));
            }
        }

        public void AddNode()
        {
            var c = Connections[Utilities.ThreadSafeRandom.Next(Connections.Count)];

            var n = new HiddenNode(CPPNSettings.RandomActivation(), CPPNSettings.RandomCombination(), c.Input.NetworkLevel * .5 + c.Output.NetworkLevel * .5);

            Nodes.Add(n);

            Connections.Add(new Connection(c.Input, n));
            Connections.Add(new Connection(n, c.Output));

            c.IsEnabled = false;
        }
    }

}
