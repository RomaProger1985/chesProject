using System;
using System.Collections.Generic;
using System.Text;

namespace ChessRules
{
    internal class Board
    {
        public string Fen { get; private set; }

        public MovedFigure[,] Figures { get; private set; }
        public Color MoveColor { get; set; }
        public int MoveNumber { get; private set; }

        public bool Check { get; private set; }

        //public bool IsCheck { get; private set; }

        public Board(string fen)
        {
            Fen = fen;
            Figures = new MovedFigure[8, 8];
            Init();
        }

        public Board Move(FigureMoving fm)
        {

            Board next = new Board(Fen);
            next.SetFigureAt(fm.From, Figure.none);
            next.SetFigureAt(fm.To, fm.Promotion == Figure.none ? fm.Figure : fm.Promotion);
            if(MoveColor == Color.black)
                next.MoveNumber++;
            next.MoveColor = MoveColor.FlipColor();
            if(fm.IsMove)
            {
                Figures[fm.To.x, fm.To.y].CountMoved = ++Figures[fm.From.x, fm.From.y].CountMoved;
                Figures[fm.To.x, fm.To.y].LastMove = true;
                Figures[fm.From.x, fm.From.y].CountMoved = 0;
                for(int x = 0; x < 8; x++)
                    for(int y = 0; y < 8; y++)
                    {
                        next.Figures[x, y].CountMoved = Figures[x, y].CountMoved;
                        if(fm.To.x == x && fm.To.y == y)
                            next.Figures[x, y].LastMove = Figures[x, y].LastMove;
                    }
            }
            next.GenerateFen();
            if(next.IsCheck())
                next.Check = true;
            return next;
        }

        private void Init()
        {
            // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            // 0                                           1 2    3 4 5
            string[] parts = Fen.Split();
            if(parts.Length != 6) return;
            InitFigures(parts[0]);
            MoveColor = (parts[1] == "b") ? Color.black : Color.white;
            MoveNumber = int.Parse(parts[5]);
        }

        public void GenerateFen()
        {
            Fen = FenFigures() + " " +
                (MoveColor == Color.white ? "w" : "b") +
                " - - 0 " + MoveNumber.ToString();
        }

        private string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            for(int y = 7; y >= 0; y--)
            {
                for(int x = 0; x < 8; x++)
                    sb.Append(Figures[x, y].Figure == Figure.none ? '1' : (char)Figures[x, y].Figure);
                if(y > 0)
                    sb.Append('/');
            }
            string eight = "11111111";
            for(int j = 8; j >= 2; j--)
                sb.Replace(eight.Substring(0, j), j.ToString());
            return sb.ToString();
        }

        public IEnumerable<FigureOnSquare> YieldFigures()
        {
            foreach(Squer squer in Squer.YieldSquares())
                if(GetFigureAt(squer).GetColor() == MoveColor)
                    yield return new FigureOnSquare(GetFigureAt(squer), squer);
        }
        public IEnumerable<FigureOnSquare> YieldBadFigures()
        {
            foreach(Squer squer in Squer.YieldSquares())
                if(GetFigureAt(squer) != Figure.none)
                if(GetFigureAt(squer).GetColor() != MoveColor)
                    yield return new FigureOnSquare(GetFigureAt(squer), squer);
        }

        private void InitFigures(string data)
        {
            for(int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');
            for(int y = 7; y >= 0; y--)
                for(int x = 0; x < 8; x++)
                    Figures[x, y] = new MovedFigure(
                        lines[7 - y][x] == '.' ? Figure.none :
                        (Figure)lines[7 - y][x], new Vector2(x, y));
        }

        public Figure GetFigureAt(Squer squer)
        {
            if(squer.OnBoard())
                return Figures[squer.x, squer.y].Figure;
            return Figure.none;
        }

        public MovedFigure GetFigureAt(Vector2 position)
        {
            return Figures[position.x, position.y];
        }

        public void SetFigureAt(Squer squer, Figure figure)
        {
            if(squer.OnBoard())
                Figures[squer.x, squer.y].Figure = figure;
        }

        private bool IsCheck()
        {
            Board affter = new Board(Fen)
            {
                MoveColor = MoveColor.FlipColor()
            };
            return affter.CenEaKing();
        }

        public bool CenEaKing()
        {
            Squer badKing = FinBadKing();
            Moves moves = new Moves(this);
            foreach(FigureOnSquare fs in YieldFigures())
            {
                FigureMoving fm = new FigureMoving(fs, badKing);
                if(moves.CenMove(fm))
                    return true;
            }
            return false;

        }
        public bool CenEatSquare(Squer squer)
        {
            Moves moves = new Moves(this);
            foreach(FigureOnSquare fs in YieldBadFigures())
            {
                FigureMoving fm = new FigureMoving(fs, squer);
                MoveColor = MoveColor.FlipColor();
                if(moves.CenMove(fm))
                {
                    MoveColor = MoveColor.FlipColor();
                    return true;
                }
                MoveColor = MoveColor.FlipColor();
            }
            
            return false;

        }

        private Squer FinBadKing()
        {
            Figure badKing = MoveColor == Color.black ? Figure.whiteKing : Figure.blackKing;
            foreach(Squer squer in Squer.YieldSquares())
                if(GetFigureAt(squer) == badKing)
                    return squer;
            return Squer.none;
        }

        public bool IsCheckAfterMove(FigureMoving fm)
        {
            Board after = Move(fm);
            return after.CenEaKing();
        }
    }
}

