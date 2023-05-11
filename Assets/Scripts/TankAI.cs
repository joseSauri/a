using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour
{
   public Transform objetivo;

   public NavMeshAgent agente;
   private void Awake()
   {
      agente = GetComponent<NavMeshAgent>();
   }

   private void FixedUpdate()
   {
      agente.destination = objetivo.position;

   }
}
