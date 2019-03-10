﻿/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/04
 *      Dernière modification: 2019/03/09
 *      
 *      Comportement des projectiles. Pour le moment le comportement est limité, mais il permet de s'assurer que
 *      les projectiles ne "collideront" pas ensemble, sur une Entité avec 0 points de santé, ou encore sur le 
 *      parent. (Le parent doit être ajouté après l'instantiation)
 * 
 */
using System;
using System.Collections;
using MyBox;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public enum Effet
    {
        OmniShotSurFinVie,
        Aucun,
    }

    [SerializeField]
    protected bool estProjectile = true;
    [SerializeField, ConditionalField("estProjectile", true)]
    protected float vitesse = 2f;
    [SerializeField, Min(0.01f), ConditionalField("estProjectile", false)]
    protected float duréeVie = 0.5f;
    [SerializeField]
    protected int dégats = 1;
    [SerializeField, Min(0.1f), ConditionalField("estProjectile", true)]
    protected float portée = 6f;
    [SerializeField, Min(0.01f)]
    protected float cooldown = 0.35f;
    [SerializeField, Min(0.01f)]
    protected float délaiDégat = 0.01f;
    [SerializeField]
    protected bool collisionAvecAutresProjectiles = false;
    [SerializeField]
    protected Effet effet;
    [SerializeField]
    private bool estTransperçant = false;


    public ProjectileComportement Comportement { set; get; }
    public GameObject Parent { set; get; }
    public float Portée { get => portée; protected set => portée = value; }
    public float Cooldown { get => cooldown; protected set => cooldown = value; }
    public Rigidbody2D CorpsPhysique { set; get; }
    public Vector2 Direction { set; get; }
    public float Vitesse { get => vitesse; }
    public bool EstTransperçant { get => estTransperçant; set => estTransperçant = value; }

    void Awake()
    {
        Comportement = new StandardProjectileComportement(this);
        Comportement = DécorerComportement();
    }

    void Start() => Comportement.Initialiser();
    void FixedUpdate() => Comportement.Actualiser();
    public void SurFinVie()
    {
        Comportement.Terminer();
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (EstSourceProjectile(collider.gameObject) && !collisionAvecAutresProjectiles) return;
        else if (EstSourceEntitéValide(collider.gameObject)) Comportement.SurImpactAvecEntité(collider);
        else if (!EstTransperçant) Comportement.SurImpact(collider);
    }

    public ProjectileComportement DécorerComportement()
    {
        switch (effet)
        {
            case Effet.OmniShotSurFinVie:
                return new OmniShotSurFinVieProjectileDécorateur(this);
            default:
                return Comportement;
        }
    }

    public float ObtenirDuréeVie() => vitesse > 0 ? portée / vitesse : duréeVie;
    public void DéclencherDégats(GameObject source) => source.GetComponent<ControleurRpg>().StartCoroutine(
        Coroutines.Instance.ActionDiférée(() => source.GetComponent<ControleurRpg>().AppliquerDégats(dégats), délaiDégat));
    public bool EstSourceValidePourDommage(GameObject source) => EstSourceEntitéValide(source) && EstSourceComposanteRpgValide(source)
        && !EstSourceTagIdentique(source);


    private bool EstSourceProjectile(GameObject source) => source.CompareTag("Projectile");
    private bool EstSourceTagIdentique(GameObject source) => source.tag == Parent.tag;
    private bool EstSourceEntitéValide(GameObject source) => !EstSourceProjectile(source)
        && source.GetComponent<ControleurEntité>() != null && !source.GetComponent<ControleurEntité>().EstMort;
    private bool EstSourceComposanteRpgValide(GameObject source) => source.GetComponent<ControleurRpg>() != null;

}
