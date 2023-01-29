
namespace ChessRules
{
    class MovedFigure
    {
        public Figure Figure { get; set; }
        public Vector2 Position { get; private set; }
        public int CountMoved;
        public bool LastMove;

        public MovedFigure (Figure figure,Vector2 position)
        {
            Figure = figure;
            Position = position;
            CountMoved = 0;
            LastMove = false;
        }

        public void SetFigure (Figure figure)
        {
            Figure = figure;
        }
    }
}
