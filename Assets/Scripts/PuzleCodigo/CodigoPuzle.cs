using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodigoPuzle : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private string code;
    [SerializeField] private GameObject keyPad;
    [SerializeField] private GameObject doorKeypad;

    public bool puzzleComplete;
    public void Number(int number)
    {
        text.text += number.ToString();
    }
    public void Execute()
    {
        if (text.text == code)
        {
            text.text = "Correct";
            Destroy(doorKeypad);
            keyPad.gameObject.SetActive(false);
            puzzleComplete = true;  
        }
        else
        {
            text.text = "";
        }
    }
}
