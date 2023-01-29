using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Text Notation;
    public Text Fen;
    public Text NumberPosition;

    private void Start()
    {
    }

    private void Update()
    {
    }
    public void ShowFen (string fen)
    {
        Fen.text = "";
        Fen.text = fen;
    }
}
