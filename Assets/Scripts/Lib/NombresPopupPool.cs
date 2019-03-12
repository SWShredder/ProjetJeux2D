using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NombresPopupPool : MonoBehaviour
{
    [SerializeField]
    private int largeurPool;
    [SerializeField]
    private GameObject nombrePopup;

    public static NombresPopupPool Instance;
    public List<GameObject> nombrePopups;
    void Start()
    {
        nombrePopups = GénérerPool(largeurPool);
    }

    // Update is called once per frame
    void Awake()
    {
        Instance = this;
    }

    private List<GameObject> GénérerPool(int largeur)
    {
        var nouvelleListe = new List<GameObject>(largeur);
        for (int i = 0; i < largeur; i++)
        {
            var obj = Instantiate(nombrePopup) as GameObject;
            obj.SetActive(false);
            obj.transform.SetParent(this.gameObject.transform);
            nouvelleListe.Add(obj);
        }
        return nouvelleListe;
    }

    public GameObject ObtenirNombrePopup(int nombre)
    {
        for (int i = 0; i < nombrePopups.Count; i++)
        {
            if (!nombrePopups[i].activeInHierarchy)
            {
                nombrePopups[i].SetActive(true);
                nombrePopups[i].GetComponentInChildren<NombrePopup>().RéglerNombre(nombre);
                return nombrePopups[i];
            }
        }
        return null;
    }

    public void RetournerNombrePopup(GameObject source)
    {
        source.transform.SetParent(this.gameObject.transform);
        source.SetActive(false);
    }
}
