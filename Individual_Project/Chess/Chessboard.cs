using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    //
    // !!! We always view the chessboard from white's side. !!!
    //
    [Serializable]
    public class Chessboard : INotifyPropertyChanged
    {
        // Fields
        private Piece[,] _state;

        // Properties
        public Piece[,] State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged(); }
        }

        #region "INotifyPropertyChanged Implementation"
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public Chessboard()
        {

            State = new Piece[8, 8];
            // Initialize black's side
            State[0, 0] = new Rook(Side.Black, new Position(0, 0));
            State[1, 0] = new Knight(Side.Black, new Position(1, 0));
            State[2, 0] = new Bishop(Side.Black, new Position(2, 0));
            State[3, 0] = new Queen(Side.Black, new Position(3, 0));
            State[4, 0] = new King(Side.Black, new Position(4, 0));
            State[5, 0] = new Bishop(Side.Black, new Position(5, 0));
            State[6, 0] = new Knight(Side.Black, new Position(6, 0));
            State[7, 0] = new Rook(Side.Black, new Position(7, 0));

            for (ushort i = 0; i < 8; i++)
            {
                State[i, 1] = new Pawn(Side.Black, new Position(i, 1));
            }

            // Initialize white's side
            State[0, 7] = new Rook(Side.White, new Position(0, 7));
            State[1, 7] = new Knight(Side.White, new Position(1, 7));
            State[2, 7] = new Bishop(Side.White, new Position(2, 7));
            State[3, 7] = new Queen(Side.White, new Position(3, 7));
            State[4, 7] = new King(Side.White, new Position(4, 7));
            State[5, 7] = new Bishop(Side.White, new Position(5, 7));
            State[6, 7] = new Knight(Side.White, new Position(6, 7));
            State[7, 7] = new Rook(Side.White, new Position(7, 7));

            for (ushort i = 0; i < 8; i++)
            {
                State[i, 6] = new Pawn(Side.White, new Position(i, 6));
            }
        }

        public bool Move(Position startSquare, Position endSquare)
        {
            if (State[startSquare.X, startSquare.Y] == null)
                return false;
            return State[startSquare.X, startSquare.Y].ValidateMove(endSquare, this);
        }

        public King FindKing(Side side)
        {
            for (int i = 0; i < State.GetLength(0); i++)
            {
                for (int j = 0; j < State.GetLength(1); j++)
                {
                    if (State[i, j]?.GetType() == typeof(King)
                        & State[i, j]?.Side == side)
                    {
                        return State[i,j] as King;
                    }
                }
            }
            return null;
        }
        
        public bool IsBlackKingAlive()
        {
            for (int i = 0; i < State.GetLength(0); i++)
            {
                for (int j = 0; j < State.GetLength(1); j++)
                {
                    if (State[i,j]?.GetType() == typeof(King)
                        & State[i,j]?.Side == Side.Black)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsWhiteKingAlive()
        {
            for (int i = 0; i < State.GetLength(0); i++)
            {
                for (int j = 0; j < State.GetLength(1); j++)
                {
                    if (State[i, j]?.GetType() == typeof(King)
                        & State[i, j]?.Side == Side.White)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Chessboard DeepClone<Chessboard>(Chessboard other)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;

                return (Chessboard)formatter.Deserialize(ms);
            }
        }
    }
}
