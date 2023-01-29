using System;

namespace ChessRules
{
    internal class Moves
    {
        private FigureMoving fm;
        private Board board;

        public Moves(Board board)
        {
            this.board = board;
        }

        public bool CenMove(FigureMoving fm)
        {
            this.fm = fm;
            return
                CenMoveFrom() &&
                CenMoveTo() &&
                CenFigureMove();
        }

        private bool CenFigureMove()
        {
                   switch(fm.Figure)
            {
                case Figure.whiteKing:
                case Figure.blackKing:
                    return CenKingMove();

                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraigMove();

                case Figure.whiteRook:
                case Figure.blackRook:
                    return (fm.SignX == 0 || fm.SignY == 0) &&
                        CanStraigMove();

                case Figure.whiteBichop:
                case Figure.blackBichop:
                    return (fm.SignX != 0 && fm.SignY != 0) &&
                        CanStraigMove();

                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return CenKnightMove();

                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CenPawnMove();

                default: return false;
            }
        }

        private bool CenPawnMove()
        {
            if(fm.From.y < 1 || fm.From.y > 6)
            {
                return false;
            }

            int stepY = fm.Figure.GetColor() == Color.white ? 1 : -1;
            return
                CenPawnGo(stepY) ||
                CenPawnJump(stepY) ||
                CenPawnEat(stepY) ||
                CenPassent(fm, stepY);
        }

        public bool CenPassent(FigureMoving fm, int stepY)
        {
            // если идем в правильном направлении
            if(fm.DeltaY == stepY)
                // если клетка куда идем пустая
                if(board.GetFigureAt(fm.To) == Figure.none)
                    // если в лево или в право сместились на 1
                    if(fm.AbsDeltaX == 1)
                        // если находимся на правильной горизонтали
                        if((fm.From.y == 3 && fm.Figure.GetColor() == Color.black) ||
                            (fm.From.y == 4 && fm.Figure.GetColor() == Color.white))
                        {
                            Squer squerFm = new Squer(fm.To.x, fm.From.y);
                            // если с боку от нас вражеская пешка
                            if((board.GetFigureAt(squerFm) == Figure.blackPawn &&
                                fm.Figure.GetColor() == Color.white) ||
                                (board.GetFigureAt(squerFm) == Figure.whitePawn &&
                                 fm.Figure.GetColor() == Color.black))
                                //если она прыгнула
                                if(board.GetFigureAt
                                    (new Vector2(squerFm.x, squerFm.y)).CountMoved < 2 &&
                                    board.GetFigureAt
                                    (new Vector2(squerFm.x, squerFm.y)).LastMove)
                                {
                                    if(fm.IsMove)
                                    {
                                        board.SetFigureAt(squerFm, Figure.none);
                                        board.GenerateFen();
                                    }
                                    return true;
                                }
                        }
            return false;
        }

        private bool CenPawnEat(int stepY)
        {
            if(board.GetFigureAt(fm.To) != Figure.none)
            {
                if(fm.AbsDeltaX == 1)
                {
                    if(fm.DeltaY == stepY)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CenPawnJump(int stepY)
        {
            if(board.GetFigureAt(fm.To) == Figure.none)
            {
                if(fm.DeltaX == 0)
                {
                    if(fm.DeltaY == 2 * stepY)
                    {
                        if(fm.From.y == 1 || fm.From.y == 6)
                        {
                            if(board.GetFigureAt(new Squer(fm.From.x, fm.From.y + stepY)) == Figure.none)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool CenPawnGo(int stepY)
        {
            if(board.GetFigureAt(fm.To) == Figure.none)
            {
                if(fm.DeltaX == 0)
                {
                    if(fm.DeltaY == stepY)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanStraigMove()
        {
            Squer at = fm.From;
            do
            {
                at = new Squer(at.x + fm.SignX, at.y + fm.SignY);
                if(at == fm.To)
                {
                    return true;
                }
            } while(at.OnBoard() &&
                     board.GetFigureAt(at) == Figure.none);
            return false; ;
        }

        private bool CenKnightMove()
        {
            if(fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2)
            {
                return true;
            }

            if(fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1)
            {
                return true;
            }

            return false;
        }

        private bool CenKingMove()
        {
            if(fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1)
            {
                return true;
            }
            if(CenCastling())
                return true;
            return false;
        }

        private bool CenCastling()
        {
            if(!board.Check)
                if(board.MoveColor == Color.white)
                {
                    //0-0 white
                    if(fm.To.x == 6 && fm.To.y == 0)
                    {
                        if(board.GetFigureAt(new Vector2(fm.From.x, fm.From.y)).CountMoved == 0)
                            if(board.GetFigureAt(new Squer(7, 0)) != Figure.none &&
                               board.GetFigureAt(new Vector2(7, 0)).CountMoved == 0 &&
                               board.GetFigureAt(new Squer(7, 0)) == Figure.whiteRook)
                                if(board.GetFigureAt(new Squer(5, 0)) == Figure.none &&
                                   board.GetFigureAt(new Squer(6, 0)) == Figure.none)
                                    if(!board.CenEatSquare(new Squer(5, 0)))
                                        if(!board.CenEatSquare(new Squer(6, 0)))
                                        {
                                            if(fm.IsMove)
                                            {
                                                board.SetFigureAt(new Squer(7, 0), Figure.none);
                                                board.SetFigureAt(new Squer(5, 0), Figure.whiteRook);
                                                board.GenerateFen();
                                            }
                                            return true;
                                        }
                    }
                    else
                        //0-0-0 white
                        if(fm.To.x == 2 && fm.To.y == 0)
                    {
                        if(board.GetFigureAt(new Vector2(fm.From.x, fm.From.y)).CountMoved == 0)
                            if(board.GetFigureAt(new Squer(0, 0)) != Figure.none &&
                               board.GetFigureAt(new Vector2(0, 0)).CountMoved == 0 &&
                               board.GetFigureAt(new Squer(0, 0)) == Figure.whiteRook)
                                if(board.GetFigureAt(new Squer(1, 0)) == Figure.none &&
                                   board.GetFigureAt(new Squer(2, 0)) == Figure.none &&
                                   board.GetFigureAt(new Squer(3, 0)) == Figure.none)
                                    if(!board.CenEatSquare(new Squer(1, 0)))
                                        if(!board.CenEatSquare(new Squer(2, 0)))
                                            if(!board.CenEatSquare(new Squer(3, 0)))
                                            {
                                    if(fm.IsMove)
                                    {
                                        board.SetFigureAt(new Squer(0, 0), Figure.none);
                                        board.SetFigureAt(new Squer(3, 0), Figure.whiteRook);
                                        board.GenerateFen();
                                    }
                                    return true;
                                }
                    }
                }
                else
                        if(board.MoveColor == Color.black)
                {
                    //0-0 black
                    if(fm.To.x == 6 && fm.To.y == 7)
                    {
                        if(board.GetFigureAt(new Vector2(fm.From.x, fm.From.y)).CountMoved == 0)
                            if(board.GetFigureAt(new Squer(7, 7)) != Figure.none &&
                               board.GetFigureAt(new Vector2(7, 7)).CountMoved == 0 &&
                               board.GetFigureAt(new Squer(7, 7)) == Figure.blackRook)
                                if(board.GetFigureAt(new Squer(5, 7)) == Figure.none &&
                                   board.GetFigureAt(new Squer(6, 7)) == Figure.none)
                                    if(!board.CenEatSquare(new Squer(5, 7)))
                                        if(!board.CenEatSquare(new Squer(6, 7)))
                                        {
                                            if(fm.IsMove)
                                    {
                                        board.SetFigureAt(new Squer(7, 7), Figure.none);
                                        board.SetFigureAt(new Squer(5, 7), Figure.blackRook);
                                        board.GenerateFen();
                                    }
                                    return true;
                                }
                    }
                    else
                        //0-0-0 black
                        if(fm.To.x == 2 && fm.To.y == 7)
                    {
                        if(board.GetFigureAt(new Vector2(fm.From.x, fm.From.y)).CountMoved == 0)
                            if(board.GetFigureAt(new Squer(0, 7)) != Figure.none &&
                               board.GetFigureAt(new Vector2(0, 7)).CountMoved == 0 &&
                               board.GetFigureAt(new Squer(0, 7)) == Figure.blackRook)
                                if(board.GetFigureAt(new Squer(1, 7)) == Figure.none &&
                                   board.GetFigureAt(new Squer(2, 7)) == Figure.none &&
                                   board.GetFigureAt(new Squer(3, 7)) == Figure.none)
                                    if(!board.CenEatSquare(new Squer(1, 7)))
                                        if(!board.CenEatSquare(new Squer(2, 7)))
                                            if(!board.CenEatSquare(new Squer(3, 7)))
                                            {
                                    if(fm.IsMove)
                                    {
                                        board.SetFigureAt(new Squer(0, 7), Figure.none);
                                        board.SetFigureAt(new Squer(3, 7), Figure.blackRook);
                                        board.GenerateFen();
                                    }
                                    return true;
                                }
                    }
                }
            return false;
        }

        private bool CenMoveTo()
        {
            return fm.From.OnBoard() &&
                board.GetFigureAt(fm.To).GetColor() != board.MoveColor;
        }

        private bool CenMoveFrom()
        {
            return fm.From.OnBoard() &&
                board.GetFigureAt(new Squer(fm.From.x, fm.From.y)) == fm.Figure &&
                fm.Figure.GetColor() == board.MoveColor;
        }
    }
}
