using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// @todo this may be a good way to create levels. Each "level" is a custom enemy generator
public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int numEnemies;

    private List<GameObject> enemies;

    private Camera mainCam;

    private void Awake()
    {
        enemies = new List<GameObject>();
        Reset();
    }

    public void Reset()
    {
        enemies.ForEach(delegate (GameObject enemy) { Destroy(enemy); });
        enemies.Clear();
    }

    public void Launch()
    {
        // use camera to get world bounds
        mainCam = Camera.main;
        Vector2 lDCorner = mainCam.ViewportToWorldPoint(new Vector3(0, 0f, mainCam.nearClipPlane));
        Vector2 rUCorner = mainCam.ViewportToWorldPoint(new Vector3(1f, 1f, mainCam.nearClipPlane));
        // declare storage size of enemies
        for (int i = 0; i < numEnemies; i++)
        {
            // spawn in a random position within the world bounds
            // @todo this could be made more performant by not deleting them all and just adding one more enemy each time....
            Vector3 position = new Vector3(Random.Range(lDCorner.x, rUCorner.x), Random.Range(lDCorner.y, rUCorner.y));
            GameObject enemy = (GameObject)Instantiate(enemyPrefab, position, Quaternion.identity);
            enemies.Add(enemy);
            enemy.transform.parent = gameObject.transform;
            enemy.GetComponent<EnemyController>().Launch();
        }
    }

    // Return a list of the instantiated enemies
    public List<GameObject> GetEnemies()
    {
        return enemies;
    }
}
