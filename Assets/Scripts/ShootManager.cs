using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    
    public GameObject misil;
    public Transform misilSpawn;
    
    //cooldown disparo
    public float cooldownTime = 5f; // Tiempo de enfriamiento en segundos
    private float lastActionTime = 0f;
    
    //Sonido
    public AudioSource disparoFoley;


    private void Awake()
    {
        disparoFoley = misilSpawn.GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (Time.time > lastActionTime)
        {
            lastActionTime = Time.time + cooldownTime;

            Instantiate(misil,misilSpawn.position,misilSpawn.rotation);
            disparoFoley.Play();
        }
        
    }
}
