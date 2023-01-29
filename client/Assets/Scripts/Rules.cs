using ChessRules;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    private delegate void DInterface(string data);

    private event DInterface EvInterface;

    private Navigation ScNavigation;

    public Chess chessBoard;
    private DragAndDrop dragAndDrop;
    public GameObject SelectWhileFigures;
    public GameObject SelectBlackFigures;
    private bool selectPromotion;
    private bool shovMoveFigures;
    private bool shovMoveFigure;
    public int WoveNumber;

    public Rules()
    {
        dragAndDrop = new DragAndDrop();
        chessBoard = new Chess("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    private void Start()
    {
        ScNavigation = GetComponent<Navigation>();
        EvInterface = ScNavigation.ParseNotationHendler;

        selectPromotion = false;
        ShowFigures();
        MarkAllMoves();
    }

    private void Update()
    {
        if(dragAndDrop.Action())
        {
            string from = GetSquare(dragAndDrop.pickPosition);
            string to = GetSquare(dragAndDrop.dropPosition);
            string figure = chessBoard.GetFigureAt(from).ToString();
            string move = figure + from + to;
            if(to[1] == '8' && figure == "P")
            {
                SelectWhileFigures.SetActive(true);
                selectPromotion = true;
            }
            if(to[1] == '1' && figure == "p")
            {
                SelectBlackFigures.SetActive(true);
                selectPromotion = true;
            }
            if(!selectPromotion)
            {
                string oldFen = chessBoard.Fen;
                chessBoard = chessBoard.Move(move);
                ShowFigures();
                MarkAllMoves();
                if(oldFen != chessBoard.Fen)
                    EvInterface(chessBoard.GetNotation());
            }
        }
        if(dragAndDrop.StartDrag)
        {
            Vector2 from;
            from.x = Convert.ToInt32(dragAndDrop.pickPosition.x / 2.0);
            from.y = Convert.ToInt32(dragAndDrop.pickPosition.y / 2.0);
            MarkAllMovesFigure(from);
        }
    }

    public void Move(string move)
    {
        bool castling = false;
        if(chessBoard.MoveWhile())
        {
            if(move == "0-0")
            {
                chessBoard = chessBoard.Move("Ke1g1");
                castling = true;
                ShowFigures();
                MarkAllMoves();
            }
            if(move == "0-0-0")
            {
                chessBoard = chessBoard.Move("Ke1c1");
                castling = true;
                ShowFigures();
                MarkAllMoves();
            }
        }
        else
        {
            if(move == "0-0")
            {
                chessBoard = chessBoard.Move("ke8g8");
                castling = true;
                ShowFigures();
                MarkAllMoves();
            }
            if(move == "0-0-0")
            {
                chessBoard = chessBoard.Move("ke8c8");
                castling = true;
                ShowFigures();
                MarkAllMoves();
            }
        }
        if(!castling)
        {
            chessBoard = chessBoard.Move(move);
            ShowFigures();
            MarkAllMoves();
        }
    }

    public void ShovMoveFigures()
    {
        shovMoveFigures = !shovMoveFigures;
        MarkAllMoves();
    }
    public void ShovMoveFigure()
    {
        shovMoveFigure = !shovMoveFigure;
    }

    public void PromotionQ()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'Q');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectWhileFigures.SetActive(false);
        selectPromotion = false;
    }
    public void PromotionR()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'R');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectWhileFigures.SetActive(false);
        selectPromotion = false;
    }
    public void PromotionB()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'B');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectWhileFigures.SetActive(false);
        selectPromotion = false;
    }
    public void PromotionN()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'N');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectWhileFigures.SetActive(false);
        selectPromotion = false;
    }
    public void PromotionP()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'P');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectWhileFigures.SetActive(false);
        selectPromotion = false;
    }
    public void Promotionq()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'q');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectBlackFigures.SetActive(false);
        selectPromotion = false;
    }
    public void Promotionr()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'r');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectBlackFigures.SetActive(false);
        selectPromotion = false;
    }
    public void Promotionb()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'b');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectBlackFigures.SetActive(false);
        selectPromotion = false;
    }
    public void Promotionn()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'n');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectBlackFigures.SetActive(false);
        selectPromotion = false;
    }
    public void Promotionp()
    {
        string from = GetSquare(dragAndDrop.pickPosition);
        string to = GetSquare(dragAndDrop.dropPosition);
        string figure = chessBoard.GetFigureAt(from).ToString();
        string move = figure + from + to;
        chessBoard = chessBoard.Move(move + 'p');
        ShowFigures();
        MarkAllMoves();
        EvInterface(chessBoard.GetNotation());
        SelectBlackFigures.SetActive(false);
        selectPromotion = false;
    }

    public void Reload()
    {
        ShowFigures();
        MarkAllMoves();
    }

    private string GetSquare(Vector2 pickPosition)
    {
        int x = Convert.ToInt32(pickPosition.x / 2.0);
        int y = Convert.ToInt32(pickPosition.y / 2.0);
        return ((char)('a' + x)).ToString() + (y + 1).ToString();
    }

    private void ShowFigures()
    {
        int nr = 0;
        for(int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x++)
            {
                string figure = chessBoard.GetFigureAt(x, y).ToString();
                if(figure == ".")
                {
                    continue;
                }

                PlaceFigure("box" + nr, figure, x, y);
                nr++;
            }
        }

        for(; nr < 32; nr++)
        {
            PlaceFigure("box" + nr, "q", 9, 9);
        }
    }

    private void MarkAllMoves()
    {
        for(int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x++)
            {
                MarkSquare(x, y, false);
            }
        }
        if(shovMoveFigures)
        {
            foreach(string moves in chessBoard.GetAllMoves())
            {
                int x, y;
                GetCoord(moves.Substring(1, 2), out x, out y);
                MarkSquare(x, y, true);
            }
        }
    }
    private void MarkAllMovesFigure(Vector2 gouingFigure)
    {
        for(int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x++)
            {
                MarkSquare(x, y, false);
            }
        }
        if(shovMoveFigure)
        {
            foreach(string moves in chessBoard.GetAllMoves((int)gouingFigure.x, (int)gouingFigure.y))
            {
                int x, y;
                x = moves[3] - 'a';
                y = moves[4] - '1';
                MarkSquare(x, y, true);
            }
        }
    }

    private void GetCoord(string name, out int x, out int y)
    {
        x = 9;
        y = 9;
        if(name.Length == 2 &&
            name[0] >= 'a' && name[0] <= 'h' &&
            name[1] >= '1' && name[1] <= '8')
        {
            x = name[0] - 'a';
            y = name[1] - '1';
        }
    }

    private void PlaceFigure(string box, string figure, int x, int y)
    {
        GameObject goBox = GameObject.Find(box);
        GameObject goFigure = GameObject.Find(figure);
        GameObject goSquare = GameObject.Find("" + y + x);

        SpriteRenderer spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;

        goBox.transform.position = goSquare.transform.position;
    }

    private void MarkSquare(int x, int y, bool isMarked)
    {
        GameObject goSquare = GameObject.Find("" + y + x);
        GameObject goCell;
        string color = (x + y) % 2 == 0 ? "Black" : "White";
        if(isMarked)
        {
            goCell = GameObject.Find(color + "SquareMarked");
        }
        else
        {
            goCell = GameObject.Find(color + "Square");
        }

        SpriteRenderer spriteSquare = goSquare.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteCell = goCell.GetComponent<SpriteRenderer>();
        spriteSquare.sprite = spriteCell.sprite;
    }

}

internal class DragAndDrop
{
    private enum State
    {
        none,
        drag
    }
    public Vector2 pickPosition { get; private set; }
    public Vector2 dropPosition { get; private set; }
    public bool StartDrag { get; private set; }


    private State state;
    private GameObject item;
    private Vector2 offset;

    public DragAndDrop()
    {
        state = State.none;
        item = null;
        StartDrag = false;
    }

    public bool Action()
    {
        switch(state)
        {
            case State.none:
                if(IsMouseButtonPressed())
                {
                    PickUp();
                }

                break;

            case State.drag:
                if(IsMouseButtonPressed())
                {
                    Drag();
                }
                else
                {
                    Drop();
                    return true;
                }
                break;
        }
        return false;
    }

    private void Drop()
    {
        dropPosition = item.transform.position;
        state = State.none;
        item = null;
    }

    private void Drag()
    {
        StartDrag = false;
        item.transform.position = GetClickPosition() + offset;
    }

    private void PickUp()
    {
        Vector2 clickPosition = GetClickPosition();
        Transform clickedItem = GetItemnAt(clickPosition);
        if(clickedItem == null)
        {
            return;
        }
        StartDrag = true;
        pickPosition = clickedItem.position;
        item = clickedItem.gameObject;
        state = State.drag;
        offset = pickPosition - clickPosition;
    }

    private Transform GetItemnAt(Vector2 position)
    {
        RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);
        if(figures.Length == 0)
        {
            return null;
        }

        return figures[0].transform;
    }

    private Vector2 GetClickPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool IsMouseButtonPressed()
    {
        return Input.GetMouseButton(0);
    }

}