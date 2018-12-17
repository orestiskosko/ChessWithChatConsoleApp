using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    static class PieceUni
    {
        static public Dictionary<Type, string> whitePieceUnicode = new Dictionary<Type, string>()
        {
            { typeof(King), "\u2654" },
            { typeof(Queen), "\u2655" },
            {typeof(Rook), "\u2656" },
            {typeof(Bishop), "\u2657" },
            {typeof(Knight), "\u2658" },
            {typeof(Pawn), "\u2659" }
        };
        static public Dictionary<Type, string> blackPieceUnicode = new Dictionary<Type, string>()
        {
            {typeof(King), "\u265A" },
            {typeof(Queen), "\u265B" },
            {typeof(Rook), "\u265C" },
            {typeof(Bishop), "\u265D" },
            {typeof(Knight), "\u265E" },
            {typeof(Pawn), "\u265F" }
        };

    }
}
