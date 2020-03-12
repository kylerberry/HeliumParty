using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 0.5f;

    int waypointIndex = 0;
    GameObject worldBounds;
    Transform obstacle;
    float[] angles;

    void Awake()
    {
        obstacle = gameObject.transform.GetChild(0);

        angles = new float[4] { 0, 90, 180, 270 };
        worldBounds = GameObject.Find("WorldBounds");

        RandomizeWaypointOrientation();
    }

    void RandomizeWaypointOrientation()
    {
        // flip
        if (Mathf.Round(Random.Range(0, 1)) == 1.0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1.0f;
            transform.localScale = scale;
        }

        // rotation angle
        int angleIndex = Random.Range(0, angles.Length);
        transform.Rotate(new Vector3(0.0f, 0.0f, angles[angleIndex]));
        if (!CheckWaypointsInWorld())
        {
            RandomizeWaypointOrientation();
        }
    }

    // pretty unsuphisticated way of checking that the waypoints are in the world
    // @todo check where they are outside so we can save some cycle doing a transform that will work
    bool CheckWaypointsInWorld()
    {
        foreach (Transform w in waypoints)
        {
            if (!worldBounds.GetComponent<EdgeCreator>().Contains(w.position))
            {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        obstacle.position = Vector3.MoveTowards(
           obstacle.position,
           waypoints[waypointIndex].transform.position,
           speed * Time.deltaTime
        );

        if (obstacle.position == waypoints[waypointIndex].transform.position)
            waypointIndex++;

        if (waypointIndex == waypoints.Count)
        {
            // back and forth movement
            waypoints.Reverse();
            waypointIndex = 0;
        }
            
    }
}
