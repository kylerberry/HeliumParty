using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetProgressPercent(float progress = 0.1f)
    {
        if (progress > 1.0)
            progress = 1.0f;
        gameObject.GetComponent<TextMesh>().text = Mathf.Floor(progress * 100) + "/100";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
