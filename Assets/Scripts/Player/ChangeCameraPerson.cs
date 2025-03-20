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

    private Quaternion originalRotation;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<CameraPlayer>();
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Change();
    }

    private void Change()
    {
        if (GameManager.Instance.state == GameManager.State.InPuzzle && !firtsPerson.activeInHierarchy)
        {
            thirdPerson.SetActive(false);
            firtsPerson.SetActive(true);

            StartCoroutine(ShowReticle());
        }
        else if (GameManager.Instance.state == GameManager.State.InGame && !thirdPerson.activeInHierarchy)
        {
            thirdPerson.SetActive(true);
            firtsPerson.SetActive(false);
            transform.rotation = originalRotation;
        }

    }

    IEnumerator ShowReticle()
    {
        yield return new WaitForSeconds(0.25f);
    }
}
