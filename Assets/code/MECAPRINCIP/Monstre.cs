using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monstre : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;
    public List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;

    private bool isPlayerDetected = false;
    private bool isPlayerInRange = false;
    private Vector2 noiseLocation;
    private bool isNoiseDetected = false;
    private bool hasInvestigatedNoise = false;
    private float noiseDetectionTime = 5f;
    private float noiseTimer = 0f;
    private float noiseInvestigationStartTime;
    private float noiseInvestigationTime = 5f;
    private float noiseInvestigationTimer = 0f;
    private bool hasHeardNoise = false;
    private Vector2 lastHeardNoisePosition;

    private Pathfinding pathfinding;
    private List<Node> path;
    private int targetIndex;

    private bool isAppeared = false;
    public List<Transform> patrolPointsCuisine;
    public List<Transform> patrolPointsSalleAManger;
    public List<Transform> patrolPointsGarage;

    public Vector2 porteCuisine;
    public Vector2 porteSalleAManger;
    public Vector2 porteGarage;
    public Vector2 posSalon;

    private Coroutine apparitionCoroutine;
    private bool hasAppearedInCuisine = false;

    public List<Transform> patrolPointsSalon; // Liste des points de patrouille pour le salon
    private bool hasAppearedInSalon = false;
    private Animator animator;
    private Coroutine noiseInvestigationCoroutine;
    public bool iskilled=false;
    public PlayerController playerController;

   public float life=3;
    public float dureeDescendre = 3.0f; // Durée de la transition de l'opacité
    private SpriteRenderer spriteRenderer;
    public float invincibilityDuration = 4.0f;
    private bool isInvincible = false;

    public InterrupteurCollision1 InterrupteurCollision1;
    public InterrupteurCollision1 InterrupteurCollision2;
    public InterrupteurCollision1 InterrupteurCollision3;
    public InterrupteurCollision1 InterrupteurCollision4;

    public LightController flashlight;
    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        apparitionCoroutine = StartCoroutine(ApparitionCoroutine());
        player = GameObject.FindGameObjectWithTag("joueur").transform;
        if (player == null)
        {
            Debug.LogError("Aucun joueur trouvé !");
        }
        boutdephoto.PhotoCollectedEvent += OnPhotoCollected;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        PlayerController.OnObjectDropped += OnObjectDropped;
    }

    void OnDisable()
    {
        PlayerController.OnObjectDropped -= OnObjectDropped;
    }

    void Update()
    {
        string salleActuelle = DeterminerSalleActuelle();

        switch (salleActuelle)
        {
            case "Cuisine":
                patrolPoints = patrolPointsCuisine;
                break;
            case "Salle à manger":
                patrolPoints = patrolPointsSalleAManger;
                break;
            case "Garage":
                patrolPoints = patrolPointsGarage;
                break;
            case "Salon":
                patrolPoints = patrolPointsSalon; // Utiliser les points de patrouille du salon
                break;
        }

        DetectPlayer();

        if (isPlayerInRange)
        {
            hasInvestigatedNoise = false;
            path = pathfinding.FindPath(transform.position, player.position);

            if (path != null && path.Count > 0)
            {
                Debug.Log("Player detected. Following path to player.");
                FollowPath();
            }
            else
            {
                Debug.LogWarning("Player detected, but no valid path found!");
            }
        }

        else
        {
            Patrol();
        }

        if (life<=0f)
        {
            StartCoroutine(DescendreOpaciteCoroutine());
        }

        if (flashlight.isLightOn&&!iskilled)
        {
            spriteRenderer.enabled = true;
        }
        if (!flashlight.isLightOn && !iskilled)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }
    void OnPhotoCollected()
    {
        // Apparition du monstre une fois que la photo est ramassée
        TeleportToGarage();
    }
    string DeterminerSalleActuelle()
    {
        return GetComponent<DetectionSalle>().salleActuelle;
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            isPlayerInRange = true;
            isNoiseDetected = false;
            isPlayerDetected = true;
        }
        else
        {
            isPlayerInRange = false;
            isPlayerDetected = false;
        }
    }

    void FollowPath()
    {
        if (isPlayerDetected)
        {
            Vector3 playerPosition = player.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        }
        else 
        {
            
            if (targetIndex < path.Count)
            {
                // Récupère le nœud cible du chemin
                Node targetNode = path[targetIndex];
                Vector2 targetPosition = targetNode.worldPosition;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
                    // Si oui, passe à la cible suivante dans le chemin
                    targetIndex++;
                }
            }
        }
    }


    void Patrol()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points assigned!");
            return;
        }

        Transform nextPatrolPoint = patrolPoints[currentPatrolIndex];

        // Calcule le déplacement de l'objet vers le prochain point de patrouille
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, nextPatrolPoint.position, step);

        // Vérifie si l'objet est suffisamment proche du prochain point de patrouille
        if (Vector2.Distance(transform.position, nextPatrolPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }


    void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points assigned!");
            return;
        }

        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, patrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentPatrolIndex = closestIndex;
        Patrol();
    }

    void OnObjectDropped(Vector2 dropLocation)
    {
        string salleMonstre = DeterminerSalleMonstre();
        string salleJoueur = player.GetComponent<DetectionSalle>().salleActuelle;

        if (salleMonstre == salleJoueur)
        {

            noiseTimer = 0f;
            targetIndex = 0;
            path = null;

            // Logique pour le bruit spécifique du Buste
            HandleNoise(dropLocation);
        }
        else
        {
            Debug.Log("Monstre n'entend pas le bruit car il est dans une autre pièce.");
        }
    }

    public void HandleNoise(Vector2 noisePosition)
    {
        // Supposons que le bruit du Buste a été créé
        Debug.Log("Noise detected at: " + noisePosition);
        isNoiseDetected = true;
        noiseLocation = noisePosition;
        hasInvestigatedNoise = false;
        noiseInvestigationStartTime = Time.time;
        lastHeardNoisePosition = noisePosition;
    }



    string DeterminerSalleMonstre()
    {
        return GetComponent<DetectionSalle>().salleActuelle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("joueur"))
        {
            KillPlayer();

        }
    }

    void KillPlayer()
    {
        
        Debug.Log("Player has been killed!");

        ///ANIMATION
        iskilled= true;
        playerController.peutpasbouger = true;
        animator.SetBool("toucher",true);
        StartCoroutine(StartTimer());
    }



    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(3);
        iskilled = false;
        player.position = new Vector2(-0.25f, -0.56f);
        animator.SetBool("toucher", false);
        playerController.peutpasbouger =false;
    }




    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    IEnumerator ApparitionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            if (!hasAppearedInSalon && DeterminerSalleMonstre() != player.GetComponent<DetectionSalle>().salleActuelle)
            {
                TeleportToPlayerRoom();
                hasAppearedInSalon = true;
            }
        }
    }

    void TeleportToPlayerRoom()
    {
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        string salleActuelle = player.GetComponent<DetectionSalle>().salleActuelle;

        if (string.IsNullOrEmpty(salleActuelle))
        {
            Debug.LogError("Player's current room not found!");
            return;
        }

        Vector2 targetPosition = Vector2.zero;

        switch (salleActuelle)
        {
            case "Cuisine":
                targetPosition = porteCuisine;
                break;
            case "Salle à manger":
                targetPosition = porteSalleAManger;
                break;
            case "Garage":
                targetPosition = porteGarage;
                break;
            case "Salon":
                targetPosition = posSalon;
                break;
            default:
                Debug.LogWarning("Salle actuelle non reconnue: " + salleActuelle);
                break;
        }

        transform.position = targetPosition;
        Debug.Log("Le monstre est apparu à la porte de la salle: " + salleActuelle + " à la position: " + targetPosition);
    }

    public void AppearInCuisine()
    {
        if (!hasAppearedInCuisine)
        {
            hasAppearedInCuisine = true;
            transform.position = porteCuisine;
            Debug.Log("Le monstre est apparu à la porte de la cuisine.");
        }
    }

    public void TeleportToSalon()
    {
        transform.position = posSalon;
        Debug.Log("Le monstre a été téléporté au salon à la position: " + posSalon);
    }
    public void TeleportToGarage()
    {
        transform.position = porteGarage;
        Debug.Log("Le monstre a été téléporté au salon à la position: " + posSalon);
    }





    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("light1") && !isInvincible&& !InterrupteurCollision1.allumer)
        {
            StartCoroutine(TakeDamage());
        }
        if (other.CompareTag("light2") && !isInvincible && !InterrupteurCollision2.allumer)
        {
            StartCoroutine(TakeDamage());
        }
        if (other.CompareTag("light3") && !isInvincible && !InterrupteurCollision3.allumer)
        {
            StartCoroutine(TakeDamage());
        }
        if (other.CompareTag("light4") && !isInvincible && !InterrupteurCollision4.allumer)
        {
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        life -= 1;
        isInvincible = true;

        // Attendre la durée d'invincibilité
        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }


    IEnumerator DescendreOpaciteCoroutine()
    {
        // Opacité initiale
        float opaciteInitiale = spriteRenderer.color.a;
        // Temps écoulé
        float tempsEcoule = 0f;

        while (tempsEcoule < dureeDescendre)
        {
            // Calculer le ratio de progression
            float ratio = tempsEcoule / dureeDescendre;
            // Calculer la nouvelle opacité en fonction du ratio
            float nouvelleOpacite = Mathf.Lerp(opaciteInitiale, 0f, ratio);
            // Créer une nouvelle couleur avec la nouvelle opacité
            Color nouvelleCouleur = spriteRenderer.color;
            nouvelleCouleur.a = nouvelleOpacite;
            // Appliquer la nouvelle couleur au sprite
            spriteRenderer.color = nouvelleCouleur;

            // Attendre un frame
            yield return null;
            // Mettre à jour le temps écoulé
            tempsEcoule += Time.deltaTime;
        }

        // Une fois la transition terminée, désactiver le GameObject
        gameObject.SetActive(false);
    }
}




