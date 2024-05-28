using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public PlayerController playerController;
    private bool isInCollisionWithPoinA;
    private bool isInCollisionWithPoinB;
    public bool peutTP=true;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (isInCollisionWithPoinA == true& peutTP)
        {
           
            if (Input.GetButtonDown("Fire1"))
            {
                playerController.transform.position = pointB.transform.position;
            }
           
        }
    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur"))
        {
            isInCollisionWithPoinA = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur"))
        {
            isInCollisionWithPoinA = false;
        }
       
    }











}


