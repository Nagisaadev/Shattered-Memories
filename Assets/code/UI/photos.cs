using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class photos : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;

    
    public bool isInCollisionWithCadre;
    

   



    void Start()
    {
       
        
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
            rectTransform.anchoredPosition = localPoint;
        }

    }


    void Update()
    {
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("cadre"))
        {
            Debug.LogWarning("ça touche");
            isInCollisionWithCadre = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("cadre"))
        {
            Debug.LogWarning("ça touche PLUS");
            isInCollisionWithCadre = false;
        }
        
    }
}
