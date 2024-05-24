using UnityEngine;

public class Monstre : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;

    private bool isPlayerInRange = false;

    void Update()
    {
        DetectPlayer();

        if (isPlayerInRange)
        {
            ChasePlayer();
        }
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
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
        // Code to handle player death
        // For example, disabling the player object
        Destroy(player.gameObject);
        Debug.Log("Player has been killed!");
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}