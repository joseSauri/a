using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class triggerAudio1 : MonoBehaviour
{
    public UnityEvent audio1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrgrMsn"))
        {
            audio1.Invoke();
            Destroy(gameObject);
        }
    }
}
