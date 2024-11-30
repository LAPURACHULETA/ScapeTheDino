using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodigoPuzle : MonoBehaviour
{
    public static CodigoPuzle Instance { get; private set; }

    [SerializeField] private TMP_Text text;
    [SerializeField] private string code;
    [SerializeField] private GameObject keyPad;
    [SerializeField] private GameObject doorKeypad;
    [SerializeField] private float timeResetCode;

    public bool puzzleComplete;
    public bool reset;
    private void Awake()
    {
        // Implementación Singleton
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            ///Destroy(gameObject); // Si ya existe una instancia, destruye este objeto.
        }
    }
    private void Start()
    {
       
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
            GameManager.Instance.changeState(GameManager.State.InGame);
        }
        if(text.text != code)
        {
            text.text = "Error";
            reset = false;
            GameManager.Instance.changeState(GameManager.State.InGame);
            keyPad.gameObject.SetActive(false);
        }
       
    }
    private IEnumerator TimeReset()
    {
        yield return new WaitForSeconds(timeResetCode);
        text.text = "";  
        reset = true;
    }
    //public bool GetCompletePuzzles(bool name)
    //{
    //    puzzleComplete = name;
    //    // Debug.Log(nameoftag);
    //    return puzzleComplete;
    //}
    //public bool SetCompletePuzzles()
    //{
    //    Debug.Log(puzzleComplete);
    //    return puzzleComplete;

    //}
}
