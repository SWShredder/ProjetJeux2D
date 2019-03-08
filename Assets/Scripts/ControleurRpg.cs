/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/05
 *      
 *      Incomplet pour le moment. Permet de gérer les points de santé et la mort.
 */
using UnityEngine;

public class ControleurRpg : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int santéMaximale = 50;

    private ControleurEntité controleurEntité;

    public int SantéMaximale { get => santéMaximale; }
    public int Santé { private set; get; }



    // Start is called before the first frame update
    void Start()
    {
        controleurEntité = GetComponent<ControleurEntité>();
        if(controleurEntité == null)
        {
            Utilitaire.MessageErreur(this, "ControleurEntité n'a pas pu être initialisé");
        }
        Santé = santéMaximale;
    }

    public void AppliquerDégats(int dégats)
    {
        if (dégats < 0)  Utilitaire.MessageErreur(this, "impossible d'appliquer des dégats négatifs. " +
            "Utilisez la méthode appropriée pour la guérison");
        GetComponent<EffetsVisuels>().Clignoter(gameObject, 0.04f);
        Santé -= dégats < Santé ? dégats : Santé;
        if (Santé <= 0) SurMort();
    }

    private void SurMort() // OnDeath
    {
        controleurEntité.Mourir();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
