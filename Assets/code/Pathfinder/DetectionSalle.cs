using System.Collections;
using UnityEngine;

public class DetectionSalle : MonoBehaviour
{
    public string salleActuelle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("joueur"))
        {
            salleActuelle = gameObject.tag; // Mettre � jour la salle actuelle lorsque le joueur entre en collision avec le collider de la salle
            other.GetComponent<PlayerController>().currentRoom = salleActuelle; // Mettre � jour la salle actuelle dans PlayerController
        }
        else if (other.CompareTag("Cuisine"))
        {
            salleActuelle = "Cuisine";
        }
        else if (other.CompareTag("SalleAManger"))
        {
            salleActuelle = "Salle � manger";
        }
        else if (other.CompareTag("Garage"))
        {
            salleActuelle = "Garage";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("joueur"))
        {
            salleActuelle = ""; // R�initialiser la salle actuelle lorsque le joueur quitte la zone de collision
            other.GetComponent<PlayerController>().currentRoom = salleActuelle; // R�initialiser la salle actuelle dans PlayerController
        }
    }
}