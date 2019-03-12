/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/05
 *      
 *      Incomplet pour le moment. Permet de gérer les points de santé et la mort.
 */
using UnityEngine;
using UnityEngine.UI;

public class ControleurRpg : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int santéMaximale = 50;

    private ControleurEntité controleurEntité;
    private Canvas canvas;


    public int SantéMaximale { get => santéMaximale; set => santéMaximale = value; }
    public int Santé { private set; get; }



    // Start is called before the first frame update
    void Start()
    {
        controleurEntité = GetComponent<ControleurEntité>();
        if (controleurEntité == null)
        {
            Utilitaire.MessageErreur(this, "ControleurEntité n'a pas pu être initialisé");
        }
        canvas = GetComponentInChildren<Canvas>();
        Santé = santéMaximale;

    }

    public void AppliquerDégats(int dégats)
    {
        GetComponent<EffetsVisuels>().Clignoter(gameObject, 0.04f);
        if (dégats >= 0) AfficherDégats(dégats);
        else AfficherGuérison(dégats * -1);
        if(dégats < 0) Santé = (Santé - dégats) > SantéMaximale ? SantéMaximale : Santé - dégats;
        else Santé -= dégats < Santé ? dégats : Santé;
        if (Santé <= 0) SurMort();
    }

    private void AfficherDégats(int dégats)
    {
        var popups = NombresPopupPool.Instance.ObtenirNombrePopup(dégats);
        var outline = popups.GetComponentInChildren<Outline>();
        var text = popups.GetComponentInChildren<Text>();

        outline.effectColor = new Color(0.73f, 0.13f, 0.06f, 0.5f);
        text.color = new Color(0.89f, 0.38f, 0.15f, 1f);

        popups.transform.SetParent(canvas.transform);
        popups.transform.position =
            new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f),
             transform.position.y + Random.Range(-0.1f, 0.1f),
             transform.position.z);

    }

    private void AfficherGuérison(int guérison)
    {
        var popups = NombresPopupPool.Instance.ObtenirNombrePopup(guérison);
        var outline = popups.GetComponentInChildren<Outline>();
        var text = popups.GetComponentInChildren<Text>();     

        outline.effectColor = new Color(0f, 0.09f, 0.06f, 0.5f);
        text.color = new Color(0.24f, 0.91f, 0.18f, 1f);

        popups.transform.SetParent(canvas.transform);
        popups.transform.position =
            new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f),
             transform.position.y + Random.Range(-0.1f, 0.1f),
             transform.position.z);
    }

    private void SurMort() // OnDeath
    {
        controleurEntité.Mourir();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
