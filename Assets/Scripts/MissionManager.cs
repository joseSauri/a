using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{

    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audio3;
    public TMP_Text sub;
    
    public void AudiosMision2()
    {
        StartCoroutine(subs2());
        audio2.Play();
    }

    public void AudiosMision1()
    {
        StartCoroutine(subs1());
        audio1.Play();
    }

    public void audioFinal()
    {
        StartCoroutine(subs4());
        audio3.Play();
    }

    public void subs3Call()
    {
        StartCoroutine(subs3());
    }


    IEnumerator subs1()
    {
        sub.text = "Que bueno que llegaron los refuerzos";
        yield return new WaitForSeconds(2);
        sub.text = "Tenemos problemas con las comunicaciones";
        yield return new WaitForSeconds(3);
        sub.text = "Tienes que encontrar el inhibidor y destruirlo";
        yield return new WaitForSeconds(3);
        sub.text = " ";
    }

    IEnumerator subs2()
    {
        sub.text = "Bien hecho camarada";
        yield return new WaitForSeconds(2);
        sub.text = "Pero cuidado, hay refuerzos en camino";
        yield return new WaitForSeconds(3);
        sub.text = "Acaba con ellos";
        yield return new WaitForSeconds(2);
        sub.text = " ";
    }
    IEnumerator subs3()
    {
        sub.text = "Haha, parece que les has hecho enfadar camarada";
        yield return new WaitForSeconds(3);
        sub.text = "Ahora solo queda acabar con el general";
        yield return new WaitForSeconds(3);
        sub.text = "Buscalo y destruyelo";
        yield return new WaitForSeconds(2);
        sub.text = " ";
    }
    
    IEnumerator subs4()
    {
        sub.text = "Bien hecho camarada";
        yield return new WaitForSeconds(1.5f);
        sub.text = "El pueblo de Cheningrado te estará eternamente agradecido";
        yield return new WaitForSeconds(4.5f);
        sub.text = "";
    }
    
    

    

}
