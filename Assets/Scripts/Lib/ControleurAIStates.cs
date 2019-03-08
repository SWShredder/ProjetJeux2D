/*      
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/06
 *      
 *      Les AIStates permettent de contrôler le comportement d'un ControleurAI. Contrairement aux states
 *      du EntitéStates, les states de AIStates n'ont pas de méthodes Terminer(). Certaines states ont des
 *      méthodes d'aide, mais la majorité des méthodes utilisées provient du ControleurAI.
 *      
 */
using UnityEngine;
using static Utilitaire;

/// <summary>
/// Le AIState duquel toutes les AIStates dérivent. 
/// </summary>
public abstract class AIState
{
    // Mettre à false si vous voulez enlever les messages liés aux AI states.
    public static readonly bool debug = false;

    protected ControleurAI controleur;
    protected float chronoInterne;
    public AIState(ControleurAI controleurAI)
    {
        controleur = controleurAI;
        if (controleur == null) MessageErreur(controleur, "Une tentative de création d'une AIState a été faite sans ControleurAI valide.");
    }
    public abstract void Initialiser();
    public abstract void Actualiser();
    public virtual void Message(string message)
    {
        if (debug) MessageErreur(controleur, message);
    }
}

/// <summary>
/// La state associée au ennemis qui sont trop éloignés et qui ne sont pas mis à jour avant
/// que la position du joueur soit dans les limites de la propriété RayonActivité. Un agent (AI)
/// dans cette state cherche périodiquement la position du joueur pour savoir s'il est éligible
/// pour sortir de veille.
/// </summary>
public class EndormiAIState : AIState
{
    public EndormiAIState(ControleurAI controleurAI) : base(controleurAI) { }

    public override void Initialiser()
    {
        Message("État: Endormi");
        // Pour s'assurer que le ControleurEntité associé au ControleurAI cesse les mouvements
        controleur.ActualiserMouvement(Vector2.zero);
    }
    public override void Actualiser()
    {
        // Si le joueur est à distance on sort de veille et on change la state du
        // ControleurAI pour ÀDistanceState
        if (controleur.EstJoueurÀDistance()) controleur.État = controleur.ÀDistanceAIState;
        // Sinon on appelle la coroutine de mise en veille conditionnelle
        else controleur.StartCoroutine(controleur.MiseEnVeilleJusquàCondition(controleur.EstJoueurÀDistance));
    }
}

/// <summary>
/// Cette state est associé aux ennemis qui sont maintenant à l'intérieur de leur rayon d'activité.
/// À partir de cette state, il est possible de retourner vers *Endormi* ou d'aller vers *En Approche*
/// pour s'approcher en ligne droite vers la position du joueur.
/// 
/// Implémentation future: possibilité d'aller vers les states AttaqueAveugle, EnPause
/// </summary>
public class ÀDistanceAIState : AIState
{
    public ÀDistanceAIState(ControleurAI controleurAI) : base(controleurAI) { }

    public override void Initialiser()
    {
        Message("État: À Distance");
        controleur.ActualiserMouvement(Vector2.zero);
    }
    public override void Actualiser()
    {
        // Si l'agent se trouve trop loin du joueur, sa state retourne à Endormi
        if (!controleur.EstJoueurÀDistance())
        {
            controleur.État = controleur.EndormiAIState;
            return;
        }

        // Implémentation temporaire pour permettre à la state de pivoter vers plusieurs autres.
        int random = UnityEngine.Random.Range(1, 1);

        switch (random)
        {
            case 1:
                Message("Choix 1: Décision -> EnApproche");
                controleur.État = controleur.EnApprocheAIState;
                return;
            default:
                return;
        }
    }
}

/// <summary>
/// Cette state est associée aux ennemis qui ne sont pas encore suffisament près du joueur pour directement se diriger
/// vers sa position. Dans cette state les ennemis se déplacent en ligne droite dans la direction du joueur. Cette state
/// peut retourner à *À Distance* si le joueur s'éloigne trop ou à l'inverse aller vers la state *À Proximité* si l'agent
/// s'est suffisament approché.
/// 
/// Implémentation future: State pourra mené vers AttaqueAveugle aussi.
/// </summary>
public class EnApprocheAIState : AIState
{
    public EnApprocheAIState(ControleurAI controleurAI) : base(controleurAI) { }

    public override void Initialiser() => Message("État: En Approche");
    public override void Actualiser()
    {
        if (!controleur.EstJoueurÀDistance()) controleur.État = controleur.ÀDistanceAIState;
        else if (controleur.EstJoueurÀProximité()) controleur.État = controleur.ÀProximitéAIState;
        else controleur.StartCoroutine(controleur.ActionRépétéeJusquàTemps(AvancerDevant, 0.2f));
    }
    /// <summary>
    /// Méthode d'aide qui permet de faire l'appel de la méthode ActualiserMouvement de ControleurAI
    /// </summary>
    public void AvancerDevant() => controleur.ActualiserMouvement(controleur.ObtenirDirectionHorizontale());
}

/// <summary>
/// State associé aux ennemis qui sont suffisament près pour entamer un positionnement dans le but de faire une attaque.
/// La state ne mène toutefois pas nécessairement à une attaque de la part de l'agent. La state peut retourner vers 
/// *À Distance* si le joueur s'éloigne ou encore se diriger vers d'autres states qui seront éventuellement implémentée.
/// </summary>
public class ÀProximitéAIState : AIState
{
    public ÀProximitéAIState(ControleurAI controleurAI) : base(controleurAI) { }

    public override void Initialiser() => Message("État: À Proximité");
    public override void Actualiser()
    {
        if (!controleur.EstJoueurÀDistance()) controleur.État = controleur.EndormiAIState;
        else if (!controleur.EstJoueurÀProximité()) controleur.État = controleur.ÀDistanceAIState;
        //else controleur.StartCoroutine(controleur.ActionRépétéeJusquàTemps(SePositionner, 0.1f));
        else controleur.État = controleur.EnPositionnementAIState;
    }
    public void SePositionner() => controleur.ActualiserMouvement(controleur.ObtenirDirectionPourPositionnement());
}

/// <summary>
/// State associé aux ennemis dont l'AI a pris la décision de se positionner dans le but de faire une attaque. Cette state
/// mène presque immanquablement à "En Attaque".
/// </summary>
public class EnPositionnementAIState : AIState
{
    public EnPositionnementAIState(ControleurAI controleurAI) : base(controleurAI) { }

    public override void Initialiser() => Message("État: En Positionnement");
    public override void Actualiser()
    {
        if (!controleur.EstJoueurÀDistance()) controleur.État = controleur.EndormiAIState;
        else if (!controleur.EstJoueurÀProximité()) controleur.État = controleur.ÀDistanceAIState;
        else if (controleur.EstPositionnéPourAttaque())
        {
            controleur.État = controleur.EnAttaqueAIState;
        }
        else controleur.StartCoroutine(controleur.ActionRépétéeJusquàTemps(SePositionner, 0.1f));
    }
    public void SePositionner() => controleur.ActualiserMouvement(controleur.ObtenirDirectionPourPositionnement());
}



public class EnAttaqueAIState : AIState
{
    public EnAttaqueAIState(ControleurAI controleurAI) : base(controleurAI) { }

    public override void Initialiser()
    {
        Message("État: En Attaque");
        controleur.ActualiserMouvement(Vector2.zero);
    }
    public override void Actualiser()
    {
        if (!controleur.EstPositionnéPourAttaque()) controleur.État = controleur.ÀProximitéAIState;
        else
        {
            
            controleur.CommanderAttaque();
            controleur.StartCoroutine(controleur.ActionDiférée(() => controleur.État = controleur.ÀProximitéAIState, 0.1f));
        }

    }
}
