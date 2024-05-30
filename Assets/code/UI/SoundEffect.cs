using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class SoundEffect : MonoBehaviour
{
    public Button yourButton; // Le bouton auquel vous voulez ajouter l'événement
    public EventReference soundEvent; // L'événement FMOD à jouer

    void Start()
    {
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        RuntimeManager.PlayOneShot(soundEvent);
    }
}