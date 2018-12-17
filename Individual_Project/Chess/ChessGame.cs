using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Chess
{
    public class ChessGame
    {
        // Properties
        public Chessboard Board { get; set; }
        public string CurrentUser { get; set; }
        public Side CurrentSide { get; set; }
        public string TurnUser { get; set; }
        private bool IsValidMove { get; set; }
        private bool HasChanged { get; set; }
        public Side? Winner { get; set; }


        /// <summary>
        /// Initializes the ChessGame class.
        /// </summary>
        public ChessGame()
        {
            Board = new Chessboard();
            IsValidMove = false;
            HasChanged = true;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Play()
        {

            // Check winning conditions
            if (!Board.IsBlackKingAlive())
            {
                Winner = Side.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("WHITE WINS!");
                Console.ResetColor();
                Console.ReadLine();
                return true;
            }
            else if (!Board.IsWhiteKingAlive())
            {
                Winner = Side.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BLACK WINS!");
                Console.ResetColor();
                Console.ReadLine();
                return true;
            }



            // Check if its the current user's turn to play
            if (!CurrentUser.Equals(TurnUser))
            {
                if (HasChanged)
                {
                    Console.Clear();
                    PrintChessboardHelper(CurrentSide);
                    Console.WriteLine($"Wait for your {TurnUser}'s move...");
                    HasChanged = !HasChanged;
                }
                return true;
            }
            else
            {
                HasChanged = true;
                Console.Clear();
                PrintChessboardHelper(CurrentSide);

                if (CurrentSide == Side.White)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.Write($"\nYour move {CurrentUser} (i.e. c2-c4): ");
            }

            // read user's move and do turn logic
            if (SquareMapping.ParseMove(Console.ReadLine(), out Position startPos, out Position endPos))
            {
                Console.ResetColor();
                if ((Board.State[startPos.X, startPos.Y]?.Side != CurrentSide) || !Board.Move(startPos, endPos))
                {
                    Console.WriteLine("Invalid move!");
                    IsValidMove = false;
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.Clear();
                    PrintChessboardHelper(CurrentSide);
                    IsValidMove = true;
                }
            }
            else
            {
                Console.WriteLine("Invalid move!");
                Thread.Sleep(1000);
                IsValidMove = false;
            }

            // Reset console foreground color
            Console.ResetColor();           

            return IsValidMove;
        }


        /// <summary>
        /// Returns a string with the current board state.
        /// </summary>
        /// <returns></returns>
        public string GetBoardState()
        {
            StringBuilder bs = new StringBuilder();
            for (int j = 0; j < Board.State.GetLength(1); j++)
            {
                for (int i = 0; i < Board.State.GetLength(0); i++)
                {
                    Piece p = Board.State[i, j];
                    bs.Append($"{i}{j}.{p?.GetType().Name}.{p?.Side.ToString()}-");
                }
            }
            bs.Remove(bs.Length - 1, 1);

            return bs.ToString();
        }


        /// <summary>
        /// Restores the boards state based on a saved one.
        /// </summary>
        /// <param name="boardState"></param>
        public void SetBoardState(string boardState)
        {
            string[] squares = boardState.Split('-');

            foreach (string square in squares)
            {
                // SPlit square info to piece info
                string[] pieceInfo = square.Split('.');

                // create piece's position object
                Position pos = new Position(ushort.Parse(pieceInfo[0].Substring(0, 1)), ushort.Parse(pieceInfo[0].Substring(1, 1)));

                // Determine if the square is null
                if (string.IsNullOrEmpty(pieceInfo[1]))
                {
                    Board.State[pos.X, pos.Y] = null;
                    continue;
                }

                // Assing the appropriate piece to the square under consideration
                Type type = null;
                if (pieceInfo[1] != null)
                {
                    switch (pieceInfo[1])
                    {
                        case "Pawn":
                            type = typeof(Pawn);
                            break;
                        case "Rook":
                            type = typeof(Rook);
                            break;
                        case "Knight":
                            type = typeof(Knight);
                            break;
                        case "Bishop":
                            type = typeof(Bishop);
                            break;
                        case "Queen":
                            type = typeof(Queen);
                            break;
                        case "King":
                            type = typeof(King);
                            break;
                        default:
                            break;
                    }
                }
                Side side = (Side)Enum.Parse(typeof(Side), pieceInfo[2]);
                Board.State[pos.X, pos.Y] = (Piece)Activator.CreateInstance(type, side, pos);
            }
        }


        #region "Drawing Methods"
        /// <summary>
        /// Prints the chessboard to console based on view of the current player.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="side"></param>
        private void PrintChessboardHelper(Side side)
        {
            if (side == Side.White)
                PrintChessboardWhiteView();
            else
                PrintChessboardBlackView();
        }


        /// <summary>
        /// Prints the chessboard from white's view.
        /// </summary>
        /// <param name="board"></param>
        private void PrintChessboardWhiteView()
        {
            Console.WriteLine();

            for (int col = 0; col < Board.State.GetLength(1); col++)
            {
                // Print each row of the chessboard
                Console.Write($"{Board.State.GetLength(1) - col}  ");

                for (int row = 0; row < Board.State.GetLength(0); row++)
                {
                    Piece curSquare = Board.State[row, col];

                    if (curSquare?.Side == Side.White)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" " + PieceUni.whitePieceUnicode[curSquare.GetType()]);
                        Console.ResetColor();
                    }
                    else if (curSquare?.Side == Side.Black)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" " + PieceUni.blackPieceUnicode[curSquare.GetType()]);
                        Console.ResetColor();
                    }
                    else
                        Console.Write("   ");

                    if (row != Board.State.GetLength(1) - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\u2502");
                        Console.ResetColor();
                    }
                }
                Console.Write("\n");
                PrintHorizontalLine();
            }
            Console.WriteLine("     A   B   C   D   E   F   G   H\n");
        }


        /// <summary>
        /// Prints the chessboard from black's view.
        /// </summary>
        /// <param name="board"></param>
        private void PrintChessboardBlackView()
        {
            Console.WriteLine();

            for (int row = Board.State.GetLength(1) - 1; row >= 0; row--)
            {
                // Print each row of the chessboard
                Console.Write($"{Board.State.GetLength(1) - row}  ");

                for (int col = Board.State.GetLength(0) - 1; col >= 0; col--)
                {
                    Piece curSquare = Board.State[col, row];

                    if (curSquare?.Side == Side.White)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" " + PieceUni.whitePieceUnicode[curSquare.GetType()]);
                        Console.ResetColor();
                    }
                    else if (curSquare?.Side == Side.Black)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" " + PieceUni.blackPieceUnicode[curSquare.GetType()]);
                        Console.ResetColor();
                    }
                    else
                        Console.Write("   ");

                    if (col != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\u2502");
                        Console.ResetColor();
                    }
                }
                Console.Write("\n");
                PrintHorizontalLine();
            }
            Console.WriteLine("     H   G   F   E   D   C   B   A\n");
        }


        /// <summary>
        /// Prints the horizontal lines in the chessboard.
        /// </summary>
        private void PrintHorizontalLine()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("    ");
            Console.Write("\u2500 ");
            Console.Write("\u253C ");
            Console.Write("\u2500 ");
            Console.Write("\u253C ");
            Console.Write("\u2500 ");
            Console.Write("\u253C ");
            Console.Write("\u2500 ");
            Console.Write("\u253C ");
            Console.Write("\u2500 ");
            Console.Write("\u253C ");
            Console.Write("\u2500 ");
            Console.Write("\u253C ");
            Console.Write("\u2500 ");
            Console.Write("\u253C ");
            Console.WriteLine("\u2500 ");
            Console.ResetColor();
        }
        #endregion
    }
}
