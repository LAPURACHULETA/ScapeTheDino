using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

public class ChangeCameraPerson : MonoBehaviour
{
    public CameraPlayer input;

    public GameObject thirdPerson;
    public GameObject firtsPerson;
 

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<CameraPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.changeValue == 1f && !firtsPerson.activeInHierarchy)
        {
            thirdPerson.SetActive(false);
            firtsPerson.SetActive(true);

            StartCoroutine(ShowReticle());
        }
        else if (input.changeValue == 1f && !thirdPerson.activeInHierarchy)
        {
            thirdPerson.SetActive(true);
            firtsPerson.SetActive(false);
        }

    }

    IEnumerator ShowReticle()
    {
        yield return new WaitForSeconds(0.25f);
    }
}
