using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levier : MonoBehaviour
{
    public Slider[] sliders; // Tableau pour stocker les sliders
    private int[] correctOrder = { 0, 1, 2 }; // L'ordre correct d'interaction
    private List<int> userInput = new List<int>(); // Liste pour stocker l'ordre des interactions de l'utilisateur
    public PlayerController playerController;
    public GameObject fond;
    public GameObject[] slidersobj;
    public Teleportation teleportation;
    void Start()
    {
        fond.SetActive(false);
        slidersobj[0].SetActive(false);
        slidersobj[1].SetActive(false);
        slidersobj[2].SetActive(false);





        // Assigner des méthodes d'écoute aux sliders
        for (int i = 0; i < sliders.Length; i++)
        {
            int index = i; // Capturer l'index pour l'utilisation dans le listener
            sliders[i].onValueChanged.AddListener(delegate { OnSliderValueChanged(index); });
        }
    }

    // Méthode appelée quand la valeur d'un slider change
    void OnSliderValueChanged(int index)
    {
        if (sliders[index].value == 1) // Considérer une interaction lorsque la valeur est à 1
        {
            // Ajouter l'index du slider manipulé à la liste des interactions utilisateur
            userInput.Add(index);

            // Vérifier si l'ordre des interactions est correct
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
                    teleportation.peutTP = true;
                }
                else
                {
                    Debug.Log("Ordre incorrect.");

                    // Remettre les sliders à zéro si l'ordre est incorrect
                    StartCoroutine(ResetSlidersWithDelay());
                }

                // Réinitialiser la liste des interactions utilisateur
                userInput.Clear();
            }
        }
    }

    // Coroutine pour réinitialiser les sliders à zéro avec un léger délai
    IEnumerator ResetSlidersWithDelay()
    {
        yield return new WaitForSeconds(0.6f); // Attendre la fin de la frame courante
        foreach (Slider slider in sliders)
        {
            slider.value = 0;
        }
    }




    void Update()
    {
       

        if (playerController.isInCollisionWithInterupteur1 == true)
        {
            
            if (Input.GetButtonDown("Fire1"))
            {

                slidersobj[0].SetActive(true);
                fond.SetActive(true);
                playerController.peutpasbouger = true;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                slidersobj[0].SetActive(false);
                fond.SetActive(false);
                playerController.peutpasbouger = false;
            }
        }

        if (playerController.isInCollisionWithInterupteur2 == true)
        {

            if (Input.GetButtonDown("Fire1"))
            {

                slidersobj[1].SetActive(true);
                fond.SetActive(true);
                playerController.peutpasbouger = true;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                slidersobj[1].SetActive(false);
                fond.SetActive(false);
                playerController.peutpasbouger = false;
            }
        }


        if (playerController.isInCollisionWithInterupteur3 == true)
        {

            if (Input.GetButtonDown("Fire1"))
            {

                slidersobj[2].SetActive(true);
                fond.SetActive(true);
                playerController.peutpasbouger = true;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                slidersobj[2].SetActive(false);
                fond.SetActive(false);
                playerController.peutpasbouger = false;
            }
        }


    }
}
