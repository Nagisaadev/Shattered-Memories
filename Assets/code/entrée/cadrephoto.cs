using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cadrephoto : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject fond;
    public GameObject grandephoto1;
    public GameObject grandephoto2;
    public GameObject grandephoto3;
    public boutdephoto boutdephoto1;
    public boutdephoto boutdephoto2;
    public boutdephoto boutdephoto3;
    
    void Start()
    {
        fond.SetActive(false);
        grandephoto1.SetActive(false);
        grandephoto2.SetActive(false);
        grandephoto3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isInCollisionWithCadrePetit == true)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                fond.SetActive(true);
                playerController.peutpasbouger = true;

                if(boutdephoto1.photo==true)
                {
                    grandephoto1.SetActive(true);
                }
                if (boutdephoto2.photo == true)
                {
                    grandephoto2.SetActive(true);
                }
                if (boutdephoto3.photo == true)
                {
                    grandephoto3.SetActive(true);
                }
            }
            if (Input.GetButtonDown("Cancel"))
            {
                fond.SetActive(false);
                playerController.peutpasbouger = false;
                grandephoto1.SetActive(false);
                grandephoto2.SetActive(false);
                grandephoto3.SetActive(false);
            }
        }
    }
}
