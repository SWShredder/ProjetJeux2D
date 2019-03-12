using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NombrePopup : MonoBehaviour
{
    [SerializeField]
    private Animator animateur;
    [SerializeField]
    private Text composanteTexte;

    void Start()
    {
        Coroutines.Instance.ActionDiférée(RetournerPopup, animateur.GetCurrentAnimatorClipInfo(0).Length);
    }

    void OnEnable()
    {
        //Destroy(gameObject, animateur.GetCurrentAnimatorClipInfo(0).Length);
        StartCoroutine(Coroutines.Instance.ActionDiférée(RetournerPopup, animateur.GetCurrentAnimatorClipInfo(0).Length));
    }
    
    public void RéglerNombre(int nombre)
    {
        composanteTexte.text = nombre.ToString();
    }

    public void RetournerPopup()
    {
        NombresPopupPool.Instance.RetournerNombrePopup(this.gameObject);
    }
}
