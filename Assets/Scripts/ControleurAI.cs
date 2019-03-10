/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019-03-06
 * 
 *      But du script:
 *      Ce script sert à interfacer avec un ControleurEnnemi pour contrôler le comportement d'un personnage 
 *      non joueur comme un ennemi. À l'instar de ControleurEntité qui est aidé par EntitéStates pour gérer 
 *      son comportement, ControleurAI est suffisament complexe pour être géré par une "State Machine".
 *         
 */
using System.Collections;
using System;
using UnityEngine;
using static Utilitaire;

public class ControleurAI : MonoBehaviour
{
    [SerializeField, Min(0), Tooltip("Rayon à partir duquel l'AI devient actif.")]
    private float rayonActivité = 10f;

    [SerializeField, Min(0), Tooltip("Rayon à partir duquel l'AI tente de se positionner pour une attaque.")]
    private float rayonProximité = 3f;

    [SerializeField, Min(0), Tooltip("La portée de l'attaque de l'agent [temporaire].")]
    private float portéeAttaque = 0.8f;

    private ControleurEntité controleurEntité;

     
    public float RayonActivité { get => rayonActivité; }
    public bool EstEnVeille { set; get; }
    public bool EstOccupé { set; get; }
    public AIState EndormiAIState { private set; get; }
    public AIState ÀDistanceAIState { private set; get; }
    public AIState EnApprocheAIState { private set; get; }
    public AIState EnAttaqueAIState { private set; get; }
    public AIState ÀProximitéAIState { private set; get; }
    public AIState EnPositionnementAIState { private set; get; }

    public AIState AttaqueAveugleAIState { private set; get; }
    ///<summary>Retourne un Vector2 qui représente la position du joueur</summary>
    public Vector2 PositionJoueur { get => ObtenirJoueur().transform.position; }

    private AIState état;
    public AIState État
    {
        set
        {
            état = value;
            état.Initialiser();
        }
        get => état;
    }

    public ControleurAI()
    {
        EndormiAIState = new EndormiAIState(this);
        ÀDistanceAIState = new ÀDistanceAIState(this);
        EnApprocheAIState = new EnApprocheAIState(this);
        EnAttaqueAIState = new EnAttaqueAIState(this);
        ÀProximitéAIState = new ÀProximitéAIState(this);
        EnPositionnementAIState = new EnPositionnementAIState(this);
    }

    void Start()
    {
        controleurEntité = GetComponent<ControleurEntité>();
        if (controleurEntité == null) MessageErreur(this, "***ce script ne peut fonctionner sans un ControleurEntité valide");
        État = EndormiAIState;
    }

    void FixedUpdate()
    {
        if (EstEnVeille)
        {
            if (controleurEntité.Mouvement.magnitude > 0) MessageErreur(this, "***n'est pas censé pouvoir se déplacer en veille");
            return;
        }
        else if (EstOccupé) return;
        else État.Actualiser();
    }


    public void CommanderMouvement(Vector2 direction) => (controleurEntité as ControleurEnnemi).SurDemandeMouvement(direction);
    public float ObtenirDistanceAvecJoueur() => Vector2.Distance(transform.position, PositionJoueur);
    public virtual Vector2 ObtenirDirectionVersJoueur() => (PositionJoueur - (Vector2)transform.position).normalized;
    public bool EstJoueurÀDistance() => ObtenirDistanceAvecJoueur() < RayonActivité;
    public Vector2 ObtenirDirectionHorizontale() => ObtenirDirectionVersJoueur() * new Vector2(1, 0);
    public bool EstJoueurÀProximité() => (ObtenirDistanceAvecJoueur() <= rayonProximité);
    public Vector2 ObtenirDirectionPourPositionnement() => transform.position.x > PositionJoueur.x ?
        (PositionJoueur - (Vector2)transform.position + new Vector2(portéeAttaque, 0)).normalized 
        : (PositionJoueur - (Vector2)transform.position - new Vector2(portéeAttaque, 0)).normalized;
    public bool EstPositionnéPourAttaque() => EstPositionnéHorizontalement() && EstPositionnéVerticalement() && EstFaceAuJoueur();
    public void CommanderAttaque() => (controleurEntité as ControleurEnnemi).SurDemandeAttaque();

    private bool EstPositionnéVerticalement() => Mathf.Abs(transform.position.y - PositionJoueur.y) <= 0.3f;
    private bool EstPositionnéHorizontalement() => Mathf.Abs(transform.position.x - PositionJoueur.x) <= portéeAttaque + 0.5f;
    private bool EstFaceAuJoueur() => (transform.position.x - PositionJoueur.x) < 0 ? controleurEntité.EstFaceDroite : !controleurEntité.EstFaceDroite;

    /*
     *      Les méthodes qui servent à pouvoir généraliser des tâches d'agent AI avec
     *      des coroutines. 
     *      
     *      Action est une méthode qui ne prend pas d'arguments et Func<bool>
     *      est n'importe qu'elle méthode qui ne prend pas d'argument et qui renvoit une valeur
     *      booléene. Func<bool> est similaire a une Predicate, mais ne nécessite pas d'argument.
     *      
     *      // Exécuterait la méthode nomMéthode() toute les cycles pendant 3 secondes
     *      ex: StartCoroutine(ActionRépétéeJusquàTemps(nomMéthode, 3f); 
     *      
     *      // Mettera en veille un agent jusqu'à ce que le joueur soit à proximité
     *      ex: controleur.StartCoroutine(MiseEnVeilleJusquàCondition(controleur.EstJoueurÀDistance));
     * 
     */
    public IEnumerator ActionConditionnelle(Action action, Func<bool> condition)
    {
        EstEnVeille = true;
        while ((bool)condition.DynamicInvoke())
        {
            yield return new WaitForSeconds(0.1f);
        }
        action.DynamicInvoke();
        EstEnVeille = false;
    }

    public IEnumerator ActionRépétéeJusquàCondition(Action action, Func<bool> condition)
    {
        EstOccupé = true;
        while ((bool)condition.DynamicInvoke())
        {
            action.DynamicInvoke();
            yield return new WaitForSeconds(0.1f);
        }
        EstOccupé = false;
    }

    public IEnumerator ActionRépétéeJusquàTemps(Action action, float secondes)
    {
        EstOccupé = true;
        for (float i = secondes; i >= 0; i -= 0.1f)
        {

            action.DynamicInvoke();
            yield return new WaitForSeconds(0.1f);
        }
        EstOccupé = false;
    }

    public IEnumerator ActionDiférée(Action action, float secondes)
    {
        EstOccupé = true;
        yield return new WaitForSeconds(secondes);
        action.DynamicInvoke();
        EstOccupé = false;
    }


    public IEnumerator MiseEnVeille(float secondes)
    {
        EstEnVeille = true;
        yield return new WaitForSeconds(secondes);
        EstEnVeille = false;
    }

    public IEnumerator MiseEnVeilleJusquàCondition(Func<bool> condition)
    {
        EstEnVeille = true;
        while (!(bool)condition.DynamicInvoke())
        {
            yield return new WaitForSeconds(0.1f);
        }
        EstEnVeille = false;
    }



}
