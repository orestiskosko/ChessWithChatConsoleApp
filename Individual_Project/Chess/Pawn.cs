using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Pawn : Piece
    {
        private bool FirstMoveDone = false;

        public Pawn(Side side, Position pos) : base(side, pos) { }

        public override bool ValidateMove(Position newPosition, Chessboard board)
        {
            int Dx = newPosition.X - CurrentPosition.X;
            int Dy = newPosition.Y - CurrentPosition.Y;

            int dir = 1;
            if (Side == Side.White)
                dir = -1;


            // Move if it's the first move.
            if (
                !FirstMoveDone
                & board.State[newPosition.X, newPosition.Y] == null
                & (Dy == dir || Dy == 2 * dir)
                & Dx == 0
                )
            {
                if (!FirstMoveDone) { FirstMoveDone = !FirstMoveDone; }
                return DoMove(this, newPosition, board);
            }

            if (
                !FirstMoveDone
                & board.State[newPosition.X, newPosition.Y] != null
                & (Dy == dir || Dy == 2 * dir)
                & Math.Abs(Dx) == 1
                )
            {
                if (!FirstMoveDone) { FirstMoveDone = !FirstMoveDone; }
                return DoMove(this, newPosition, board);
            }

            // Move if it's not the first move.
            if (
                FirstMoveDone
                & board.State[newPosition.X, newPosition.Y] == null
                & Dy == dir
                & Dx == 0
                )
            {
                if (!FirstMoveDone) { FirstMoveDone = !FirstMoveDone; }
                return DoMove(this, newPosition, board);
            }

            if (
                FirstMoveDone
                & board.State[newPosition.X, newPosition.Y] != null
                & Dy == dir
                & Math.Abs(Dx) == 1
                )
            {
                if (!FirstMoveDone) { FirstMoveDone = !FirstMoveDone; }
                return DoMove(this, newPosition, board);
            }

            return false;
        }
    }
}
