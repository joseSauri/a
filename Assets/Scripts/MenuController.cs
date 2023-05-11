using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject HUD;
    public GameObject menumuerte;
    public TankController tankScript;
    
    // Start is called before the first frame update
    void Start()
    {
        HUD.SetActive(true);
        menumuerte.SetActive(false);
    }


    public void menusMuerte()
    {
        HUD.SetActive(false);
        menumuerte.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
