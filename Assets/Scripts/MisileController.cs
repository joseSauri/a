using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MisileController : MonoBehaviour
{
    //velocidad del misil
    public float vel;
    public Transform misil;
    public ParticleSystem explosion;
    public AudioSource explosionAudio;
    private void Start()
    {
        explosionAudio = GetComponent<AudioSource>();
        misil = transform.Find("Misil");
        explosion = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (misil)
        {
            transform.Translate(-transform.forward*vel*Time.deltaTime);
        }

        StartCoroutine(DestroyOnTime());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            destruir();
        }
        else
        {         
            destruir();
        }
    }

    IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(5);
        destruir();

    }

    IEnumerator particulas()
    {
        explosion.Play();
        yield return new WaitForSeconds(1);
        explosion.Stop();
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
        
    }

    public void destruir()
    {
        explosionAudio.Play();
        Destroy(misil.gameObject);
        StartCoroutine(particulas());
        transform.Translate(0,0,0);
    }

    
}
