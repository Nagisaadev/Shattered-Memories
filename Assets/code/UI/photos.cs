using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class photos : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    public GameObject cadre;
    public boutdephoto boutdephoto;
    private bool isInCollisionWithCadre;
    public GameObject grandephoto1;
    public GameObject grandephoto2;
    public GameObject grandephoto3;

    private bool mom = false;
    private bool boy = false;
    private bool girl = false;



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
            
            isInCollisionWithCadre = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("cadre"))
        {
           
            isInCollisionWithCadre = false;
        }
        
    }
}
