/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/05
 *      Dernière modification: 2019/03/05
 *      
 *      La majorité de l'implémentation de ControleurEnnemi se trouve dans ControleurEntité. La
 *      classe sert surtout à distinguer les quelques nuances qu'il pourrait y avoir au cour du
 *      développement. 
 *    
 */
using UnityEngine;

public class ControleurEnnemi : ControleurEntité
{
    [SerializeField]
    private Transform positionProjectiles;

    [SerializeField]
    private GameObject projectile;

    // --- Public --- //
    
    public EntitéState ÉtatInactif { set; get; }
    public EntitéState ÉtatMouvement { set; get; }
    public EntitéState ÉtatAttaque { set; get; }
    public EntitéState ÉtatMort { set; get; }

    // --- Constructeur --- //
    public ControleurEnnemi()
    {
        ÉtatInactif = new EnnemiInactifState(this);
        ÉtatMort = new EnnemiMortState(this);
        ÉtatMouvement = new EnnemiMouvementState(this);
        ÉtatAttaque = new EnnemiAttaqueState(this);
    }

    // --- Méthodes --- //
    public override void Attaquer()
    {
        var instance = Instantiate(
            projectile,
            positionProjectiles.position,
            EstFaceDroite ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f));
        StartCoroutine(VérrouillerAttaque(instance.GetComponent<Projectile>().Cooldown));
        instance.GetComponent<Projectile>().Parent = this.gameObject;
    }

    public void SurDemandeMouvement(Vector2 direction)
    {
        if (!EstEnAttaque && !EstEnCooldownAttaque) Mouvement = direction;
    }

    public void SurDemandeAttaque()
    {
        if(!EstEnAttaque && EstAttaquePrête && !EstEnCooldownAttaque) État = ÉtatAttaque;
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
            Debug.Log("La composante Position Projectile n'a pas été définie et la méthode attaque ne pourra pas" +
                " fonctionner.");
        }
    }
}
