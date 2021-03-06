﻿/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/08
 *      Dernière modification: 2019/03/08
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileDécorateur : ProjectileComportement
{
    protected ProjectileComportement comportement;
    public ProjectileDécorateur(Projectile projectile) : base(projectile) => comportement = projectile.Comportement;
    public override void Actualiser() => comportement.Actualiser();
    public override void Initialiser() => comportement.Initialiser();
    public override void SurImpact(Collider2D collider) => comportement.SurImpact(collider);
    public override void SurImpactAvecEntité(Collider2D collider) => comportement.SurImpactAvecEntité(collider);
    public override void Terminer() => comportement.Terminer();
}


public class BoomerangProjectileDécorateur : ProjectileDécorateur
{
    public BoomerangProjectileDécorateur(Projectile projectile) : base(projectile) => comportement = projectile.Comportement;
    public override void Actualiser()
    {
        base.Actualiser();
        //projectile.CorpsPhysique.velocity = Vector2.up * 2 + projectile.CorpsPhysique.velocity;
    }
}

public class OmniShotSurFinVieProjectileDécorateur : ProjectileDécorateur
{
    public OmniShotSurFinVieProjectileDécorateur(Projectile projectile) : base(projectile) => comportement = projectile.Comportement;
    public override void Terminer()
    {
        Object omnishot = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Projectiles/OmniShot.prefab", typeof(GameObject));
        var instance = GameObject.Instantiate(omnishot, projectile.transform.position, projectile.transform.rotation) as GameObject;
        var projectiles = instance.GetComponentsInChildren<Projectile>();
        foreach (Projectile p in projectiles)
        {
            p.Parent = projectile.Parent;
        }
        GameObject.Destroy(instance, projectiles[0].ObtenirDuréeVie());
        base.Terminer();
    }

}

public class OmniShotImpactEntitéProjectileDécorateur : ProjectileDécorateur
{
    public OmniShotImpactEntitéProjectileDécorateur(Projectile projectile) : base(projectile) => comportement = projectile.Comportement;
    public override void SurImpactAvecEntité(Collider2D collider)
    {
        Object omnishot = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Projectiles/OmniShot.prefab", typeof(GameObject));
        var instance = GameObject.Instantiate(omnishot, projectile.transform.position, projectile.transform.rotation) as GameObject;
        var projectiles = instance.GetComponentsInChildren<Projectile>();
        foreach (Projectile p in projectiles)
        {
            p.Parent = projectile.Parent;
        }
        GameObject.Destroy(instance, projectiles[0].ObtenirDuréeVie());
        base.SurImpactAvecEntité(collider);
    }
}

public class OmniShotSurImpactProjectileDécorateur : ProjectileDécorateur
{
    public OmniShotSurImpactProjectileDécorateur(Projectile projectile) : base(projectile) => comportement = projectile.Comportement;
    public override void SurImpact(Collider2D collider)
    {
        Object omnishot = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Projectiles/OmniShot.prefab", typeof(GameObject));
        var instance = GameObject.Instantiate(omnishot, projectile.transform.position, projectile.transform.rotation) as GameObject;
        var projectiles = instance.GetComponentsInChildren<Projectile>();
        foreach (Projectile p in projectiles)
        {
            p.Parent = projectile.Parent;
        }
        GameObject.Destroy(instance, projectiles[0].ObtenirDuréeVie());
        base.SurImpact(collider);
    }
}


