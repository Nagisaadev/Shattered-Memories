using UnityEngine;

public class DetectionSalle : MonoBehaviour
{
    public string salleActuelle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered trigger with: " + other.name); // Ajoutez cette ligne pour déboguer
        if (other.CompareTag("joueur"))
        {
            salleActuelle = gameObject.tag; // Mettre à jour la salle actuelle lorsque le joueur entre en collision avec le collider de la salle
            Debug.Log("Salle actuelle: " + salleActuelle); // Ajouter un log pour vérifier la salle actuelle
        }
        if (other.CompareTag("Cuisine"))
        {
            salleActuelle = "Cuisine";
        }
        else if (other.CompareTag("SalleAManger"))
        {
            salleActuelle = "Salle à manger";
        }
        else if (other.CompareTag("Garage"))
        {
            salleActuelle = "Garage";
        }
        if (other.CompareTag("joueur"))
        {
            salleActuelle = gameObject.tag; // Mettre à jour la salle actuelle lorsque le joueur entre en collision avec le collider de la salle
            Debug.Log("Salle actuelle: " + salleActuelle); // Ajouter un log pour vérifier la salle actuelle
        }
        // Ajoutez d'autres conditions pour les autres pièces si nécessaire
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited trigger with: " + other.name); // Ajoutez cette ligne pour déboguer
        // Lorsque le joueur sort de la zone de collision, réinitialisez la salle actuelle
        salleActuelle = "";
    }
}

