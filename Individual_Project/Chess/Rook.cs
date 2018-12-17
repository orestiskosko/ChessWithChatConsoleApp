using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Rook : Piece
    {
        public Rook(Side side, Position pos) : base(side, pos) { }

        public override bool ValidateMove(Position newPos, Chessboard board)
        {
            if (newPos.X == CurrentPosition.X || newPos.Y == CurrentPosition.Y)
                return DoMove(this, newPos, board);
            else
                return false;
        }

    }
}
