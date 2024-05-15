using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiverDesactiverDijonteur : MonoBehaviour
{
    public GameObject dijoncteur;
    private PlayerController playerController;

    void Start()
    {
        dijoncteur.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
        if (playerController.activation == false)
        {
            Debug.LogError("Le script PlayerController n'a pas �t� trouv� dans la sc�ne.");
        }

    }

    void Update()
    {
        
            if (playerController.activation== true)
            {
                dijoncteur.SetActive(true);
            }
            else
            {
                dijoncteur.SetActive(false);
            }
        
    }
}
