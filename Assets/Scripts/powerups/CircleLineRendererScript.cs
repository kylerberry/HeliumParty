using UnityEngine;
using System.Collections;

public class CircleLineRendererScript : MonoBehaviour
{
    public int segments;
    public float xradius = 1.0f;
    public float yradius = 1.0f;
    LineRenderer line;
/*    float lineMinWidth = 0.05f;
    float lineMaxWidth = 0.15f;
    float lineWidth;
    int sign = 1;*/

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        /*lineWidth = lineMinWidth;*/
    }

    private void Update()
    {
/*        lineWidth += sign * 0.005f;
        if (lineWidth >= lineMaxWidth)
            sign = -1;
        else if (lineWidth <= lineMinWidth)
            sign = 1;
        line.SetWidth(lineWidth, lineWidth);*/
        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }
}
