
using System;

namespace ChessRules
{
   class FigureMoving
    {
        public Figure Figure { get; private set; }
        public Squer From { get; private set; }
        public Squer To { get; private set; }
        public bool IsMove { get; set; }

        public Figure Promotion { get; private set; }

        public FigureMoving(FigureOnSquare fs, Squer to,Figure promotion = Figure.none)
        {
            Figure = fs.Figure;
            From = fs.Squer;
            To = to;
            Promotion = promotion;
        }
        public FigureMoving (string move)// Pe2e4 Pe7e8Q
        {                                // 01234 012345
            Figure = (Figure)move[0];
            From = new Squer(move.Substring(1, 2));
            To = new Squer(move.Substring(3, 2));
            Promotion = (move.Length == 6) ? (Figure)move[5] : Figure.none;
        }

        public int DeltaX { get { return To.x - From.x; } }
        public int DeltaY { get { return To.y - From.y; } }

        public int AbsDeltaX {get{ return Math.Abs(DeltaX); } }
        public int AbsDeltaY { get { return Math.Abs(DeltaY); } }

        public int SignX { get { return Math.Sign(DeltaX); } }
        public int SignY { get { return Math.Sign(DeltaY); } }

        public override string ToString()
        {
            string text = (char)Figure + From.Name + To.Name;
            if (Promotion != Figure.none)
                text += (char)Promotion;
            return text;
        }
    }
}
