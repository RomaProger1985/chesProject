using System.Collections.Generic;
using System.Text;

namespace ChessRules
{
    public class Chess
    {
        public string Fen { get; private set; }
        public int MoveNumber { get; private set; }

        private Board board;
        private Moves moves;
        private List<FigureMoving> allMoves;
        private List<string> notation;

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            Fen = fen;
            board = new Board(fen);
            MoveNumber = board.MoveNumber;
            moves = new Moves(board);
            notation = new List<string>();
        }

        private Chess(Board board)

        {
            this.board = board;
            Fen = board.Fen;
            MoveNumber = board.MoveNumber;
            moves = new Moves(board);
        }

        public bool MoveWhile()
        {
            if(board.MoveColor == Color.white)
                return true;
            else
                return false;
        }

        public Chess Move(string move)// pe2e4 pe7e8Q
        {
            FigureMoving fm = new FigureMoving(move)
            {
                IsMove = true
            };
            //проверка promotion
            if(fm.IsMove)
            {
                if(move.Length > 5)
                {
                    if(fm.Figure == Figure.blackPawn || fm.Figure == Figure.whitePawn)
                    {
                        if(fm.Figure == Figure.whitePawn && fm.From.y != 6)
                            return this;
                        if(fm.Figure == Figure.blackPawn && fm.From.y != 1)
                            return this;
                    }
                    else
                    {
                        return this;
                    }
                }
            }

            if(!moves.CenMove(fm))
            {
                return this;
            }

            if(board.IsCheckAfterMove(fm))
            {
                return this;
            }

            Board nextBoard = board.Move(fm);
            MoveNumber = board.MoveNumber;
            SetNotation(fm, move);
            Chess nextChess = new Chess(nextBoard)
            {
                notation = notation
            };

            return nextChess;
        }

        private void SetNotation (FigureMoving fm , string move)
        {
            //parse move
            StringBuilder sb = new StringBuilder(move);
            if(board.GetFigureAt(new Squer(fm.To.x, fm.To.y)) == Figure.none)
                sb.Insert(3, "-");
            else
                sb.Insert(3, "x");
            //if move while
            if(fm.Figure.GetColor() == Color.white)
            {
                //continuation notation
                if(board.MoveNumber > 1)
                {
                    //if while castling
                    if(fm.Figure == Figure.whiteKing)
                    {
                        if(fm.DeltaX > 1)
                            notation.Add(" " + board.MoveNumber + "." + "0-0");
                        if(fm.DeltaX < -1)
                            notation.Add(" " + board.MoveNumber + "." + "0-0-0");
                    }
                    else
                    {
                        notation.Add(" " + board.MoveNumber + "." + " " + sb);
                    }
                }
                //start notation
                else
                    notation.Add(board.MoveNumber + "." + " " + sb);
            }
            //if move black
            if(fm.Figure.GetColor() == Color.black)
            {

                //if black castling
                if(fm.Figure == Figure.blackKing)
                {
                    if(fm.DeltaX > 1)
                        notation.Add(" " + "0-0");
                    if(fm.DeltaX < -1)
                        notation.Add(" " + "0-0-0");
                }
                else
                {
                    notation.Add(" " + sb);
                }

            }
        }

        public string GetNotation()
        {
            string list = "";
            foreach(string value in notation)
                list+= value;
            return list;
        }

        public char GetFigureAt(int x, int y)
        {
            Squer squer = new Squer(x, y);
            Figure f = board.GetFigureAt(squer);
            return f == Figure.none ? '.' : (char)f;
        }

        public char GetFigureAt(string position)
        {
            Squer squer = new Squer(position[0] - 'a', position[1] - '1');
            Figure f = board.GetFigureAt(squer);
            return f == Figure.none ? '.' : (char)f;
        }

        private void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            foreach(FigureOnSquare fs in board.YieldFigures())
            {
                foreach(Squer to in Squer.YieldSquares())
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if(moves.CenMove(fm))
                    {
                        if(!board.IsCheckAfterMove(fm))
                        {
                            allMoves.Add(fm);
                        }
                    }
                }
            }
        }
        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach(FigureMoving fm in allMoves)
            {
                list.Add(fm.ToString());
            }

            return list;
        }

        public List<string> GetAllMoves(int x, int y)
        {
            FindAllMoves();
            Squer squer = new Squer(x, y);
            Figure figure = (Figure)GetFigureAt(x, y);
            FigureOnSquare fs = new FigureOnSquare(figure, squer);
            FigureMoving figureMoving = new FigureMoving(fs, squer);
            List<string> list = new List<string>();
            foreach(FigureMoving fm in allMoves)
            {
                if(fm.Figure == figureMoving.Figure && fm.From == figureMoving.From)
                {
                    list.Add(fm.ToString());
                }
            }
            return list;
        }

        public bool IsCheck()
        {
            return board.Check;
        }
    }
}
