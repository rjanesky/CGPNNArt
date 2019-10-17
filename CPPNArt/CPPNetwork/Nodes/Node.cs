using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

using CPPNArt.Utilities;

namespace CPPNArt
{
    public abstract class Node
    {
        protected abstract Complex evaluate(Complex [] input, List<Connection> connections);

        public int InnovationNumber;

        public double NetworkLevel;

        public Func<Complex, Complex> Activation;

        public Func<Complex, Complex, Complex> Combine;

        protected List<Connection> IncomingConnections;

        public Node()
        {
            IncomingConnections = new List<Connection>();
        }

        public Complex Evaluate(Complex [] input, List<Connection> connections)
        {
            return CPPNSettings.OutputTransform(evaluate(input, connections));
        }

        public void ClearConnectionCache()
        {
            IncomingConnections.Clear();
        }

        public void BuildConnectionCache(IEnumerable<Connection> xs)
        {
            IncomingConnections.AddRange(xs.Where(x => x.IsEnabled && x.Output == this));
        }
    }
}
