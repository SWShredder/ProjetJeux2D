/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/08
 *      Dernière modification: 2019/03/08
 * 
 */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class ProjectileComportement : ScriptableObject
{
    protected Projectile projectile;
    public ProjectileComportement(Projectile projectile) { this.projectile = projectile; }
    public abstract void Initialiser();
    public abstract void Actualiser();
    public abstract void SurImpact(Collider2D collider);
    public abstract void SurImpactAvecEntité(Collider2D collider);
    public abstract void Terminer();
}

public class StandardProjectileComportement : ProjectileComportement
{
    public StandardProjectileComportement(Projectile projectile) : base(projectile) { }
    public override void Initialiser()
    {
        projectile.StartCoroutine(Coroutines.Instance.ActionDiférée(projectile.SurFinVie, projectile.ObtenirDuréeVie()));
        projectile.CorpsPhysique = projectile.GetComponent<Rigidbody2D>();
        projectile.Direction = projectile.transform.rotation == Quaternion.Euler(0f, 0f, 0f) ? Vector2.right : Vector2.left;
    }
    public override void Actualiser() => projectile.CorpsPhysique.velocity = projectile.transform.right * projectile.Vitesse;
    public override void SurImpact(Collider2D collider) => projectile.SurFinVie();
    public override void SurImpactAvecEntité(Collider2D collider)
    {
        if (projectile.EstSourceValidePourDommage(collider.gameObject)) projectile.DéclencherDégats(collider.gameObject);
        if (projectile.EstTransperçant) return;
        projectile.SurFinVie();
    }
    public override void Terminer()
    {

    }
}
