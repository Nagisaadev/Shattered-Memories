using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class dijoncteur : MonoBehaviour
{
    public TextMeshProUGUI[] Nombretexts = new TextMeshProUGUI[3]; // Tableau pour stocker les trois TextMeshProUGUI
    int[] chiffres = new int[3]; // Tableau pour stocker les trois valeurs de chiffres

    void Start()
    {
        
        for (int i = 0; i < Nombretexts.Length; i++)
        {
            Nombretexts[i].text = chiffres[i].ToString();
        }
    }

    void Update()
    {


    }

    public void boutonpresshaut(int index)
    {
        if (index < 0 || index >= chiffres.Length) return; // Vérifier si l'index est valide

        chiffres[index] += 1;
        if (chiffres[index] >= 10)
        {
            chiffres[index] = 0;
        }
        Nombretexts[index].text = chiffres[index].ToString();
    }

    public void boutonpressbas(int index)
    {
        if (index < 0 || index >= chiffres.Length) return; // Vérifier si l'index est valide

        chiffres[index] -= 1;
        if (chiffres[index] < 0)
        {
            chiffres[index] = 9;
        }
        Nombretexts[index].text = chiffres[index].ToString();
    }

    // Méthode pour récupérer les valeurs des trois compteurs
    public int[] GetChiffres()
    {
        return chiffres;
    }

}
