using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class King : Piece
    {
        public King(Side side, Position pos) : base(side, pos) { }


        public override bool ValidateMove(Position newPosition, Chessboard board)
        {
            int absDx = Math.Abs(newPosition.X - CurrentPosition.X);
            int absDy = Math.Abs(newPosition.Y - CurrentPosition.Y);

            if (absDx <= 1 && absDy <= 1)
            {
                return DoMove(this, newPosition, board);
            }
            return false;
        }
    }
}
