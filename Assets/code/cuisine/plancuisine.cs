using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plancuisine : MonoBehaviour
{
    public GameObject plan;
    public GameObject player;
    private bool colision=false;
    void Start()
    {
        plan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")&& colision == true)
        {
            plan.SetActive(true);
        }
        if (colision == false)
        {
            plan.SetActive(false);
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
