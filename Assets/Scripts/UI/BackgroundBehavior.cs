using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    // @todo would be cool if the background could react to gaining powerups and beating levels

    // this is very tied to transform speed and sprite size
    public float loopTime = 2.45f;

    float timeElapsed = 0.0f;
    Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= loopTime)
        {
            transform.position = originalPosition;
            timeElapsed = 0;
        }
        transform.position += new Vector3(1.0f * Time.deltaTime, 1.0f * Time.deltaTime, 0.0f);
    }
}
