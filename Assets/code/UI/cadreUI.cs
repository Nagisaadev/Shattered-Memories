using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cadreUI : MonoBehaviour
{
    public photos Photos1;
    public photos Photos2;
    public photos Photos3;
    public GameObject grandephoto1;
    public GameObject grandephoto2;
    public GameObject grandephoto3;

   

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      if(Photos1.isInCollisionWithCadre&&!Photos1.isDragging)
        {
            Destroy(grandephoto1,0.1f);
            animator.SetBool("mom", true);
        }
        
        if (Photos2.isInCollisionWithCadre && !Photos2.isDragging)
        {
            Destroy(grandephoto2,0.1f);
            animator.SetBool("girl", true);
        }
        if (Photos3.isInCollisionWithCadre && !Photos3.isDragging)
        {
            Destroy(grandephoto3, 0.1f);
            animator.SetBool("boy", true);
        }
    }
}
