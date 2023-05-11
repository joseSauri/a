using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public enum TipoIA
{
    Tanque,Persona,Boss
}
public enum EstadoIA
{
    Idle,
    Atacando,
    PersAtacando,
    Huyendo,
    Patrullando,
    Defendiendo,
}
public class AIController : MonoBehaviour
{
    [Header("IA")]
    public TipoIA tipoIA;
    public EstadoIA estadoIA;
    public float distanciaDeteccion;
    public float distanciaSegura;

    [Header("Stats")] 
    public float velocidadAndar;
    public float velocidadCorrer;

    [Header("Pasear")] 
    public float minimaDistanciaPaseo;
    public float maximaDistanciaPaseo;
    public float minTiempoEspera;
    public float maxTiempoEspera;

    [Header("Combate")] 
    public int damage;
    public float rationaAtaque;
    private float ultimoAtaque;
    public float distanciaAtaque;

    [Header("Patrullando")] 
    public float distanciaWaypoint;
    public Transform[] waypoints;
    public int indiceWaypoints = 0;
   [SerializeField] private Vector3 target;
    

    public float distanciaAlJugador;
    
    private NavMeshAgent agente;
    //public Animator animator;
    public GameObject jugador;

    public ShootManager ShootManager;
    
    //Cañon tanque
    public FieldOfView view;
    public GameObject cannon;
    public float cannonRot;

    //COMPONENTES

    public Rigidbody rb;

    private void Awake()
    {

        view = GetComponent<FieldOfView>();
        jugador  = GameObject.FindWithTag("Player");
        ShootManager = GetComponent<ShootManager>();
        agente = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        //Calculamos la distancia al jugador a todas las IAs------------------------------------------------------------
        distanciaAlJugador = Vector3.Distance(transform.position, jugador.transform.position);
        
        //CONTROL DE LAS VARIABLES DE MOVIMIENTO EN EL ANIMATOR---------------------------------------------------------
       /* if(estadoIA == EstadoIA.Patrullando || estadoIA == EstadoIA.Vagando)
        {
            animator.SetBool("correr",false);
            animator.SetBool("move",true);


        }else if (estadoIA == EstadoIA.Atacando || estadoIA == EstadoIA.Huyendo)
        {
            animator.SetBool("correr",true);
            animator.SetBool("move",false);
        }
        else
        {
            animator.SetBool("move",false);
            animator.SetBool("correr",false);
        }*/
        
        //CONTROL DE LOS METODOS UPDATE DEPENDIENDO DEL ESTADO DE LA IA-------------------------------------------------
        switch (estadoIA)
        {
            case EstadoIA.Huyendo:
                //animator.SetBool("correr",true);
                HuyendoUpdate();
                break;
            case EstadoIA.Atacando:
                //animator.SetBool("correr",true);
                AtacandoUpdate();
                break;
            case EstadoIA.Patrullando:
                //animator.SetBool("correr",true);
                PatrulandoUpdate();
                break;
            case EstadoIA.Defendiendo:
                //animator.SetBool("correr",false);
                //animator.SetBool("move",false);
                DefendiendoUpdate();
                break;
            default: PasivoUpdate();
                break;
        }
        
    }
//CONTROL DEL ESTADO DE LAS IAs-----------------------------------------------------------------------------------------
    private void PasivoUpdate()
    {
        if (tipoIA == TipoIA.Persona)
        {
            SetEstado(EstadoIA.Defendiendo);
        }
        
        if (tipoIA == TipoIA.Tanque && distanciaAlJugador < distanciaDeteccion)
        {
            SetEstado(EstadoIA.Atacando);
        }
        else if (tipoIA == TipoIA.Persona && distanciaAlJugador < distanciaDeteccion)
        {
            SetEstado(EstadoIA.PersAtacando);
            agente.SetDestination(GetSitioHuir());
        }
        if(tipoIA == TipoIA.Boss && distanciaAlJugador < distanciaDeteccion)
        {
            SetEstado(EstadoIA.Atacando);
        }
        
       
    }

//CALCULAR DESTINO CUANDO ESTA EN ESTADO HUYENDO------------------------------------------------------------------------
    Vector3 GetSitioHuir()
    {
        
        //Calcular el sitio al que huir
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * distanciaSegura), out hit, distanciaSegura,
            NavMesh.AllAreas);
        int i = 0;
        
            while (GetAnguloDestino(hit.position) > 90 || distanciaAlJugador < distanciaSegura)
            {
                NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * distanciaSegura), out hit, distanciaSegura,
                    NavMesh.AllAreas);
                i++;
                if (i == 30)
                {
                    break;
                }
            }
            //Devuelve la posicion que sera el nuevo destino
            return hit.position;

    }

    private float GetAnguloDestino(Vector3 targetPosition)
    {
        return Vector3.Angle(transform.position - jugador.transform.position, transform.position + targetPosition);
    }
    
    //METODO QUE LLAMAMOS PARA CAMBIAR EL ESTADO---------------------------------------------------------------------------
    private void SetEstado(EstadoIA newEstado)
    {
        estadoIA = newEstado;
        switch (estadoIA)
        {
            case EstadoIA.Idle:
                agente.speed = velocidadAndar;
                agente.isStopped = true;
                break;
            case EstadoIA.Patrullando:
                agente.speed = velocidadAndar;
                agente.isStopped = false;
                break;
            default:
                agente.speed = velocidadCorrer;
                agente.isStopped = false;
                break;
        }
    } 

//CAMBIAR ENTRE EL ARRAY DE WAYPOINT PARA QUE LA IA RECORRA UNA RUTA DE IDA Y VUELTA-----------------------------------
    private void CambioWaypoint()
    {
        //Si aun no ha llegado al ultimo waypoint suma 1 a la variable indiceWaypoints que utilizamos para indicar
        //que waypoint dentro del array usara la IA de destino
        if (indiceWaypoints <= 3)
        {
            indiceWaypoints++;
        } 
        //Cuando llega al final vuelve al primer waypoint y repite la ruta
        else 
        {
            indiceWaypoints = 0;
        }
    }
    
//ACTUALIZAR EL DESTINO DEPENDIENDO EL WAYPOINT------------------------------------------------------------------------- 
    private void ActualizarDestino()
    {
        target = waypoints[indiceWaypoints].position;
        agente.SetDestination(target);
    }

//CONTROLAR EL ESTADO PATRULLANDO---------------------------------------------------------------------------------------
    private void PatrulandoUpdate()
    {
        //Si el jugador esta cerca o a la vista, la IA cambia a atacando.
        if (distanciaAlJugador < distanciaDeteccion || view.canSeePlayer)
        {
            SetEstado(estadoIA = EstadoIA.Atacando);
        }
        //Si esta lejos, le damos como destino los waypoints de su ruta
        else
        {
            agente.SetDestination(target);
        }

        distanciaWaypoint = Vector3.Distance(transform.position, waypoints[indiceWaypoints].transform.position);
        //Cuando llega a un waypoint nuevo llamamos a los metodos que calculan el nuevo waypoint y cambian el destino
        if (Vector3.Distance(transform.position,waypoints[indiceWaypoints].transform.position) < 2f)
        {
            CambioWaypoint();
            ActualizarDestino();
        }
        
    }

//CONTROLAR ESTADO ATACANDO---------------------------------------------------------------------------------------------    
    private void AtacandoUpdate()
    {
//SI ESTA LEJOS COMO PARA DISPARAR O NO TIENE AL JUGADOR A LA VISTA SE MUEVE HACIA EL. 
        if (distanciaAlJugador > distanciaAtaque || view.canSeePlayer == false)
        {
            
            agente.isStopped = false;
            agente.SetDestination(jugador.transform.position);
            Vector3 target = new Vector3(jugador.transform.position.x,0,jugador.transform.position.z);
            cannon.transform.LookAt(jugador.transform.position);
        }  
        //SI ESTA A TIRO DISPARA Y A LA VISTA
        else
        {
            agente.isStopped = true;
            Vector3 target = new Vector3(jugador.transform.position.x,0,jugador.transform.position.z);
            cannon.transform.LookAt(jugador.transform.position);
            if (Time.time - ultimoAtaque > rationaAtaque)
            {
                ShootManager.Shoot();
            }
        }
    }
    
//CONTROLAR ESTADO HUYENDO----------------------------------------------------------------------------------------------    
    private void HuyendoUpdate()
    {
        //Si es persona utiliza el impacto de la bala como punto del que huir
        if (tipoIA == TipoIA.Persona)
        {
            if (distanciaAlJugador < distanciaSegura && agente.remainingDistance < 0.1f)
            {
                agente.SetDestination(GetSitioHuir());
            }else if (distanciaAlJugador > distanciaSegura)
            {
                SetEstado(EstadoIA.Idle);
            } 
        }

    }
    
//CONTROLAR ESTADO DEFENDIENDO------------------------------------------------------------------------------------------    
    private void DefendiendoUpdate()
    {
  
        if (distanciaAlJugador > distanciaDeteccion)
        {
            SetEstado(EstadoIA.Idle);
        }
        else
        {
            SetEstado(EstadoIA.Atacando);
            
        }
    }

   

   
}
