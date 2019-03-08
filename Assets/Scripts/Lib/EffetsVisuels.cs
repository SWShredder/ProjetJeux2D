/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/05
 *      
 *      Les effets visuels seront centralisés dans ce script. Ce script doit sérieusement être remodelé de
 *      manière à être statique et ne pas dépendre de MonoBehaviour
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffetsVisuels : MonoBehaviour
{
    private bool EstEffetClignotant;
    private bool EstEffetFondue;
    public void Fondue(GameObject entité, float durée)
    {
        if (!EstEffetFondue) StartCoroutine(EffetFondue(entité, durée));
    }

    public void Clignoter(GameObject entité, float durée)
    {
        if (!EstEffetClignotant) StartCoroutine(EffetClignotant(entité, durée));
    }

    IEnumerator EffetFondue(GameObject entité, float durée)
    {
        var renderer = entité.GetComponent<SpriteRenderer>();
        for (float f = durée; f >= 0; f -= 0.1f)
        {
            Color couleur = renderer.material.color;
            couleur.a = f;
            renderer.material.color = couleur;
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator EffetClignotant(GameObject entité, float durée)
    {
        EstEffetClignotant = true;
        var renderer = entité.GetComponent<SpriteRenderer>();
        Color couleur = renderer.material.color;
        Color couleurOriginale = couleur;

        for (float f = durée / 2; f >= 0; f -= 0.01f)
        {
            couleur += new Color(1, 1, 1);
            renderer.material.color = couleur;
            yield return new WaitForSeconds(.01f);
        }

        for (float f = durée / 2; f >= 0; f -= 0.01f)
        {
            couleur -= new Color(1, 1, 1);
            renderer.material.color = couleur;
            yield return new WaitForSeconds(.01f);
        }
        renderer.material.color = couleurOriginale;
        EstEffetClignotant = false;
    }
}
