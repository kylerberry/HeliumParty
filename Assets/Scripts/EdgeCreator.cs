using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Creates Collider walls at the main camera bounds
 */
public class EdgeCreator : MonoBehaviour {

    Camera mainCam;

    // the world edge collider was given a fatter radius so that it had more stopping power against faster objects breaking out
    // the offset is so the bounds still sit at the screen edge despite the extra radius
    public float edgeOffset = 0.1f;
    Bounds collBounds;

    void Start()
    {
        mainCam = Camera.main;
        Vector2 lDCorner = mainCam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, mainCam.nearClipPlane));
        Vector2 rUCorner = mainCam.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, mainCam.nearClipPlane));
        EdgeCollider2D edge = gameObject.GetComponent<EdgeCollider2D>();

        Vector2[] colliderpoints = edge.points;
        colliderpoints[0] = new Vector2(lDCorner.x - edgeOffset, rUCorner.y + edgeOffset);
        colliderpoints[1] = new Vector2(rUCorner.x + edgeOffset, rUCorner.y + edgeOffset);
        colliderpoints[2] = new Vector2(rUCorner.x + edgeOffset, lDCorner.y - edgeOffset);
        colliderpoints[3] = new Vector2(lDCorner.x - edgeOffset, lDCorner.y - edgeOffset);
        colliderpoints[4] = new Vector2(lDCorner.x - edgeOffset, rUCorner.y + edgeOffset);
        edge.points = colliderpoints;
        collBounds = edge.bounds;
    }

    public float Area()
    {
        EdgeCollider2D edge = gameObject.GetComponent<EdgeCollider2D>();
        return edge.bounds.size.x * edge.bounds.size.y;
    }

    public bool Contains(Vector3 point)
    {
        return collBounds.Contains(point);
    }

    public void DisableCollider()
    {
        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
    }

    public void EnableCollider()
    {
        gameObject.GetComponent<EdgeCollider2D>().enabled = true;
    }

}
