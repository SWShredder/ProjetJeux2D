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

    [SerializeField, Min(0)]
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
        var objetCollision = collision.gameObject;

        if (objetCollision.CompareTag("Projectile") || objetCollision.gameObject.tag == Parent.gameObject.tag) return;
        if (objetCollision.GetComponent<ControleurEntité>() != null && objetCollision.GetComponent<ControleurEntité>().EstMort)
        {
            return;
        }
        else if (collision.gameObject.GetComponent<ControleurRpg>() != null)
        {
            //collision.gameObject.GetComponent<ControleurRpg>().AppliquerDégats(dégats);
            collision.gameObject.GetComponent<ControleurRpg>().StartCoroutine(Coroutines.Instance.ActionDiférée(
                () => collision.gameObject.GetComponent<ControleurRpg>().AppliquerDégats(dégats), délaiDégat));
        }
        if (!EstTransperçant) Destroy(gameObject);
    }
}
