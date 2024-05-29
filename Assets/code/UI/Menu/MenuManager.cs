using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Ajoutez cette ligne

public class MenuManager : MonoBehaviour
{
    public GameObject OptionsPanel;
    public GameObject Panel;

    public void PlayGame()
    {
        Panel.SetActive(false);
        OptionsPanel.SetActive(false);
        StartCoroutine(LoadGameScene());

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

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(0.1f); // Court délai pour s'assurer que les menus sont désactivés
        SceneManager.LoadScene("Jeu");
    }

}

