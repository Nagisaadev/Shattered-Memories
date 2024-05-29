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
    private float noiseTimer = 0f;
    private bool isPlayerDetected = false;
    private bool isPlayerInRange = false;
    private Vector2 noiseLocation;
    private bool isNoiseDetected = false;
    private bool hasInvestigatedNoise = false;

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
    private bool isInvestigatingNoise = false;

    private Coroutine noiseInvestigationCoroutine;

    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        apparitionCoroutine = StartCoroutine(ApparitionCoroutine());
        player = GameObject.FindGameObjectWithTag("joueur").transform;
        if (player == null)
        {
            Debug.LogError("Aucun joueur trouvé !");
        }
    }

    void OnEnable()
    {
        PlayerController.OnNoiseMade += OnNoiseMade;
    }

    void OnDisable()
    {
        PlayerController.OnNoiseMade -= OnNoiseMade;
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
        if (isInvestigatingNoise)
        {
            if (path == null || path.Count == 0)
            {
                path = pathfinding.FindPath(transform.position, noiseLocation);
                targetIndex = 0;
            }
            else
            {
                FollowPath();
            }
        }
        else
        {
            Patrol();
        }
    }

    void OnNoiseMade(Vector2 noisePosition)
    {
        string salleMonstre = DeterminerSalleMonstre();
        string salleJoueur = player.GetComponent<DetectionSalle>().salleActuelle;

        if (salleMonstre == salleJoueur)
        {
            // Arrêtez la coroutine d'investigation du bruit si elle est déjà en cours
            if (noiseInvestigationCoroutine != null)
            {
                StopCoroutine(noiseInvestigationCoroutine);
            }

            noiseLocation = noisePosition;
            isNoiseDetected = true;
            hasInvestigatedNoise = false;
            isInvestigatingNoise = true; // Le monstre enquête sur le bruit
            targetIndex = 0;
            path = null;

            // Commencez à suivre le bruit
            StartCoroutine(FollowNoise());

            // Indiquez au monstre d'entendre le bruit
            HearNoise(noisePosition);
        }
        else
        {
            Debug.Log("Monstre n'entend pas le bruit car il est dans une autre pièce.");
        }
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
        if (path == null || path.Count == 0) return;

        Node targetNode = path[targetIndex];
        Vector2 targetPosition = targetNode.worldPosition;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetIndex++;
            if (targetIndex >= path.Count)
            {
                isInvestigatingNoise = false; // Arrêter d'enquêter lorsque le monstre atteint la position du bruit
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
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, nextPatrolPoint.position, step);

        if (Vector2.Distance(transform.position, nextPatrolPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }

    void HearNoise(Vector2 noisePosition)
    {
        // Move towards the noise position
        path = pathfinding.FindPath(transform.position, noisePosition);
        hasInvestigatedNoise = true;
        targetIndex = 0;
    }

    string DeterminerSalleActuelle()
    {
        var detectionSalle = GetComponent<DetectionSalle>();
        if (detectionSalle == null)
        {
            Debug.LogError("No DetectionSalle component found!");
            return string.Empty;
        }
        return detectionSalle.salleActuelle;
    }

    string DeterminerSalleMonstre()
    {
        var detectionSalle = GetComponent<DetectionSalle>();
        if (detectionSalle == null)
        {
            Debug.LogError("No DetectionSalle component found!");
            return string.Empty;
        }
        return detectionSalle.salleActuelle;
    }

    IEnumerator FollowNoise()
    {
        // Trouver un chemin vers la position du bruit
        path = pathfinding.FindPath(transform.position, noiseLocation);

        // Attendre une petite période de temps pour donner une chance au monstre de se déplacer vers la position du bruit
        yield return new WaitForSeconds(0.5f);

        // Suivre le chemin vers la position du bruit
        while (targetIndex < path.Count)
        {
            Node targetNode = path[targetIndex];
            Vector2 targetPosition = targetNode.worldPosition;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
            }
            yield return null;
        }

        // Arrêter d'enquêter lorsque le monstre atteint la position du bruit
        isInvestigatingNoise = false;
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
        Destroy(player.gameObject);
        Debug.Log("Player has been killed!");
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
}




