using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactive : MonoBehaviour
{
    private LaserPuzle laser;

    private void Start()
    {
        laser=gameObject.GetComponent<LaserPuzle>();
    }
   
    public void RatateLaser()
    {
       laser.isRotating = true;
    }
    
}
