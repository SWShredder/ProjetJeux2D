/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/02
 *      Dernière modification: 2019/03/09
 *    
 *      La majorité du comportement de ControleurJoueur se trouve dans ControleurEntité tout comme
 *      ControleurEnnemi. Le script existe pour permettre de facilement séparer l'implémentation
 *      de ControleurEntité au besoin si nécessaire.
 *      
 */
using UnityEngine;
using MyBox;
using static Utilitaire;
using System.Collections.Generic;

public class ControleurJoueur : ControleurEntité
{
    [SerializeField] private GameObject attaque1;
    [SerializeField, HideInInspector] private GameObject attaque2;
    [SerializeField] private Transform positionAttaque1;
    [SerializeField, HideInInspector] private Transform positionAttaque2;

    public List<Projectile.Effet> DécorationsAttaque1;
    [HideInInspector]
    public List<Projectile.Effet> DécorationsAttaque2;
    ///<summary>Référence vers la state JoueurInactifState</summary>
    public EntitéState ÉtatInactif { private set; get; }
    ///<summary>Référence vers la state JoueurMouvementState</summary>
    public EntitéState ÉtatMouvement { private set; get; }
    ///<summary>Référence vers la state JoueurAttaque1State</summary>
    public EntitéState ÉtatAttaque1 { private set; get; }
    ///<summary>Référence vers la state JoueurAttaque2State</summary>
    public EntitéState ÉtatAttaque2 { private set; get; }
    ///<summary>Référence vers la state JoueurMortState</summary>
    public EntitéState ÉtatMort { private set; get; } 
    ///<summary>Le GameObject qui représente le projectile de l'attaque 1</summary>
    public GameObject Attaque1 { private set; get; }
    ///<summary>Le GameObject qui représente le projectile de l'attaque 2</summary>
    public GameObject Attaque2 { private set; get; }
    public bool EstSurPickup { set; get; }

    public ControleurJoueur()
    {
        ÉtatInactif = new JoueurInactifState(this);
        ÉtatMouvement = new JoueurMouvementState(this);
        ÉtatAttaque1 = new JoueurAttaque1State(this);
        ÉtatAttaque2 = new JoueurAttaque2State(this);
        ÉtatMort = new JoueurMortState(this);
    }

    ///<summary>
    ///[Ne pas utiliser]Permet de commander l'attaque 1 du Joueur. Utiliser plutot ExécuterAttaque1()
    ///</summary>
    public override void Attaquer()
    {
        var instance = Instantiate(
            attaque1,
            positionAttaque1.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
        StartCoroutine(VérrouillerAttaque(instance.GetComponent<Projectile>().Cooldown));
        instance.GetComponent<Projectile>().Parent = this.gameObject;
    }
    /// <summary>
    /// Permet de commander l'attaque 1 du Joueur. L'attaque est executée immédiatement et la méthode
    /// verrouillerAttaque est appelée pour empecher le controleur d'attaquer si l'attaque est en cooldown;
    /// </summary>
    public void ExécuterAttaque1()
    {
        
        var instance = Instantiate(
            attaque1,
            positionAttaque1.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
        instance.GetComponent<Projectile>().DécorerComportement(DécorationsAttaque1);
        StartCoroutine(VérrouillerAttaque(instance.GetComponent<Projectile>().Cooldown));
        instance.GetComponent<Projectile>().Parent = this.gameObject;
    }
    /// <summary>
    /// Permet de commander l'attaque 2 du Joueur. L'attaque est executée immédiatement et la méthode
    /// verrouillerAttaque est appelée pour empecher le controleur d'attaquer si l'attaque est en cooldown;
    /// </summary>
    public void ExécuterAttaque2()
    {
        GameObject instance = ProjectilesPool.Instance.ObtenirProjectile(positionAttaque2.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
        StartCoroutine(VérrouillerAttaque(instance.GetComponent<Projectile>().Cooldown));
        instance.GetComponent<Projectile>().Parent = this.gameObject;
    }
    /// <summary>
    /// Permet de commander la mort du Joueur.
    /// </summary>
    public override void Mourir()
    {
        base.Mourir();
        État = ÉtatMort;
        GestionnaireJeu.Instance.AppelerMenuDéfaite();
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
