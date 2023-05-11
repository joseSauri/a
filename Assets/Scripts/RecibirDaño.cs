using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RecibirDaño : MonoBehaviour
{
    public float vidaAi;
    public AIController ai;
    public ShootManager shoot;
    public Rigidbody rb;
    public NavMeshAgent navMesh;
    public ParticleSystem humo;
    public GameObject enemyMark;
    public UnityEvent win;
    public void recibirDaño()
    {
        vidaAi--;
        if (vidaAi<= 0)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                navMesh.isStopped = true;
                Destroy(navMesh);
                rb.AddForce(0,60,0,ForceMode.VelocityChange);
                rb.AddForce(0,-9,0,ForceMode.VelocityChange);
                Destroy(enemyMark);
                Destroy(ai);
                Destroy(shoot);
                StartCoroutine(humoAnim());
                StartCoroutine(destroyDretoyed());
            }
            else
            {
                win.Invoke();
                navMesh.isStopped = true;
                Destroy(navMesh);
                rb.AddForce(0,60,0,ForceMode.VelocityChange);
                rb.AddForce(0,-9,0,ForceMode.VelocityChange);
                Destroy(enemyMark);
                Destroy(ai);
                Destroy(shoot);
                StartCoroutine(humoAnim());
            }
            
        }
        
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Misil"))
        {
            recibirDaño();
            
        }
    }

    IEnumerator humoAnim()
    {
        humo.Play();
        yield return new WaitForSeconds(20);
        humo.Stop();

    }

    IEnumerator destroyDretoyed()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
        
    }
}
