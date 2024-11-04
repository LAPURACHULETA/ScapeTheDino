using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzlesManager : MonoBehaviour
{
    private void FixedUpdate()
    {
        CheckCompleteCombos();
    }

    void CheckCompleteCombos()
    {
        if (CodigoPuzle.Instance != null && 
            CheckComboPuzzle.Instance != null && 
            LaserPuzle.Instance != null && GameManager.Instance != null)
        {
            if (CodigoPuzle.Instance.SetCompletePuzzles() == true &&
                CheckComboPuzzle.Instance.SetCompletePuzzles() == true &&
                LaserPuzle.Instance.SetCompletepuzzle() == true)
            {
                GameManager.Instance.changeState(GameManager.State.Win);
            }
            else
            {
                
            }
        }
        else
        {
            
        }
    }
}
