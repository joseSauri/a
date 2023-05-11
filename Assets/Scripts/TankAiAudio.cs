using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAiAudio : MonoBehaviour
{
    public AudioSource tankMove;
    public AudioSource tankEngine;
    public NavMeshAgent agente;


    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!agente.isStopped)
        {
            tankMove.Play();
            tankEngine.Stop();
        }
        else
        {
            tankEngine.Play();
            tankMove.Stop();
        }
    }
}
