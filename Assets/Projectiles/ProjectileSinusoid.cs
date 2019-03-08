using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSinusoid : ProjectileComportement
{
    [SerializeField]
    private float frequenceSinusoid = 2.0f;

    [SerializeField]
    private float intensitéSinusoid = 0.25f;

    void Start()
    {
        Destroy(gameObject, duréeVie);
        corpsPhysique = GetComponent<Rigidbody2D>();
        direction = transform.rotation == Quaternion.Euler(0f, 0f, 0f) ? Vector2.right : Vector2.left;
    }

    void FixedUpdate()
    {
        Debug.Log("pink shoot");      
        corpsPhysique.velocity = direction * vitesse;
        this.transform.position = this.transform.position + transform.up * Mathf.Sin(Time.time * frequenceSinusoid) * intensitéSinusoid;
    }
}
