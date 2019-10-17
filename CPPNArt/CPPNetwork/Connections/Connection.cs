using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CPPNArt.Utilities;

using System.Numerics;

namespace CPPNArt
{
    public class Connection
    {
        public int InnovationNumber;

        public bool IsEnabled;
        public Complex Weight;

        public Node Input;
        public Node Output;

        public static Connection UpdateWeight(Connection i, Complex w)
        {
            var c = new Connection(i)
            {
                Weight = w
            };

            return c;
        }

        public Connection(Connection s)
        {
            IsEnabled = s.IsEnabled;
            Weight = s.Weight;
            Input = s.Input;
            Output = s.Output;
        }

        public Connection(Node input, Node output)
        {
            IsEnabled = true;
            Weight = CPPNSettings.GenerateWeight();

            this.Input = input;
            this.Output = output;
        }
    }
}
