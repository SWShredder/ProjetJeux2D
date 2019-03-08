/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/04
 *      Dernière modification: 2019/03/09
 *      
 *      Comportement des projectiles. Pour le moment le comportement est limité, mais il permet de s'assurer que
 *      les projectiles ne "collideront" pas ensemble, sur une Entité avec 0 points de santé, ou encore sur le 
 *      parent. (Le parent doit être ajouté après l'instantiation)
 * 
 */
using System;
using System.Collections;
using MyBox;
using UnityEngine;

public class ProjectileComportement : MonoBehaviour
{
    // --- Champs --- //
    protected Vector2 direction;
    protected Rigidbody2D corpsPhysique;
    public GameObject Parent { set; get; }

    [SerializeField]
    protected bool estProjectile = true;
    [SerializeField, ConditionalField("estProjectile", true)]
    protected float vitesse = 2f;
    [SerializeField, ConditionalField("estProjectile", false)]
    protected float duréeVie = 5f;
    [SerializeField]
    protected int dégats = 1;
    [SerializeField, Min(0.1f)]
    protected float portée = 6f;
    [SerializeField, Min(0.05f)]
    protected float cooldown = 0.35f;
    [SerializeField, Min(0.01f)]
    protected float délaiDégat = 0.01f;
    [SerializeField]
    protected bool EstTransperçant = false;

    public float Portée { get => portée; protected set => portée = value; }
    public float Cooldown { get => cooldown; protected set => cooldown = value; }


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, DuréeVie());
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

    protected float DuréeVie() => vitesse > 0 ? portée / vitesse : duréeVie;

    protected void DéclencherDégats(GameObject source) => source.GetComponent<ControleurRpg>().StartCoroutine(
        Coroutines.Instance.ActionDiférée(() => source.GetComponent<ControleurRpg>().AppliquerDégats(dégats), délaiDégat));
    protected bool EstSourceValidePourDommage(GameObject source) => EstSourceEntitéValide(source) && EstSourceComposanteRpgValide(source)
        && !EstSourceTagIdentique(source);
    private bool EstSourceProjectile(GameObject source) => source.CompareTag("Projectile");
    private bool EstSourceTagIdentique(GameObject source) => source.tag == Parent.tag;
    private bool EstSourceEntitéValide(GameObject source) => !EstSourceProjectile(source)
        && source.GetComponent<ControleurEntité>() != null && !source.GetComponent<ControleurEntité>().EstMort;
    private bool EstSourceComposanteRpgValide(GameObject source) => source.GetComponent<ControleurRpg>() != null;

}
