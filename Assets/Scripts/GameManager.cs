using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject craftingObj;
    [SerializeField] private GameObject inventoryObj;
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject winner;

    public enum State
    {
        None,
        Resume,
        Pause,
        GameOver,
        Win
    }
    
    public State state;
    public void changeState(State newState)
    {
        if (newState == state)
        {
            return;
        }

        state = newState;

        switch (state)
        {
            case State.None:
                ButtonResume();
                break;
            case State.Resume:
                ButtonResume();
                break;
            case State.Pause:
                ButtonPause();
                break;
            case State.GameOver:
                GameOver();
                break;
            case State.Win:
                Winner();
                break;
        }
    }
    public void ButtonPlay()
    {
        SceneManager.LoadScene(1);
        ButtonResume();
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
    public void ButtonPause()
    {
        Time.timeScale = 0;
    }
    public void ButtonGoToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ButtonResume()
    {
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        craftingObj.SetActive(false);
        inventoryObj.SetActive(false);
        pauseObj.SetActive(true);
        ButtonPause();
    }
    public void Winner()
    {
        ButtonPause();
    }
}
