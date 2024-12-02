using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfTowers : MonoBehaviour
{
    public static ListOfTowers Instance;

    public List<GameObject> listTorres = new List<GameObject>();
    public GameObject doorLaser; // Puerta a destruir
    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {

        }
    }
    public List<GameObject> GetListTorres(List<GameObject> name)
    {
        listTorres = name;
        return listTorres;
    }

    public List<GameObject> SetListTorres()
    {
        //Debug.Log(listTorres);
        return listTorres;
    }
}
