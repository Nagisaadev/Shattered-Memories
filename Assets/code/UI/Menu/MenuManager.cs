using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Ajoutez cette ligne

public class MenuManager : MonoBehaviour
{
    public GameObject OptionsPanel;
    public GameObject Panel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Jeu");

    }

    public void OpenOptions()
    {
        Panel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        Panel.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();


    }


}