using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkAndDieScript : MonoBehaviour
{
    public float timeout = 9.0f;
    LineRenderer circle;
    TextMeshPro text;
    float timeAlive = 0;

    void Awake()
    {
        circle = gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
        text = gameObject.transform.GetChild(1).GetComponent<TextMeshPro>();
        ToggleVisibility();
        Enable();
    }

    public void Enable()
    {
        ToggleVisibility();
        // starts blinking after 1/3 of the time
        InvokeRepeating("Blink", timeout * 0.33f, 0.2f);
        // after 2/3 time the blink rate is made faster
        Invoke("IncreaseBlinkRate", timeout * 0.66f);
    }

    void IncreaseBlinkRate()
    {
        // cancels the old blink invocation
        CancelInvoke();
        // starts another blink at a faster rate
        InvokeRepeating("Blink", 0, 0.1f);
        Invoke("DestroyThis", timeout - timeAlive);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
    }

    void Blink()
    {
        circle.forceRenderingOff = !circle.forceRenderingOff;
        Renderer r = text.renderer;
        r.enabled = !r.enabled;
    }

    void ToggleVisibility()
    {
        Blink();
    }

}
