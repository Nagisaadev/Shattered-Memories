using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;


public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Vitesse de d�placement du joueur
    private Rigidbody2D rb;
    private Vector3 localScale;
    private bool isInCollisionWithCompteur = false;
    public bool activation = false;
    public GameObject dijoncteur;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // R�cup�rer le composant Rigidbody2D attach� au joueur
        localScale = transform.localScale; // Sauvegarder l'�chelle initiale du joueur
        animator = GetComponent<Animator>();



        dijoncteur.SetActive(false);

       

    }

    void Update()
    {
        // R�cup�rer les entr�es de l'axe horizontal et vertical
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // Calculer le vecteur de d�placement
        Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;

        // Appliquer la force de d�placement au Rigidbody2D
        rb.velocity = movement;

        if (rb.velocity == movement)
        {
            animator.SetBool("isrunning",true);
        }

        if (rb.velocity == new Vector2(0f,0f))
        {
            animator.SetBool("isrunning", false);
        }

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


        if (isInCollisionWithCompteur ==true)
        {
            Debug.Log(Input.GetButtonDown("Fire1"));
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire1 pressed while in collision with Compteur");
                dijoncteur.SetActive(true);
            }
            if (Input.GetButtonDown("Cancel")) 
            {

                dijoncteur.SetActive(false);
            }
        }







    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Entered trigger with Compteur");
            isInCollisionWithCompteur = true;

        }
    }

   

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Exited trigger with Compteur");
            isInCollisionWithCompteur = false;

        }
    }







}