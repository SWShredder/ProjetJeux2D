/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/04
 *      
 *      Comportement des projectiles. Pour le moment le comportement est limité, mais il permet de s'assurer que
 *      les projectiles ne "collideront" pas ensemble, sur une Entité avec 0 points de santé, ou encore sur le 
 *      parent. (Le parent doit être ajouté après l'instantiation)
 * 
 */
using System;
using System.Collections;
using UnityEngine;

public class ProjectileComportement : MonoBehaviour
{
    // --- Champs --- //
    protected Vector2 direction;
    protected Rigidbody2D corpsPhysique;
    public GameObject Parent { set; get; }

    [SerializeField]
    protected float vitesse = 2f;

    [SerializeField]
    protected int dégats = 1;

    [SerializeField]
    protected float duréeVie = 5f;

    [SerializeField]
    protected float délaiDégat = 0f;

    [SerializeField]
    protected bool EstTransperçant = false;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, duréeVie);
        corpsPhysique = GetComponent<Rigidbody2D>();
        direction = transform.rotation == Quaternion.Euler(0f, 0f, 0f) ? Vector2.right : Vector2.left;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        corpsPhysique.velocity = direction * vitesse;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var source = collision.gameObject;
        if (EstSourceValidePourDommage(source)) DéclencherDégats(source);
        if (!EstTransperçant) Destroy(this.gameObject);
    }


    protected void DéclencherDégats(GameObject source) => source.GetComponent<ControleurRpg>().StartCoroutine(
        Coroutines.Instance.ActionDiférée(() => source.GetComponent<ControleurRpg>().AppliquerDégats(dégats), délaiDégat));
    protected bool EstSourceValidePourDommage(GameObject source) => EstSourceEntitéValide(source) && EstSourceComposanteRpgValide(source);
    private bool EstSourceProjectile(GameObject source) => source.CompareTag("Projectile");
    private bool EstSourceEntitéValide(GameObject source) => !EstSourceProjectile(source)
        && source.GetComponent<ControleurEntité>() != null && !source.GetComponent<ControleurEntité>().EstMort;
    private bool EstSourceComposanteRpgValide(GameObject source) => source.GetComponent<ControleurRpg>() != null;

}
