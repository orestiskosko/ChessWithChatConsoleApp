using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Position
    {

        public ushort X { get; set; }
        public ushort Y { get; set; }

        public Position(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }
    }
}
