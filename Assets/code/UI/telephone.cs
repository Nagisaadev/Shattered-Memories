using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class telephone : MonoBehaviour
{
    private Animator animator;
    
    private SpriteRenderer spriteRenderer;
    public GameObject tel;
    public stopplayer stopplayer;
    public Image imageToFade;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopplayer.ordre==1)
        {
            animator.SetBool("eldebut", true);
        }
        if (stopplayer.ordre == 2)
        {
            animator.SetBool("terminado", true);
            StartCoroutine(timer());
        }
        
    }


    IEnumerator timer()
    {
        yield return new WaitForSeconds(2);
       
        gameObject.SetActive(false);
    }






}
