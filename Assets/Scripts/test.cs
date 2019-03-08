using System.Collections;
using System;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class test : MonoBehaviour
{
   


    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetButton("Option"))
        {
            LoadScene(GetActiveScene().name);
        }
    }
}
