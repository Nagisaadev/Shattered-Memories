using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class indices : MonoBehaviour
{
    public TextMeshProUGUI letexte;
    public float textspeed;
    public string text;
    public GameObject fond;
    public int nombredepassage=0;
    void Start()
    {
        fond.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void dialogue()
    {
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach(char c in text.ToCharArray())
        {
           letexte.text += c;
            yield return new WaitForSeconds(textspeed);
        }
        yield return new WaitForSeconds(5);
        {
            letexte.enabled = false;
            fond.SetActive(false);
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("joueur"))&( nombredepassage<=0))
        {
            nombredepassage += 1;
            fond.SetActive(true);
            dialogue();
           
        }
    }
}
