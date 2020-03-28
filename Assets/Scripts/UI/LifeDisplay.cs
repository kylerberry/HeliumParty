using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetLife(int amount)
    {
        amount -= 1;
        if (amount <= 0)
            amount = 0;
        gameObject.GetComponent<TextMeshPro>().text = "x " + amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
