using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plancuisine : MonoBehaviour
{
    public GameObject plan;
    public PlayerController playerController;
    private bool colision=false;
    void Start()
    {
        plan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (colision == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                plan.SetActive(true);
                playerController.peutpasbouger = true;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                plan.SetActive(false);
                playerController.peutpasbouger = false;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur") )
        {
            colision = true;
            
        }
      
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur"))
        {
            colision = false;
        }
    }
}
