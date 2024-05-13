using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barredeflashlght : MonoBehaviour
{
    public LightController lightController;
    public Slider slider;
    public float maxTime = 10f;
    private float currentTime;
    private float timeSinceOff = 0f;

    void Start()
    {

        currentTime = maxTime;
    }

    void Update()
    {
        Debug.Log(lightController.isLightOn);
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
        else
        {
            currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
            timeSinceOff += Time.deltaTime;
            if (timeSinceOff >= 3f)
            {
                currentTime += Time.deltaTime;
                slider.value = currentTime / maxTime;
            }
        }
    }
}

