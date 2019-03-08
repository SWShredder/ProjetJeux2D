/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/05
 *      Dernière modification: 2019/03/06
 *      
 *      Pour les ennemis, les states servent principalement à gérer les animations et à réutiliser
 *      le maximum de code possible.
 *      
 *      Modif:
 *      Ajout state EnnemiMouvementState. 2019/03/06 YS
 *      
 *      À faire: ajout state attaquer. Doit terminer ControleurAI avant de pouvoir
 *      compléter ce script-ci
 *      
 */
public class EnnemiInactifState : InactifState
{
    private ControleurEnnemi controleurEnnemi;
    public EnnemiInactifState(ControleurEntité controleur) : base(controleur) 
        => controleurEnnemi = (ControleurEnnemi)controleur;

    public override void Actualiser()
    {
        if (controleur.EstEnCooldownAttaque) return;
        else if (controleur.Mouvement.magnitude > 0) controleur.État = controleurEnnemi.ÉtatMouvement;
    }
}

public class EnnemiMouvementState : MouvementState
{
    private ControleurEnnemi controleurEnnemi;
    public EnnemiMouvementState(ControleurEntité controleur) : base(controleur)
        => controleurEnnemi = (ControleurEnnemi)controleur;

    public override void Actualiser()
    {
        //if (controleur.EstEnCooldownAttaque) controleur.État = controleurEnnemi.ÉtatInactif;
        //else if (controleur.EstEnAttaque) controleur.État = controleurEnnemi.ÉtatAttaque;
        if (controleur.Mouvement.magnitude == 0) controleur.État = controleurEnnemi.ÉtatInactif;
    }
}

public class EnnemiAttaqueState : AttaqueState
{
    private ControleurEnnemi controleurEnnemi;
    public EnnemiAttaqueState(ControleurEntité controleur) : base(controleur)
        => controleurEnnemi = (ControleurEnnemi)controleur;

    public override void Actualiser()
    {
        if (EstCooldownStateTerminé()) controleur.État = controleurEnnemi.ÉtatInactif;
    }
}

public class EnnemiMortState : MortState
{
    private ControleurEnnemi controleurEnnemi;
    public EnnemiMortState(ControleurEntité controleur) : base(controleur)
        => controleurEnnemi = (ControleurEnnemi)controleur;

    public override void Actualiser()
    {

    }

    public override void Terminer()
    {
        
    }
}
