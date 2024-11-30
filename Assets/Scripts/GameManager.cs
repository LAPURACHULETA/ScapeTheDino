using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject winner;
    CameraPlayer camera;
    
    public enum State
    {
        InGame,
        Resume,
        Pause,
        InPuzzle,
        GameOver,
        Win
    }

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

    public State state;
    private void Start()
    {
        camera = FindObjectOfType<CameraPlayer>();
    }
    
    public void changeState(State newState)
    {
        if (newState == state)
        {
            return;
        }
        if(state == State.Pause&&newState != State.InGame)
        {
            return;
        }
        state = newState;

        switch (state)
        {
            case State.InGame:
                ButtonResume();
                camera.LookMouse();
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
            case State.InPuzzle:

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
        pauseObj.SetActive(true);
    }
    public void ButtonGoToMenu()
    {
        SceneManager.LoadScene(0);
    } 
    public void Reiniciar()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void ButtonResume()
    {
        Time.timeScale = 1;
        pauseObj.SetActive(false);
        changeState(GameManager.State.InGame);
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
        
    }
    public void Winner()
    {
        winner.SetActive(true);
    }
    
}
