using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;



public class ObjetPickup : MonoBehaviour
{
    public enum Effet
    {
        Omnishot, Aucun
    }

    public Transform positionSol;
    private EffetPickup effetPickup;
    [SerializeField]
    private Effet effet = Effet.Aucun;
    [SerializeField]
    private int santé;
    [SerializeField]
    private int santéMaximale;
    [SerializeField]
    private float vitesseMouvement;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float débitTir;

    public int Santé { get => santé; private set => santé = value; }
    public int SantéMaximale { get => santéMaximale; private set => santéMaximale = value; }
    // Start is called before the first frame update
    void Start()
    {
        effetPickup = AppliquerEffetSurPickup();
    }

    private void LateUpdate()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.positionSol.position.y);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Fire1") && collision.gameObject.CompareTag("Player"))
        {
            AppliquerStats(collision.gameObject);
            collision.gameObject.GetComponent<ControleurJoueur>().EstSurPickup = false;
            if(effetPickup != null) effetPickup.Appliquer();
            Destroy(this.gameObject);
          
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var source = collision.gameObject;
        if (source.CompareTag("Player"))
        {
            source.GetComponent<ControleurJoueur>().EstSurPickup = true;         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var source = collision.gameObject;
        if (source.CompareTag("Player"))
        {
            source.GetComponent<ControleurJoueur>().EstSurPickup = false;
        }
    }

    private void AppliquerStats(GameObject source)
    {
        var controleur = source.GetComponent<ControleurRpg>();
        if (Santé != 0) controleur.AppliquerDégats(santé * -1);
        if (SantéMaximale != 0) controleur.SantéMaximale += SantéMaximale;
    }

    private EffetPickup AppliquerEffetSurPickup()
    {
        switch (effet)
        {
            case Effet.Omnishot:
                return new OmnishotFinVieEffet(this);
            default:
                return null;
        }
    }
}
