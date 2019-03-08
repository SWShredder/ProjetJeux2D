/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/03
 *      Dernière modification: 2019/03/04
 *      
 *      
 *      Les states servent à contrôler JoueurControleur. Chaque state gère
 *      l'input et fait les manipulations à la composante Animator de
 *      JoueurControleur (héritée de EntitéControleur). Chaque state contient
 *      les méthodes:
 *      
 *      
 *      
 *      Initialiser(): Se fait appelée une seule fois lorsqu'il y a changement
 *                     de state. Son implémentation est définie dans les classes
 *                     abstraites prédéfinie et peut être modifiée au besoin.
 *                    
 *      Actualiser():  C'est la méthode qui gère la logique et qui doit être 
 *                     implémentée par toutes les classes qui ont un EntitéState
 *                     comme parent.
 *      
 *      Terminer():    La méthode est appelée avant de quitter. Elle a les mêmes
 *                     caractéristiques que Initialiser();
 *                     
 *    
 *    
 *      À Faire:    
 *      JoueurMortState est incomplète.
 *                                 
 */

using UnityEngine;


public class JoueurInactifState : InactifState
{
    private ControleurJoueur controleurJoueur;
    public JoueurInactifState(ControleurEntité controleur) : base(controleur)
        => controleurJoueur = (ControleurJoueur)controleur;
    public override void Actualiser()
    {
        if (controleur.EstEnCooldownAttaque) return;
        if (Input.GetButton("Fire1") && controleurJoueur.EstAttaquePrête) controleur.État = controleurJoueur.ÉtatAttaque1;
        else
        {
            Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            controleur.Mouvement = velocity;
            if (velocity.magnitude > 0) controleur.État = controleurJoueur.ÉtatMouvement;
        }
    }
}


public class JoueurMouvementState : MouvementState
{
    private ControleurJoueur controleurJoueur;
    public JoueurMouvementState(ControleurEntité controleur) : base(controleur)
        => controleurJoueur = (ControleurJoueur)controleur;

    public override void Actualiser()
    {
        if (Input.GetButton("Fire1") && controleurJoueur.EstAttaquePrête)
        {
            controleur.État = controleurJoueur.ÉtatAttaque1;
        }
        else
        {
            Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            controleur.Mouvement = velocity;
            if (velocity.magnitude == 0) controleur.État = controleurJoueur.ÉtatInactif;
        }
    }

}


public class JoueurAttaque1State : AttaqueState
{
    private ControleurJoueur controleurJoueur;
    public JoueurAttaque1State(ControleurEntité controleur) : base(controleur)
        => controleurJoueur = (ControleurJoueur)controleur;
    public override void Initialiser()
    {
        Message("EntitéState -> En Attaque 1");
        controleur.EstEnAttaque = true;
        controleur.Animateur.SetBool("EstEnAttaque", true);
        attaqueChrono = Time.fixedTime;
        controleurJoueur.ExécuterAttaque1();
    }

    public override void Actualiser()
    {
        if (controleurJoueur.EstAttaquePrête && Input.GetButton("Fire1")) controleurJoueur.ExécuterAttaque1();
        else if (Input.GetButton("Fire1")) return;
        else if (EstCooldownStateTerminé()) controleur.État = controleurJoueur.ÉtatInactif;
    }

}

public class JoueurAttaque2State : AttaqueState
{
    private ControleurJoueur controleurJoueur;
    public JoueurAttaque2State(ControleurEntité controleur) : base(controleur)
        => controleurJoueur = (ControleurJoueur)controleur;
    public override void Initialiser()
    {
        Message("EntitéState -> En Attaque 2");
        controleur.EstEnAttaque = true;
        controleur.Animateur.SetBool("EstEnAttaque", true);
        attaqueChrono = Time.fixedTime;
        controleurJoueur.ExécuterAttaque2();
    }

    public override void Actualiser()
    {
        if (EstCooldownStateTerminé()) controleur.État = controleurJoueur.ÉtatInactif;
    }
}


public class JoueurMortState : MortState
{
    private ControleurJoueur joueurControleur;
    public JoueurMortState(ControleurEntité controleur) : base(controleur)
        => joueurControleur = (ControleurJoueur)controleur;
    public override void Initialiser()
    {
        base.Initialiser();
        // Code à faire ici.
    }
    public override void Actualiser()
    {
        Debug.Log("La state JoueurMortState n'est pas encore implémentée.");
    }

    public override void Terminer()
    {
        throw new System.NotImplementedException();
    }
}
