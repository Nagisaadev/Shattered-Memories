using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levier : MonoBehaviour
{
    public Slider[] sliders; // Tableau pour stocker les sliders
    private int[] correctOrder = { 0, 1, 2 }; // L'ordre correct d'interaction
    private List<int> userInput = new List<int>(); // Liste pour stocker l'ordre des interactions de l'utilisateur

    void Start()
    {
        // Assigner des m�thodes d'�coute aux sliders
        for (int i = 0; i < sliders.Length; i++)
        {
            int index = i; // Capturer l'index pour l'utilisation dans le listener
            sliders[i].onValueChanged.AddListener(delegate { OnSliderValueChanged(index); });
        }
    }

    // M�thode appel�e quand la valeur d'un slider change
    void OnSliderValueChanged(int index)
    {
        if (sliders[index].value == 1) // Consid�rer une interaction lorsque la valeur est � 1
        {
            // Ajouter l'index du slider manipul� � la liste des interactions utilisateur
            userInput.Add(index);

            // V�rifier si l'ordre des interactions est correct
            if (userInput.Count == correctOrder.Length)
            {
                bool isCorrect = true;
                for (int i = 0; i < correctOrder.Length; i++)
                {
                    if (userInput[i] != correctOrder[i])
                    {
                        isCorrect = false;
                        break;
                    }
                }

                // Si l'ordre est correct, afficher un message
                if (isCorrect)
                {
                    Debug.Log("L'objet est ouvert !");
                }
                else
                {
                    Debug.Log("Ordre incorrect.");

                    // Remettre les sliders � z�ro si l'ordre est incorrect
                    StartCoroutine(ResetSlidersWithDelay());
                }

                // R�initialiser la liste des interactions utilisateur
                userInput.Clear();
            }
        }
    }

    // Coroutine pour r�initialiser les sliders � z�ro avec un l�ger d�lai
    IEnumerator ResetSlidersWithDelay()
    {
        yield return new WaitForSeconds(0.6f); // Attendre la fin de la frame courante
        foreach (Slider slider in sliders)
        {
            slider.value = 0;
        }
    }
}
