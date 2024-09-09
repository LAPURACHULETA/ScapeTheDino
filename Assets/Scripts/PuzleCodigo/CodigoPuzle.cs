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
    [SerializeField] private float timeResetCode;

    public bool puzzleComplete;
    public bool reset;
    GameManager manager;
    private void Start()
    {
        manager=FindObjectOfType<GameManager>();
        reset=true;
    }
    private void Update()
    {
        if (reset==false)
        {
            StartCoroutine(TimeReset());
        }
    }
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
            manager.changeState(GameManager.State.InGame);
        }
        if(text.text != code)
        {
            text.text = "Error";
            reset = false;   
        }
       
    }
    private IEnumerator TimeReset()
    {
        yield return new WaitForSeconds(timeResetCode);
        text.text = "";  
        reset = true;
    }
}
