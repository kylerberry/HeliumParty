using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject stationaryObstacle;
    public GameObject movingObstacle;

    Camera mainCam;

    Vector2 worldLDCorner;
    Vector2 worldRUCorner;

    public int numObstacles = 1;

    public List<GameObject> obstacles;

    float gridSizeBuffer = 0.375f;

    void Start()
    {
        mainCam = Camera.main;
        worldLDCorner = mainCam.ViewportToWorldPoint(new Vector3(0, 0f, mainCam.nearClipPlane));
        worldRUCorner = mainCam.ViewportToWorldPoint(new Vector3(1f, 1f, mainCam.nearClipPlane));
    }

    public void ClearObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            Destroy(obstacles[i]);
            obstacles.RemoveAt(i);
        }
    }

    public bool PointIntersectsObstacles(Vector3 point)
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            GameObject obstacle = obstacles[i];
            if (obstacle.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().bounds.Contains(point))
            {
                return true;
            }
        }

        return false;
    }

    Vector3 GenerateWorldPoint()
    {
        Vector3 position = new Vector3(Random.Range(worldLDCorner.x + gridSizeBuffer, worldRUCorner.x - gridSizeBuffer), Random.Range(worldLDCorner.y + gridSizeBuffer, worldRUCorner.y - gridSizeBuffer));
        if (PointIntersectsObstacles(position))
        {
            return GenerateWorldPoint();
        }
        return position;
    }

    public void SpawnObstacles()
    {
        for (int i = 0; i < numObstacles; i++)
        {
            Vector3 position = GenerateWorldPoint();
            // @todo choose an obstacle enemy befitting the level difficulty
            GameObject obstacle = (GameObject)Instantiate(movingObstacle, position, Quaternion.identity);
            obstacle.transform.parent = gameObject.transform;
            obstacles.Add(obstacle);
        }
    }
}
