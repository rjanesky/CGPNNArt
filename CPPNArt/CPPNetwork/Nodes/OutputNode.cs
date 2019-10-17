using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CPPNArt.Utilities;

using System.Numerics;

namespace CPPNArt
{
    public class OutputNode : Node
    {
        public OutputNode(Func<Complex, Complex> activation, Func<Complex, Complex, Complex> combine)
            : base()
        {
            Activation = activation;

            Combine = combine;

            NetworkLevel = double.MaxValue;
        }

        protected override Complex evaluate(Complex [] input, List<Connection> connections)
        {
            return Activation(this.IncomingConnections
                .Select(x => x.Weight * x.Input.Evaluate(input, connections))
                .Aggregate(Combine));
        }
    }
}
