
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionnaireJeu : MonoBehaviour
{
    public static GestionnaireJeu Instance;
    private GameObject menuDéfaite;
    public bool EstPartieDéfaite { private set; get; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        menuDéfaite = GameObject.FindGameObjectWithTag("MenuDéfaite");
        menuDéfaite.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (EstPartieDéfaite && Input.anyKeyDown) RechargerPartie();
    }

    public void AppelerMenuDéfaite()
    {
        EstPartieDéfaite = true;
        menuDéfaite.SetActive(true);
    }

    public void RechargerPartie()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
