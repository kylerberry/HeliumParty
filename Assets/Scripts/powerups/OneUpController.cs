using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @todo more of a generic powerupcontroller
public class OneUpController : MonoBehaviour
{
    CircleLineRendererScript circle;
    BlinkAndDieScript blink;

    // Start is called before the first frame update
    void Awake()
    {
        circle = gameObject.transform.GetChild(0).GetComponent<CircleLineRendererScript>();
        blink = gameObject.GetComponent<BlinkAndDieScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRadius(float radius)
    {
        circle.xradius = radius;
        circle.yradius = radius;
    }

    public void SetTimeAvailable(float seconds)
    {
        blink.timeout = seconds;
    }
}
