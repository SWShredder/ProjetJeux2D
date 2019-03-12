using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilitaire;

public class BarSante : MonoBehaviour
{
    private GameObject joueur;
    private ControleurRpg controleur;
    private RectTransform rectTransform;
    void Start()
    {
        controleur = GameObject.FindGameObjectWithTag("Player").GetComponent<ControleurRpg>();
        if (controleur == null) MessageErreur(this, "Aucun controleurRpg n'a pu être trouvé et cette composante ne pourra fonctionner");
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        rectTransform.localScale = new Vector3(controleur.Santé * 1.0f / controleur.SantéMaximale, 1, 1);
    }
}
