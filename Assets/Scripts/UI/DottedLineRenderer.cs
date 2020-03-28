using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLineRenderer : MonoBehaviour
{
    private LineRenderer lR;
    public List<Vector2> positions;
    private Renderer rend;
    // Use this for initialization
    void Start()
    {
        lR = GetComponent<LineRenderer>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        for (int p = 0; p < positions.Count; p++)
        {
            lR.SetPosition(p, positions[p]); //sets the 
        }
        rend.material.mainTextureScale = new Vector2((int)Vector2.Distance(positions[0], positions[1]), 1);
    }
}
