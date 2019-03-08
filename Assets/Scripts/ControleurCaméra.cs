/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/04
 *      
 *      Ce script permet d'appliquer un effet de smoothing sur le déplacement de la caméra et
 *      de calibrer cet effet. La caméra ne doit pas être l'enfant du personnage. 
 *      
 */
using UnityEngine;

public class ControleurCaméra : MonoBehaviour
{

    [SerializeField, Range(0.5f, 5f), Tooltip("La vitesse de déplacement de la caméra")]
    private float vitesseMouvement = 2.5f;

    [SerializeField, Tooltip("Mettre à true si vous ne souhaitez pas avoir d'effet sur le déplacement de la caméra")]
    private bool désactiverInterpolation = false;

    private GameObject joueur;
    private Vector3 valeurOffset;

    void Start()
    {
        joueur = GameObject.FindGameObjectWithTag("Player");
        if(joueur == null)
        {
            Utilitaire.MessageErreur(this, "Aucun tag Player n'a pu être trouvé. Un tag player est nécessaire sur au moins un GameObject");
        }
    }

    void LateUpdate()
    {
        // Détermine la position du joueur et altère la valeur des z (la caméra doit être à l'arrière des objets).
        var nouvellePosition = joueur.transform.position;
        nouvellePosition.z -= 15;

        // Si l'interpolation est désactivée, la caméra bouge instantanément.
        this.transform.position = !désactiverInterpolation ?
            Vector3.Lerp(this.transform.position, nouvellePosition, vitesseMouvement * Time.deltaTime) : nouvellePosition;
    }
}
