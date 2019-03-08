/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/05
 *      Dernière modification: 2019/03/05
 *     
 *      Modifications:
 *      2019/03/05 ajout MessageErreur, ObtenirDurée
 */ 

using System.Collections;
using System;
using UnityEngine;

public static class Utilitaire
{
    // une référence vers joueur est conservée pour éviter de devoir faire de multiples appels à une méthodes
    private static GameObject référenceJoueur;

    /// <summary>
    /// Permet d'obtenir une référence rapide vers le gameObject du joueur. Nécessite que le joueur ait
    /// le Tag "Player"
    /// </summary>
    /// <returns>Un GameObject</returns>
    public static GameObject ObtenirJoueur()
    {
        if (référenceJoueur == null) référenceJoueur = GameObject.FindGameObjectWithTag("Player");
        return référenceJoueur;
    }
    public static void MessageErreur(Component component, string message)
    {
        Debug.Log($"{component}: {message}"); 
    }

    /// <summary>
    /// Permet d'ajuster directement la durée d'une animation en fonction d'une durée en secondes voulue.
    /// </summary>
    /// <param name="animateur">L'Animator de la source</param>
    /// <param name="animInfo">La state d'animation pour obtenir durée actuelle</param>
    /// <param name="duréeVoulue">La durée voulue en secondes</param>
    /// <param name="nomParamètre">Le nom du paramètre float dans l'Animator</param>
    public static void AjusterLongueurAnimation(Animator animateur, AnimatorStateInfo animInfo, float duréeVoulue, string nomParamètre) 
        => animateur.SetFloat(nomParamètre, animInfo.ObtenirCoefficientAjustementVitesse(duréeVoulue));

    /// <summary>
    /// Permet d'obtenir le coefficient nécessaire afin d'ajuster la vitesse d'une animation pour qu'elle
    /// coordonne avec une durée voulue.
    /// </summary>
    /// <param name="animInfo">La state d'animation pour obtenir durée actuelle</param>
    /// <param name="duréeVoulue">La durée voulue en secondes</param>
    /// <returns>Le coefficent d'ajustement</returns>
    public static float ObtenirCoefficientAjustementVitesse(this AnimatorStateInfo animInfo, float duréeVoulue)
        =>  animInfo.length / duréeVoulue;

    public static float ObtenirDurée(this Animator animateur)
    {
        return animateur.GetCurrentAnimatorStateInfo(0).length;
    }
}

public class Coroutines
{
    private static Coroutines instance;
    public static Coroutines Instance
    {
        private set => instance = value;
        get 
        {
            if (instance == null) instance = new Coroutines();
            return instance;
        }
    }

    private Coroutines()
    {

    }

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

        while ((bool)condition.DynamicInvoke())
        {
            yield return new WaitForSeconds(0.1f);
        }
        action.DynamicInvoke();

    }

    public IEnumerator ActionRépétéeJusquàCondition(Action action, Func<bool> condition)
    {

        while ((bool)condition.DynamicInvoke())
        {
            action.DynamicInvoke();
            yield return new WaitForSeconds(0.1f);
        }

    }

    public IEnumerator ActionRépétéeJusquàTemps(Action action, float secondes)
    {

        for (float i = secondes; i >= 0; i -= 0.1f)
        {

            action.DynamicInvoke();
            yield return new WaitForSeconds(0.1f);
        }

    }

    public IEnumerator ActionDiférée(Action action, float secondes)
    {

        yield return new WaitForSeconds(secondes);
        action.DynamicInvoke();

    }
}
