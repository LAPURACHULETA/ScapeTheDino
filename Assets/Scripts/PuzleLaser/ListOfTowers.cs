using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfTowers : MonoBehaviour
{
    public static ListOfTowers Instance;

    public List<GameObject> listTorres = new List<GameObject>();
    public List<GameObject> listTorresConnect = new List<GameObject>();
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
    /// <summary>
    /// Verifica si todas las torres están conectadas.
    /// </summary>
    public bool CheckAllTowersConnected()
    {

        List<GameObject> allTorres = SetListTorres();

        foreach (GameObject torre in allTorres)
        {
            if (!listTorresConnect.Contains(torre))
            {
                ////Debug.Log($"Torre no conectada: {torre.name}");
                return false; // Una torre no está conectada
            }
        }
        return true;
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
