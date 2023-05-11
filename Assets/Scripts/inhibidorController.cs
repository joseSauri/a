using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class inhibidorController : MonoBehaviour
{
    public UnityEvent mision2;
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Misil"))
        {
            mision2.Invoke();
            Destroy(gameObject);
        }
    }
}
