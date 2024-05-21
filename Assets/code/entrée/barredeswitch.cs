using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class barredeswitch : MonoBehaviour
{
    public dijoncteur compteurScript;
    public Slider slider;
    public Light2D myLight;
   
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        int[] valeurs = compteurScript.GetChiffres();
        if (valeurs[0] == 2 && valeurs[1] == 7 && valeurs[2] == 1 && slider.value >= 0.9)
        {
            Debug.Log("Les compteurs sont exactement 2, 7 et 1 !");
            

                myLight.enabled = false;
          

        }

        if (!(valeurs[0] == 2 && valeurs[1] == 7 && valeurs[2] == 1) && slider.value >= 0.9)
        {


            slider.value = 0;



        }
    }
}
