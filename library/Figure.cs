namespace ChessRules
{
    enum Figure
    {
        none,

        whiteKing = 'K',
        whiteQueen = 'Q',
        whiteRook = 'R',
        whiteBichop = 'B',
        whiteKnight = 'N',
        whitePawn = 'P',

        blackKing = 'k',
        blackQueen = 'q',
        blackRook = 'r',
        blackBichop = 'b',
        blackKnight = 'n',
        blackPawn = 'p'
    }

    static class FigueMethod
    {

        public static Color GetColor (this Figure figure)
        {
            if (figure == Figure.none)
                return Color.none;
            return (figure == Figure.whiteKing ||
                figure == Figure.whiteQueen ||
                figure == Figure.whiteRook ||
                figure == Figure.whiteBichop ||
                figure == Figure.whiteKnight ||
                figure == Figure.whitePawn)
                ? Color.white
                : Color.black;
        }
    }
}
