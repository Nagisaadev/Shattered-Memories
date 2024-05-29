using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plancuisine : MonoBehaviour
{
    public GameObject plan;
    public PlayerController playerController;
    private bool colision = false;
    private bool monsterAppeared = false;

    void Start()
    {
        plan.SetActive(false);
    }

    void Update()
    {
        if (colision)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                plan.SetActive(true);
                playerController.peutpasbouger = true;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                plan.SetActive(false);
                playerController.peutpasbouger = false;
                if (!monsterAppeared)
                {
                    StartCoroutine(StartMonsterAppearanceTimer());
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur"))
        {
            colision = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur"))
        {
            colision = false;
        }
    }

    IEnumerator StartMonsterAppearanceTimer()
    {
        yield return new WaitForSeconds(5f);
        Monstre monster = FindObjectOfType<Monstre>();
        if (monster != null)
        {
            monster.AppearInCuisine();
            monsterAppeared = true;
        }
    }
}


