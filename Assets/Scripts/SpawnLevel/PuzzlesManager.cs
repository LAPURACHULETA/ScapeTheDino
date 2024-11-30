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
        if (/*CodigoPuzle.Instance != null &&
            CheckComboPuzzle.Instance != null&&*/
            LaserPuzle.Instance != null && GameManager.Instance != null)
        {
            if (/*CodigoPuzle.Instance.puzzleComplete == true &&
                CheckComboPuzzle.Instance.puzzleComplete == true &&*/
                LaserPuzle.Instance.puzzleComplete == true)
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
