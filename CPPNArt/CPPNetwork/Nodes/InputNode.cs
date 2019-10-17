using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CPPNArt.Utilities;

using System.Numerics;

namespace CPPNArt
{
    public class InputNode : Node
    {
        public int Index;

        public InputNode(int index)
            : base()
        {
            Index = index;

            Activation = x => x;

            NetworkLevel = double.MinValue;
        }

        protected override Complex evaluate(Complex [] input, List<Connection> connections)
        {
            return Activation(input[Index]);
        }
    }
}
