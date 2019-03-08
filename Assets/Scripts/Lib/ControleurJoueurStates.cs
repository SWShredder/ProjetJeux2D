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
    private ControleurJoueur joueurControleur;
    public JoueurInactifState(ControleurEntité controleur) : base(controleur)
        => joueurControleur = (ControleurJoueur)controleur;
    public override void Actualiser()
    {
        if (Input.GetButton("Fire1")) controleur.État = joueurControleur.ÉtatAttaque;

        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        controleur.Mouvement = velocity;

        if (velocity.magnitude > 0) controleur.État = joueurControleur.ÉtatMouvement;

    }
}


public class JoueurMouvementState : MouvementState
{
    private ControleurJoueur joueurControleur;
    public JoueurMouvementState(ControleurEntité controleur) : base(controleur)
        => joueurControleur = (ControleurJoueur)controleur;

    public override void Actualiser()
    {
        if (Input.GetButton("Fire1"))
        {
            controleur.État = joueurControleur.ÉtatAttaque;
        }
        else
        {
            Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            controleur.Mouvement = velocity;
            if (velocity.magnitude == 0) controleur.État = joueurControleur.ÉtatInactif;
        }
    }

}


public class JoueurAttaqueState : AttaqueState
{
    private ControleurJoueur joueurControleur;
    public JoueurAttaqueState(ControleurEntité controleur) : base(controleur)
        => joueurControleur = (ControleurJoueur)controleur;

    public override void Actualiser()
    {
        if (EstAttaquePrête()) controleur.État = joueurControleur.ÉtatInactif;
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
