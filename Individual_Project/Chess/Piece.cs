using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess
{
    [Serializable]
    public abstract class Piece
    {
        public Position CurrentPosition { get; set; }
        public Side Side { get; set; }


        public Piece(Side side, Position pos)
        {
            Side = side;
            CurrentPosition = new Position(pos.X, pos.Y);
        }

        /// <summary>
        /// Performs a deep clone of the Piece class.
        /// </summary>
        /// <typeparam name="Piece"></typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Piece DeepClone<Piece>(Piece other)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;

                return (Piece)formatter.Deserialize(ms);
            }
        }


        public abstract bool ValidateMove(Position newPosition, Chessboard board);


        internal bool DoMove(Piece piece, Position newPos, Chessboard board)
        {
            bool Obstacle = false;

            // Store a temporary state of the board
            Chessboard tempBoard = Chessboard.DeepClone(board);
            Piece pieceMirrored = DeepClone(piece);


            // Check if there any same-side pieces in the way of the moving piece
            // Knight is an eception
            if (!piece.GetType().Equals(typeof(Knight)))
            {
                int Dx = newPos.X - piece.CurrentPosition.X;
                int Dy = newPos.Y - piece.CurrentPosition.Y;

                // Perform mirror operations when needed
                Position newPosMirrored = new Position(newPos.X, newPos.Y);
                if (Dx < 0)
                {
                    tempBoard.State = HorizontalMirror(tempBoard.State);
                    newPosMirrored.X = (ushort)(7 - newPos.X);
                    pieceMirrored.CurrentPosition.X = (ushort)(7 - piece.CurrentPosition.X);
                }
                if (Dy < 0)
                {
                    tempBoard.State = VerticalMirror(tempBoard.State);
                    newPosMirrored.Y = (ushort)(7 - newPos.Y);
                    pieceMirrored.CurrentPosition.Y = (ushort)(7 - piece.CurrentPosition.Y);
                }

                Dx = newPosMirrored.X - pieceMirrored.CurrentPosition.X;
                Dy = newPosMirrored.Y - pieceMirrored.CurrentPosition.Y;

                if (Dx == 0)
                {
                    for (int iy = pieceMirrored.CurrentPosition.Y + 1; iy < newPosMirrored.Y; iy++)
                    {
                        if (tempBoard.State[newPosMirrored.X, iy] == null)
                            continue;

                        if (iy < newPosMirrored.Y && tempBoard.State[newPosMirrored.X, iy].Side != pieceMirrored.Side)
                            Obstacle = true;

                        if (tempBoard.State[newPosMirrored.X, iy].Side == pieceMirrored.Side)
                            Obstacle = true;
                    }
                }
                else if (Dy == 0) // Connection line between moves is horizontal
                {
                    for (int ix = pieceMirrored.CurrentPosition.X + 1; ix < newPosMirrored.X; ix++)
                    {
                        if (tempBoard.State[ix, newPosMirrored.Y] == null)
                            continue;

                        if (ix < newPosMirrored.X && tempBoard.State[ix, newPosMirrored.Y].Side != pieceMirrored.Side)
                            Obstacle = true;

                        if (tempBoard.State[ix, newPosMirrored.Y].Side == pieceMirrored.Side)
                            Obstacle = true;
                    }
                }
                else if (Dy / Dx == 1) // Connection line between moves is diagonal
                {
                    int ix = pieceMirrored.CurrentPosition.X + 1;
                    int iy = pieceMirrored.CurrentPosition.Y + 1;

                    for (int i = pieceMirrored.CurrentPosition.X; i < newPosMirrored.X; i++)
                    {
                        if (tempBoard.State[ix, iy] == null)
                        {
                            ix++;
                            iy++;
                            continue;
                        }

                        if (tempBoard.State[ix, iy].Side == pieceMirrored.Side)
                            Obstacle = true;

                        if (ix < newPosMirrored.X && iy < newPosMirrored.Y && tempBoard.State[ix, iy].Side != pieceMirrored.Side)
                            Obstacle = true;

                        ix++;
                        iy++;
                    }
                }

            }


            // Check if in the end position of the move there is a piece of the same side
            if (board.State[newPos.X, newPos.Y]?.Side == piece.Side)
                Obstacle = true;

            if (!Obstacle)
            {
                board.State[newPos.X, newPos.Y] = piece;
                board.State[piece.CurrentPosition.X, piece.CurrentPosition.Y] = null;
                piece.CurrentPosition.X = newPos.X;
                piece.CurrentPosition.Y = newPos.Y;
                return true;
            }
            return false;
        }


        internal bool CheckKingsSafety(Piece pieceMoving, Position newPos, Chessboard board)
        {
            bool mat = false;
            Side defSide = pieceMoving.Side;
            Chessboard tempBoard = Chessboard.DeepClone(board);
            //Piece pieceMirrored = DeepClone(pieceMoving);
            King defKing = tempBoard.FindKing(defSide);

            int Dx = pieceMoving.CurrentPosition.X - defKing.CurrentPosition.X;
            int Dy = pieceMoving.CurrentPosition.Y - defKing.CurrentPosition.Y;

            // Perform mirror operations when needed
            if (Dx < 0)
                tempBoard.State = HorizontalMirror(tempBoard.State);
            if (Dy < 0)
                tempBoard.State = VerticalMirror(tempBoard.State);

            // Find king again if board was mirrored
            defKing = tempBoard.FindKing(defSide);

            // Store possible enemy piece that might has mat.
            Piece possibleMatPiece = null;

            if (Dx == 0)
            {
                for (int iy = defKing.CurrentPosition.Y+1; iy < 8; iy++)
                {
                    if (tempBoard.State[defKing.CurrentPosition.X, iy]?.Side != defSide)
                        possibleMatPiece = tempBoard.State[defKing.CurrentPosition.X, iy];
                }
            }
            else if (Dy == 0)
            {
                for (int ix = defKing.CurrentPosition.X + 1; ix < 8; ix++)
                {
                    if (tempBoard.State[ix, defKing.CurrentPosition.Y]?.Side != defSide)
                        possibleMatPiece = tempBoard.State[ix, defKing.CurrentPosition.Y];
                }
            }
            else
            {
                int ix = defKing.CurrentPosition.X + 1;
                int iy = defKing.CurrentPosition.Y + 1;

                for (int i = defKing.CurrentPosition.X; i < 8; i++)
                {
                    if (tempBoard.State[ix, iy] == null)
                    {
                        ix++;
                        iy++;
                        continue;
                    }

                    if (tempBoard.State[ix, iy].Side != defKing.Side)
                        possibleMatPiece = tempBoard.State[ix, iy];

                    ix++;
                    iy++;
                }
            }

            // Try to kill defendant King.


            return false;
        }

        #region "Helpers: Chessboard mirrors"
        /// <summary>
        /// Horizontally mirrors a chessboard.
        /// </summary>
        /// <param name="subBoard"></param>
        /// <returns></returns>
        private Piece[,] HorizontalMirror(Piece[,] subBoard)
        {
            int xLen = subBoard.GetLength(0);
            int yLen = subBoard.GetLength(1);

            Piece[,] mirroredSubBoard = new Piece[xLen, yLen];
            for (int i = 0; i < xLen; i++)
            {
                for (int j = 0; j < yLen; j++)
                {
                    mirroredSubBoard[xLen - 1 - i, j] = subBoard[i, j];
                }
            }
            return mirroredSubBoard;
        }

        /// <summary>
        /// Vertically mirrors a chessboard.
        /// </summary>
        /// <param name="subBoard"></param>
        /// <returns></returns>
        private Piece[,] VerticalMirror(Piece[,] subBoard)
        {
            int xLen = subBoard.GetLength(0);
            int yLen = subBoard.GetLength(1);

            Piece[,] mirroredSubBoard = new Piece[xLen, yLen];
            for (int j = 0; j < yLen; j++)
            {
                for (int i = 0; i < xLen; i++)
                {
                    mirroredSubBoard[i, yLen - 1 - j] = subBoard[i, j];
                }
            }
            return mirroredSubBoard;
        }
        #endregion
    }
}
