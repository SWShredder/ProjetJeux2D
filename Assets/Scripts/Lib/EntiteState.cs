/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/03
 *      Dernière modification: 2019/03/05
 *      
 *      
 *      Les states servent à contrôler un EntitéControleur. Chaque state gère
 *      l'input (joueur ou AI) et fait les manipulations à la composante Animator de
 *      EntitéControleur. Chaque state contient les méthodes:
 *      
 *            
 *      Initialiser(): Se fait appelée une seule fois lorsqu'il y a changement
 *                     de state. L'appel se fait par la méthode Set de État dans 
 *                     EntitéControleur lorsqu'on modifie la valeur de État d'un
 *                     controleur.
 *                    
 *      Actualiser():  C'est la méthode où la logique est définie pour faire le changement
 *                     de states. Elle est appelée à chaque frame.
 *      
 *      Terminer():    La méthode est appelée par EntitéControleur à l'image de Initialiser() 
 *                     avant de quitter.
 *                     
 *                     
 *                     
 *      L'utilisation des classes abstraites InactifState, MouvementState, AttaqueState, etc. en utilisant
 *      l'héritage permet de ne pas avoir à définir l'implémentation des méthodes Initialiser() et Terminer() 
 *      puisque celles-ci sont déjà définies dans ces classes. 
 *                                 
 */
using UnityEngine;

public abstract class EntitéState
{
    private static readonly bool debug = true;
    protected ControleurEntité controleur;
    public EntitéState(ControleurEntité controleur)
    {
        this.controleur = controleur;
    }
    public abstract void Initialiser();
    public abstract void Actualiser();
    public abstract void Terminer();

    protected void Message(string message)
    {
        Utilitaire.MessageErreur(controleur, message);
    }
}

public abstract class InactifState : EntitéState
{
    public InactifState(ControleurEntité controleur) : base(controleur) { }
    public override void Initialiser()
    {
        Message("EntitéState -> Inactif");
        controleur.EstEnMouvement = false;
    }
    public override void Terminer() { }

}

public abstract class MouvementState : EntitéState
{
    public MouvementState(ControleurEntité controleur) : base(controleur) { }
    public override void Initialiser()
    {
        Message("EntitéState -> En Mouvement");
        controleur.Animateur.SetBool("EstEnMouvement", true);
        controleur.EstEnMouvement = true;
    }
    public override void Terminer()
    {
        controleur.Mouvement = Vector2.zero;
        controleur.EstEnMouvement = false;
        controleur.Animateur.SetBool("EstEnMouvement", false);
    }
}

public abstract class AttaqueState : EntitéState
{
    protected float attaqueChrono;
    public AttaqueState(ControleurEntité controleur) : base(controleur) { }

    public override void Initialiser()
    {
        Message("EntitéState -> En Attaque");
        controleur.Attaquer();
        controleur.EstEnAttaque = true;
        controleur.Animateur.SetBool("EstEnAttaque", true);      
        attaqueChrono = Time.fixedTime;
    }

    public override void Terminer()
    {
        controleur.EstEnAttaque = false;
        controleur.Animateur.SetBool("EstEnAttaque", false);
        controleur.MettreEnCooldown();
    }

    protected bool EstCooldownStateTerminé() => Time.fixedTime - attaqueChrono >= controleur.CooldownAttackState;

}

public abstract class MortState : EntitéState
{

    public MortState(ControleurEntité controleur) : base(controleur) { }

    public override void Initialiser()
    {
        Message("EntitéState -> Mort");
        controleur.GetComponent<EffetsVisuels>().Fondue(controleur.gameObject, 2f);
        controleur.Animateur.SetTrigger("Mort");
        GameObject.Destroy(controleur.gameObject, controleur.Animateur.ObtenirDurée() + 0.5f);
    }

}
