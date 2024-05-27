using UnityEngine;

public class DetectionSalle : MonoBehaviour
{
    public string salleActuelle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered trigger with: " + other.name); // Ajoutez cette ligne pour d�boguer
        if (other.CompareTag("joueur"))
        {
            salleActuelle = gameObject.tag; // Mettre � jour la salle actuelle lorsque le joueur entre en collision avec le collider de la salle
            Debug.Log("Salle actuelle: " + salleActuelle); // Ajouter un log pour v�rifier la salle actuelle
        }
        if (other.CompareTag("Cuisine"))
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
        if (other.CompareTag("joueur"))
        {
            salleActuelle = gameObject.tag; // Mettre � jour la salle actuelle lorsque le joueur entre en collision avec le collider de la salle
            Debug.Log("Salle actuelle: " + salleActuelle); // Ajouter un log pour v�rifier la salle actuelle
        }
        // Ajoutez d'autres conditions pour les autres pi�ces si n�cessaire
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited trigger with: " + other.name); // Ajoutez cette ligne pour d�boguer
        // Lorsque le joueur sort de la zone de collision, r�initialisez la salle actuelle
        salleActuelle = "";
    }
}

