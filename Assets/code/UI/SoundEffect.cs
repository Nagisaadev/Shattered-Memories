using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class SoundEffect : MonoBehaviour
{
    public Button yourButton; // Le bouton auquel vous voulez ajouter l'�v�nement
    public EventReference soundEvent; // L'�v�nement FMOD � jouer

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