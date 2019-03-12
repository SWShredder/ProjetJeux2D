using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffetPickup
{
    protected ObjetPickup objetPickup;
    protected ControleurJoueur controleurJoueur;

    public EffetPickup(ObjetPickup objetPickup)
    {
        this.objetPickup = objetPickup;
        controleurJoueur = GameObject.FindGameObjectWithTag("Player").GetComponent<ControleurJoueur>();
    }
    public abstract void Appliquer();
}

public class OmnishotFinVieEffet : EffetPickup
{
    public OmnishotFinVieEffet(ObjetPickup objetPickup) : base(objetPickup) => this.objetPickup = objetPickup;
    public override void Appliquer()
    {
        controleurJoueur.DécorationsAttaque1.Add(Projectile.Effet.OmnishotFinVie);
    }
}
