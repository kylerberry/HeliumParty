using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillerFeedbackScript : MonoBehaviour
{
    private float heightScale = 0.0f;
    private float step = 0.05f;
    // fudged this by dragging the scale until it was at the top of the screen
    // not sure if this is robust
    private float maxScale = 10.0f;
    private float heightChange = 0.0f;
    void Start()
    {
    }

/*    private void Update()
    {
        Vector3 scale = transform.localScale;
        if (scale.y >= heightScale)
        {
            scale.y = heightScale;
            transform.localScale = scale;
            return;
        }

        // animate the height change by `step`
        scale.y += step;
        transform.localScale = scale;
    }*/

    public void SetHeightScale(float percent)
    {
        Debug.Log(percent);
        heightScale = percent * maxScale;
        Vector3 scale = transform.localScale;
        scale.y = heightScale;
        transform.localScale = scale;
        /* heightChange = heightScale - transform.localScale.y;*/
    }
}
