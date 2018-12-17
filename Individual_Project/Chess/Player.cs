using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Player
    {
        string Username { get; set; }
        Side Side { get; set; }

        public Player(string username, Side side)
        {
            Username = username;
            Side = side;
        }
    }
}
