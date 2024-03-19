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
        Change();
    }

    private void Change()
    {
        if (input.changeValueCamera != 1f /*tomar el component chinemachine y ve si termino de trans*/)
        {
            return;
        }

        //poner varibale flipflop para el cambio correcto de las camara con un bool
        if (thirdPerson.activeInHierarchy && !firtsPerson.activeInHierarchy)
        {
            thirdPerson.SetActive(false);
            firtsPerson.SetActive(true);

            StartCoroutine(ShowReticle());
        }
        else if (!thirdPerson.activeInHierarchy && firtsPerson.activeInHierarchy)
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
