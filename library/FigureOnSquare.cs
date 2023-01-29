namespace ChessRules
{
    class FigureOnSquare
    {
        public Figure Figure { get; private set; }
        public Squer Squer { get; private set; }
        public FigureOnSquare (Figure figure, Squer squer)
        {
            Figure = figure;
            Squer = squer;
        }
    }
}
