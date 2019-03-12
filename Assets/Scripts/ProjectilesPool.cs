using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesPool : MonoBehaviour
{
    public enum Type { Projectile, Omnishot };

    public static ProjectilesPool Instance;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private int largeurPoolProjectiles;
    [SerializeField]
    private GameObject omnishot;
    [SerializeField]
    private int largeurPoolOmnishots;

    public List<GameObject> projectiles;
    public List<GameObject> omnishots;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        projectiles = PopulerPool(projectile, largeurPoolProjectiles);
        omnishots = PopulerPool(omnishot, largeurPoolOmnishots);
    }

    private List<GameObject> PopulerPool(GameObject source, int largeurPool)
    {
        var list = new List<GameObject>(largeurPool);
        for (int i = 0; i < largeurPool; i++)
        {
            GameObject p = Instantiate(source) as GameObject;
            p.SetActive(false);
            list.Add(p);           
        }
        return list;
    }

    public GameObject ObtenirProjectile()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                projectiles[i].SetActive(true);
                return projectiles[i];
            }
        }
        return null;
    }

    public GameObject ObtenirProjectile(Vector3 position, Quaternion rotation)
    {
        var p = ObtenirProjectile();
        p.transform.position = position;
        p.transform.rotation = rotation;
        return p;
    }

    public void RetournerProjectile(GameObject projectileRetourné)
    {
        projectileRetourné.SetActive(false);
    }
}
