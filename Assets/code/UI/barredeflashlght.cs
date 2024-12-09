using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barredeflashlght : MonoBehaviour
{
    public LightController lightController;
    public Slider slider;
    public float maxTime = 15f;
    private float currentTime;
    private float timeSinceOff = 0f;
    private bool surchaufe = false;

    void Start()
    {

        currentTime = maxTime;
    }

    void Update()
    {
        
        if (lightController.isLightOn)
        {
            timeSinceOff = 0f;
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime;
                slider.value = currentTime / maxTime;
            }
            else
            {
                currentTime = 0f;
            }
        }
       
       
        else if (surchaufe == false)
        {
            currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
            timeSinceOff += Time.deltaTime;
            if (timeSinceOff >= 1.5f)
            {
                currentTime += Time.deltaTime;
                slider.value = currentTime / maxTime;
            }
        }

        if (slider.value <=0f)
        {
            surchaufe = true;
           
        }
        if (surchaufe == true)
       
        {
            lightController.isLightOn = false;
            currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
            timeSinceOff += Time.deltaTime;
            if (timeSinceOff >= 0.5f)
            {
                currentTime += Time.deltaTime;
                slider.value = currentTime / maxTime;
                surchaufe = false;
            }
            
        }


    }

}

