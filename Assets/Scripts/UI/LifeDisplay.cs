using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetLife(int amount)
    {
        if (amount < 0)
            amount = 0;
        gameObject.GetComponent<TextMesh>().text = "x " + amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
