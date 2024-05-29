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

    private Coroutine noiseInvestigationCoroutine;

    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        apparitionCoroutine = StartCoroutine(ApparitionCoroutine());
        player = GameObject.FindGameObjectWithTag("joueur").transform;
        if (player == null)
        {
            Debug.LogError("Aucun joueur trouv� !");
        }
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
            case "Salle � manger":
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
        else if (isNoiseDetected && !hasInvestigatedNoise)
        {
            if (noiseInvestigationCoroutine == null)
            {
                noiseInvestigationCoroutine = StartCoroutine(InvestigateNoise());
            }
        }
        else if (hasInvestigatedNoise)
        {
            // Traiter l'investigation du bruit
        }
        else
        {
            Patrol();
        }
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
        else // Sinon, suivre le chemin normalement
        {
            if (targetIndex < path.Count)
            {
                Node targetNode = path[targetIndex];
                Vector2 targetPosition = targetNode.worldPosition;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
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
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, nextPatrolPoint.position, step);

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
            noiseLocation = dropLocation;
            isNoiseDetected = true;
            hasInvestigatedNoise = false;
            targetIndex = 0;
            path = null;

            // Logique pour le bruit sp�cifique du Buste
            HandleNoise(dropLocation);
        }
        else
        {
            Debug.Log("Monstre n'entend pas le bruit car il est dans une autre pi�ce.");
        }
    }

    public void HandleNoise(Vector2 noisePosition)
    {
        Debug.Log("Noise detected at: " + noisePosition);
        isNoiseDetected = true;
        noiseLocation = noisePosition;
        hasInvestigatedNoise = false;
        // Si une investigation de bruit est d�j� en cours, arr�tez-la
        if (noiseInvestigationCoroutine != null)
        {
            StopCoroutine(noiseInvestigationCoroutine);
        }
        // Lancez une nouvelle investigation de bruit
        noiseInvestigationCoroutine = StartCoroutine(InvestigateNoise());
    }

    IEnumerator InvestigateNoise()
    {
        if (isNoiseDetected)
        {
            path = pathfinding.FindPath(transform.position, noiseLocation);
            if (path != null && path.Count > 0)
            {
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

                Debug.Log("Monster reached the noise location. Waiting for 5 seconds.");
                yield return new WaitForSeconds(5f);
            }
            else
            {
                Debug.LogWarning("Noise detected, but no valid path found!");
            }

            isNoiseDetected = false;
            hasInvestigatedNoise = true;
            noiseInvestigationCoroutine = null;
            Debug.Log("Monster finished investigating the noise. Resuming patrol.");
        }
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
            case "Salle � manger":
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
        Debug.Log("Le monstre est apparu � la porte de la salle: " + salleActuelle + " � la position: " + targetPosition);
    }

    public void AppearInCuisine()
    {
        if (!hasAppearedInCuisine)
        {
            hasAppearedInCuisine = true;
            transform.position = porteCuisine;
            Debug.Log("Le monstre est apparu � la porte de la cuisine.");
        }
    }

    public void TeleportToSalon()
    {
        transform.position = posSalon;
        Debug.Log("Le monstre a �t� t�l�port� au salon � la position: " + posSalon);
    }
}




