using System.Collections;
using System.Collections.Generic;
using static Utilitaire;
using UnityEngine.UI;
using UnityEngine;

public class PointsSante : MonoBehaviour
{
    private GameObject joueur;
    private ControleurRpg controleur;
    private Text composanteTexte;

    void Start()
    {
        controleur = GameObject.FindGameObjectWithTag("Player").GetComponent<ControleurRpg>();
        if (controleur == null) MessageErreur(this, "Aucun controleurRpg n'a pu être trouvé et cette composante ne pourra fonctionner");
        composanteTexte = GetComponent<Text>();
    }
    
    void LateUpdate()
    {
        composanteTexte.text = $"{controleur.Santé}/{controleur.SantéMaximale}";
    }
}
