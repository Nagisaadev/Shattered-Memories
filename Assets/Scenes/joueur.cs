using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Vitesse de d�placement du joueur
    private Rigidbody2D rb;
    private Vector3 localScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // R�cup�rer le composant Rigidbody2D attach� au joueur
        localScale = transform.localScale; // Sauvegarder l'�chelle initiale du joueur
    }

    void FixedUpdate()
    {
        // R�cup�rer les entr�es de l'axe horizontal et vertical
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculer le vecteur de d�placement
        Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;

        // Appliquer la force de d�placement au Rigidbody2D
        rb.velocity = movement;

        // Sym�trie du personnage si d�placement vers la gauche
        if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
        // Sym�trie du personnage si d�placement vers la droite
        else if (moveHorizontal > 0)
        {
            transform.localScale = localScale;
        }
    }
}