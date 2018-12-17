using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess
{
    static class SquareMapping
    {
        static readonly Dictionary<string, int> mapX = new Dictionary<string, int>()
        {
            {"a", 0},
            {"b", 1},
            {"c", 2},
            {"d", 3},
            {"e", 4},
            {"f", 5},
            {"g", 6},
            {"h", 7}
        };
        static readonly Dictionary<string, int> mapY = new Dictionary<string, int>()
        {
            {"1", 7},
            {"2", 6},
            {"3", 5},
            {"4", 4},
            {"5", 3},
            {"6", 2},
            {"7", 1},
            {"8", 0}
        };


        static public bool ParseMove(string moveString, out Position startPos, out Position endPos)
        {
            startPos = null;
            endPos = null;

            if (!Regex.IsMatch(moveString, "^[a-h][1-8]-[a-h][1-8]"))
                return false;
            
            startPos = new Position((ushort)mapX[moveString.Substring(0,1)], (ushort)mapY[moveString.Substring(1, 1)]);
            endPos = new Position((ushort)mapX[moveString.Substring(3, 1)], (ushort)mapY[moveString.Substring(4, 1)]);

            return true;
        }
    }
}
