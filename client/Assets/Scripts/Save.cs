using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class Save : MonoBehaviour {

    public Text Pgn;
    public string Path = "";
    public string NameFile = "test.txt";

    // Use this for initialization
    void Start () {
		
	}

        // Update is called once per frame
        void Update () {
		
	}

    public void OnSave()
    { // функция сохранения
        StreamWriter sw = new StreamWriter(Path + "/" + NameFile); // создаём файл
        sw.WriteLine(Pgn.text); // записываем в файл строку
        sw.Close(); // закрываем файл
    }

    public void OnLoad() // функция чтения
    {
        StreamReader sr = new StreamReader(Path + "/" + NameFile); // открываем файл
        Pgn.text = sr.ReadToEnd();
        sr.Close(); // закрываем файл
    }
}
