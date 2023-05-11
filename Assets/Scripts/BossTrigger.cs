using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class BossTrigger : MonoBehaviour
{
    public RecibirDaño iaScript;
    public UnityEvent bossTrigtger;

    private void Start()
    {
        iaScript = GetComponent<RecibirDaño>();
    }

    void Update()
    {
        if (iaScript.vidaAi == 0)
        {
            bossTrigtger.Invoke();
            Destroy(this);
        }
    }
}
