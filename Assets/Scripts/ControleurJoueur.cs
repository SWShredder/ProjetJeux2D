/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/02
 *      Dernière modification: 2019/03/05
 *    
 *      La majorité du comportement de ControleurJoueur se trouve dans ControleurEntité tout comme
 *      ControleurEnnemi. Le script existe pour permettre de facilement séparer l'implémentation
 *      de ControleurEntité au besoin si nécessaire.
 *      
 */
using UnityEngine;
using MyBox;
using System.Collections;
using static Utilitaire;

public class ControleurJoueur : ControleurEntité
{
    [SerializeField] private GameObject attaque1;
    [SerializeField] private GameObject attaque2;
    [SerializeField] private Transform positionAttaque1;
    [SerializeField] private Transform positionAttaque2;

    public EntitéState ÉtatInactif { private set; get; }
    public EntitéState ÉtatMouvement { private set; get; }
    public EntitéState ÉtatAttaque1 { private set; get; }
    public EntitéState ÉtatAttaque2 { private set; get; }
    public EntitéState ÉtatMort { private set; get; }
    public GameObject Attaque1 { private set; get; }
    public GameObject Attaque2 { private set; get; }

    public ControleurJoueur()
    {
        ÉtatInactif = new JoueurInactifState(this);
        ÉtatMouvement = new JoueurMouvementState(this);
        ÉtatAttaque1 = new JoueurAttaque1State(this);
        ÉtatAttaque2 = new JoueurAttaque2State(this);
        ÉtatMort = new JoueurMortState(this);
    }


    public override void Attaquer()
    {
        var instance = Instantiate(
            attaque1,
            positionAttaque1.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
        StartCoroutine(VérrouillerAttaque(instance.GetComponent<ProjectileComportement>().Cooldown));
        instance.GetComponent<ProjectileComportement>().Parent = this.gameObject;
    }

    public void ExécuterAttaque1()
    {
        var instance = Instantiate(
            attaque1,
            positionAttaque1.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
        StartCoroutine(VérrouillerAttaque(instance.GetComponent<ProjectileComportement>().Cooldown));
        instance.GetComponent<ProjectileComportement>().Parent = this.gameObject;
    }

    public void ExécuterAttaque2()
    {
        var instance = Instantiate(
            attaque2,
            positionAttaque2.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
        StartCoroutine(VérrouillerAttaque(instance.GetComponent<ProjectileComportement>().Cooldown));
        instance.GetComponent<ProjectileComportement>().Parent = this.gameObject;
    }

    public override void Mourir()
    {
        base.Mourir();
        État = ÉtatMort;
    }

    void Start()
    {
        Initialiser();
        État = ÉtatInactif;
        if (positionAttaque1 == null)
        {
            MessageErreur(this, "La composante Position Projectile n'a pas été définie et la méthode attaque ne pourra pas" +
                " fonctionner.");
        }
    }

}
