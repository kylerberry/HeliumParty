using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDisplayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Set the level display text 
    public void SetLevel(int num)
    {
        gameObject.GetComponent<TextMesh>().text = "Level - " + num;
    }

    public void setVisibility(bool visible)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = visible;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
