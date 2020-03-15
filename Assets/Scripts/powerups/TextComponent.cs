using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextComponent : MonoBehaviour
{

    TextMeshPro text;
    int sign = 1;

    // @todo do size relative to parent transform
    int minSize = 12;
    int maxSize = 14;
    float maxBreathTime = 0.15f;
    float breathTime = 0.0f;

    void Awake()
    {
        text = gameObject.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        breathTime += Time.deltaTime;
        if (breathTime < maxBreathTime)
            return;

        breathTime = 0.0f;
        if (text.fontSize == maxSize)
            sign = -1;
        else if (text.fontSize == minSize)
            sign = 1;

        text.fontSize += sign * 1;
    }
}
