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
using static Utilitaire;

public class ControleurJoueur : ControleurEntité
{
    [SerializeField]
    private Transform positionProjectiles;

    [SerializeField]
    private GameObject projectile;

    public EntitéState ÉtatInactif { private set; get; }
    public EntitéState ÉtatMouvement { private set; get; }
    public EntitéState ÉtatAttaque { private set; get; }
    public EntitéState ÉtatMort { private set; get; }

    public ControleurJoueur()
    {
        ÉtatInactif = new JoueurInactifState(this);
        ÉtatMouvement = new JoueurMouvementState(this);
        ÉtatAttaque = new JoueurAttaqueState(this);
        ÉtatMort = new JoueurMortState(this);
    }


    public override void Attaquer()
    {
        var instance = Instantiate(
            projectile,
            positionProjectiles.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
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
        if (positionProjectiles == null)
        {
            MessageErreur(this, "La composante Position Projectile n'a pas été définie et la méthode attaque ne pourra pas" +
                " fonctionner.");
        }
    }
}
