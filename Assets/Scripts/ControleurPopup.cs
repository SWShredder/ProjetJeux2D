/*
 *      Auteur: Yanik Sweeney
 *      Date de création: 2019/03/10
 *      
 *      Attribution: Largement inspiré de GameGrind sur youtube. 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleurPopup : MonoBehaviour
{
    public static NombrePopup popup;
    void Start()
    {
        popup = Resources.Load<NombrePopup>("NombrePopupParent");
    }

    static public void GénérerNombresPopup(int nombre, GameObject source, Canvas canvas)
    {
        NombrePopup instance = Instantiate(popup);
        instance.transform.SetParent(canvas.transform, false);
        var pos = source.transform.position;
        instance.transform.position = pos;
        //instance.RéglerNombre(nombre);
    }
}
