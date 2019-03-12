using System.Collections;
using System;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class GestionnaireJeuInput : MonoBehaviour
{
    private GameObject menuPause;
    private bool EstEnPause;

    void Awake()
    {
        menuPause = GameObject.FindGameObjectWithTag("MenuPause");
        menuPause.SetActive(false);
    }
    void LateUpdate()
    {
        if (GestionnaireJeu.Instance.EstPartieDéfaite) return;
        if (Input.GetButtonDown("Start"))
        {
            if (Time.timeScale == 1)
            {
                menuPause.SetActive(true);
                Time.timeScale = 0;
                EstEnPause = true;
            }
            else if (Time.timeScale == 0)
            {
                menuPause.SetActive(false);
                Time.timeScale = 1;
                EstEnPause = false;
            }
        }
        if (Input.GetButtonDown("Option") && EstEnPause)
        {
            GestionnaireJeu.Instance.RechargerPartie();
            Time.timeScale = 1;
            EstEnPause = false;
        }
    }
}
