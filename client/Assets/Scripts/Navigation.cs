using ChessRules;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public List<string[]> NotationParsed;
    public Rules ScRules;
    public List<string> thisFen;
    public int NumberPosition;

    private string sendNotation = "";
    private Interface ScInterface;

    private void Start()
    {
        ScInterface = GetComponent<Interface>();
        ScRules = GetComponent<Rules>();
        thisFen = new List<string>
        {
            ScRules.chessBoard.Fen
        };
        NumberPosition = 0;
        ScInterface.Fen.text = thisFen[0];
        ScInterface.NumberPosition.text = NumberPosition.ToString();
    }

    private void Update()
    {

    }

    public void ParseNotationHendler(string data)
    {
        sendNotation = "";
        NotationParsed = new List<string[]>();
        Regex regex = new Regex(@"(\d*[.]\s\S*)\s*(\S*)");
        MatchCollection matchCollection = regex.Matches(data);
        foreach(Match value in matchCollection)
        {
            NotationParsed.Add(new string[2] { value.Groups[1].Value, value.Groups[2].Value });
        }
        thisFen.Add(ScRules.chessBoard.Fen);
        NumberPosition++;
        updateNotation();
    }

    private void updateNotation()
    {
        sendNotation = "";
        for(int x = 0; x < NotationParsed.Count; x++)
        {
            if(NumberPosition % 2 == 0)
            {
                if(NumberPosition / 2 == x)
                {
                    if(ScRules.chessBoard.MoveWhile())
                        sendNotation += NotationParsed[x][0] + " " + "<color=red>" + NotationParsed[x][1] + "</color>\n";
                    else
                        sendNotation += "<color=red>" + NotationParsed[x][0] + "</color> " + NotationParsed[x][1] + "\n";
                }
                else
                    sendNotation += NotationParsed[x][0] + " " + NotationParsed[x][1] + "\n";
            }
            else
            {
                if((NumberPosition - 1) / 2 == x)
                {
                    if(ScRules.chessBoard.MoveWhile())
                        sendNotation += NotationParsed[x][0] + " " + "<color=red>" + NotationParsed[x][1] + "</color>\n";
                    else
                        sendNotation += "<color=red>" + NotationParsed[x][0] + "</color> " + NotationParsed[x][1] + "\n";
                }
                else
                    sendNotation += NotationParsed[x][0] + " " + NotationParsed[x][1] + "\n";
            }
        }
        ScInterface.Notation.text = sendNotation;
        ScInterface.Fen.text = thisFen[NumberPosition];
        ScInterface.NumberPosition.text = NumberPosition.ToString();
    }

    public void OnNext()
    {
        if((NumberPosition + 1) < thisFen.Count)
        {
            goTo(NumberPosition + 1);
            NumberPosition++;
            updateNotation();
        }

    }

    public void OnBack()
    {
        if((NumberPosition - 1) >= 0)
        {
            goTo(NumberPosition - 1);
            NumberPosition--;
        }
        updateNotation();
    }

    public void ToTheBegining()
    {
        goTo(0);
        NumberPosition = 0;
        updateNotation();
    }

    public void Restar()
    {
        ScRules.chessBoard = new Chess("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        thisFen = new List<string>
        {
            ScRules.chessBoard.Fen
        };
        NumberPosition = 0;
        ScRules.Reload();
    }

    private void goTo(int NPosition)
    {
        ScRules.chessBoard = new Chess(thisFen[NPosition]);
        ScRules.Reload();
    }
}
