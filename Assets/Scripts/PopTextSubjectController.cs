using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopTextSubjectController : MonoBehaviour
{
    TextMeshPro text;
    public List<string> exclamations;
    
    void Start()
    {
        exclamations.Add("Pop!");
        exclamations.Add("Bang!");
        exclamations.Add("Boom!");
        exclamations.Add("Pow!");
        text = gameObject.GetComponent<TextMeshPro>();
        InvokeRepeating("RandomizeText", 2.0f, 2.0f);
    }

    void RandomizeText()
    {
        int i = Random.Range(0, exclamations.Count);
        text.SetText(exclamations[i]); 
    }
}
