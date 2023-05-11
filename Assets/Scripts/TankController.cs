using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TankController : MonoBehaviour
{

    public float vidaJugador;
    [Header("Rotation")]
    public Vector2 curRotateInput;
    public float rotSpeed;
    [Header("Movement")]
    public Vector2 curMoveInput;
    public float moveSpeed;
    [Header("Camera & cannon")]
    public GameObject cameraContainer;
    public float camCurRot;
    private Vector2 mouseDelta;
    public float cameraSens;

    public ShootManager ShootManager;
    
    
    //Componentes
    public Rigidbody rb;
    public AudioSource tankMove;
    public AudioSource tankEngine;
    public AudioSource alarm;
    public ParticleSystem humo;
    public Image DMGIcon;


    public UnityEvent muerte;
    

    private void Awake()
    {
        DMGIcon.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ShootManager = GetComponent<ShootManager>();
        rb = GetComponent<Rigidbody>();
    }
    
    
    
    
//---------INPUTS------------------------------------------------------------------------------------------------------------
    public void MoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            tankEngine.Stop();
            tankMove.Play();
            curMoveInput = context.ReadValue<Vector2>();
        }else if (context.phase == InputActionPhase.Canceled)
        {
            tankEngine.Play();
            tankMove.Stop();
            curMoveInput = Vector2.zero;
        }
    }

    public void RotateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!tankMove.isPlaying)
            {
                tankEngine.Stop();
                tankMove.Play();  
            }
            curRotateInput = context.ReadValue<Vector2>();
        }else if (context.phase == InputActionPhase.Canceled)
        {
            if (tankMove.isPlaying && !Input.GetKey(KeyCode.W))
            {
                if (!Input.GetKey(KeyCode.S))
                {
                    tankEngine.Play();
                    tankMove.Stop();
                }
            }
            curRotateInput = Vector2.zero;

        }
    }

    public void RotateCannonInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void shootInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShootManager.Shoot();
        }
    }
    //---------INPUTS------------------------------------------------------------------------------------------------------------


    private void FixedUpdate()
    {
        Move();
        Rotate();
        RotateCannon();

    }

    private void Update()
    {
        if (vidaJugador == 3 && !humo.isPlaying)
        {
            humo.Play();
        }

        if (vidaJugador == 1 && !alarm.isPlaying)
        {
            alarm.Play();
            DMGIcon.enabled = true;
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MisilE"))
        {
            if (vidaJugador == 1)
            {
                muerte.Invoke();
            } 
            else
            {
                vidaJugador--;
            }
        }
    }

   

    //--------ACTIONS-------------------------------------------------------------------------------------------------------------
    private void RotateCannon()
    {
        camCurRot += mouseDelta.x * cameraSens;
        // Mover camara
        cameraContainer.transform.localEulerAngles = new Vector3(0, camCurRot, 0);

    }

    private void Rotate()
    {
        transform.localEulerAngles += new Vector3(0, curRotateInput.x * rotSpeed, 0);
    }
    private void Move()
    {
     
        Vector3 dir = transform.forward * curMoveInput.y + transform.right * curMoveInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }
  
    //--------ACTIONS-------------------------------------------------------------------------------------------------------------

}
