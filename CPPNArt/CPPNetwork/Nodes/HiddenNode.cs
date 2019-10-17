using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

using CPPNArt.Utilities;

namespace CPPNArt
{
    public class HiddenNode : Node
    {
        public HiddenNode(Func<Complex, Complex> activation, Func<Complex, Complex, Complex> combine, double networkLevel)
            : base()
        {
            Activation = activation;

            Combine = combine;

            NetworkLevel = networkLevel;
        }

        protected override Complex evaluate(Complex [] input, List<Connection> connections)
        {
            return Activation(this.IncomingConnections
                .Select(x => x.Weight * x.Input.Evaluate(input, connections))
                .Aggregate(Combine));
        }
    }
}
