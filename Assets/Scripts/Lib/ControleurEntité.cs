/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/03
 *      Dernière modification: 2019/03/07
 *    
 *      Ce script a pour but de mettre en commun le code commun aux Controlleurs d'entité.
 *      Il contrôle, en autres, les paramètres de vitesse de mouvement, l'initialisation du corps
 *      physique et de la composante animator.
 *      
 *      Modif:
 *      Ajout méthode ActualiserMouvement et le triage par les Z pour que les sprites
 *      puissent s'achicher correctement lorsqu'ils sont collés les uns sur les autres. 2019/03/07 YS
 *      
 *           
 */

using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using static Utilitaire;

public abstract class ControleurEntité : MonoBehaviour
{

    [SerializeField, Min(0f), Tooltip("Vitesse de mouvement globale")]
    protected float vitesseMouvement = 1f;
    [SerializeField]
    protected bool paramètresAvancés = true;
    [SerializeField, Min(0f), Tooltip("Vitesse de mouvement sur l'axe vertical"), ConditionalField("paramètresAvancés")]
    protected float coefficientAxeVertical = 0.6f;
    [SerializeField, Min(0.01f), Tooltip("Délai d'inactivité après les attaques"), ConditionalField("paramètresAvancés")]
    protected float cooldownAttaques = 0.2f;
    [SerializeField, Min(0.1f), Tooltip("Délai minimum dans lequel l'entité doit rester dans la state Attaque"), ConditionalField("paramètresAvancés")]
    protected float cooldownAttaqueState = 0.15f;

    protected Rigidbody2D corpsPhysique;
    protected Animator animateur;
    protected EntitéState état;

    // --- Public --- // 
    public virtual bool EstAttaquePrête { protected set; get; }
    public virtual bool EstMort { protected set; get; }
    public virtual bool EstEnAttaque { set; get; }
    public virtual bool EstEnMouvement { set; get; }
    public virtual bool EstFaceDroite { set; get; }
    public virtual bool EstInitialisé { private set; get; }
    public virtual bool EstEnCooldownAttaque { private set; get; }
    public virtual float CooldownAttaques { get => cooldownAttaques; }
    public virtual Rigidbody2D CorpsPhysique { get => corpsPhysique; }
    public virtual Animator Animateur { get => animateur; }
    public virtual Vector2 Mouvement { set; get; }
    public virtual float VitesseMouvement { get => vitesseMouvement; }
    public virtual float CooldownAttackState { set => cooldownAttaqueState = value; get => cooldownAttaqueState; }
    public virtual EntitéState État
    {
        set
        {
            if (état != null) état.Terminer();
            état = value;
            état.Initialiser();
        }
        get
        {
            if (état == null)
            {
                MessageErreur(this, "Une tentative a été faite pour obtenir l'état d'un EntitéControleur alors que celui-ci" +
                    " n'était pas encore défini.");
            }
            return état;
        }
    }



    // --- Constructeur --- //
    public ControleurEntité()
    {
        EstFaceDroite = true;
        EstEnMouvement = false;
    }

    // --- Méthodes --- //
    public abstract void Attaquer();

    protected void ActualiserDirection(bool estFaceDroite)
    {
        if (EstFaceDroite == estFaceDroite) return;
        EstFaceDroite = estFaceDroite;
        transform.rotation = EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f);
    }

    protected void ActualiserMouvement()
    {
        corpsPhysique.velocity = Mouvement * vitesseMouvement * new Vector2(1, coefficientAxeVertical);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y);
        if (Mouvement.magnitude == 0)
        {
            //EstEnMouvement = false;
        }
    }

    public virtual void Mourir()
    {
        EstMort = true;
        EstEnMouvement = false;
        EstEnAttaque = false;
        Mouvement = Vector2.zero;
        ActualiserMouvement();
    }

    protected void Initialiser()
    {
        corpsPhysique = GetComponent<Rigidbody2D>();
        animateur = GetComponent<Animator>();

        // Vérification si les composantes nécessaires sont présentes et affiche des messages d'erreur si nécessaire.
        if (corpsPhysique == null) MessageErreur(this, "n'a pas de composante RigidBody2D assigné et ne pourra pas être déplacé.");
        if (animateur == null) MessageErreur(this, "n'a pas de composante Animator assigné et ne pourra pas être animé.");

        EstInitialisé = corpsPhysique != null && animateur != null;
        EstEnAttaque = false;
        EstEnCooldownAttaque = false;
        EstAttaquePrête = true;
    }

    void FixedUpdate()
    {
        if (État != null) État.Actualiser();
        if (EstMort) return;
        // Applique la vélocité au corps physique. 
        ActualiserMouvement();

        // Ajuste la direction du personnage seulement si le joueur appuie sur les touches gauche/droite
        if (EstEnMouvement && Mouvement.x != 0) ActualiserDirection(Mouvement.x > 0);
    }

    public void MettreEnCooldown()
    {
        StartCoroutine(AppliquerCooldownAttaques(CooldownAttaques));
    }

    private IEnumerator AppliquerCooldownAttaques(float seconds)
    {
        EstEnCooldownAttaque = true;
        yield return new WaitForSeconds(seconds);
        EstEnCooldownAttaque = false;
    }

    protected IEnumerator VérrouillerAttaque(float seconds)
    {
        EstAttaquePrête = false;
        yield return new WaitForSeconds(seconds);
        EstAttaquePrête = true;
    }

}
