using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactive : MonoBehaviour
{
    private LaserPuzle laser;
    public string nameOfTrampa;
    private void Start()
    {
        laser=gameObject.GetComponent<LaserPuzle>();
    }
   
    public void RatateLaser()
    {
       laser.isRotating = true;
    }
    
}
