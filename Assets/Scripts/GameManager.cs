using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EnemyGenerator enemyManager;  
    EdgeCreator worldBounds;
    LevelDisplayController lvlDisplay;
    LevelProgressDisplay progressDisplay;
    GameObject gameOver;
    BallGenerator ballManager;
    LifeDisplay lifeDisplay;
    ObstacleGenerator obstacleManager;

    public int currLevel = 3;

    float levelCriteria = 0.5f;

    bool transitioningLevel = false;

    public int numLives = 3;
    private int lastNumLives;

    int maxObstacles = 3;

    void Start()
    {
        // get child references
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            switch (child.transform.name)
            {
                case "WorldBounds":
                    // use the first child of world bound because parent is just a trigger collider
                    worldBounds = child.transform.GetChild(0).GetComponent<EdgeCreator>();
                    break;
                case "EnemyManager":
                    enemyManager = child.GetComponent<EnemyGenerator>();
                    break;
                case "BallManager":
                    ballManager = child.GetComponent<BallGenerator>();
                    break;
                case "GameStatsBar":
                    // @todo this is a little thin, better to just look for the parent gameStatsBar wrap apis
                    lvlDisplay = child.transform.GetChild(1).GetComponent<LevelDisplayController>();
                    progressDisplay = child.transform.GetChild(2).GetComponent<LevelProgressDisplay>();
                    lifeDisplay = child.transform.GetChild(3).GetComponent<LifeDisplay>();
                    break;
                case "GameOver":
                    gameOver = child;
                    break;
                case "ObstacleManager":
                    obstacleManager = child.GetComponent<ObstacleGenerator>();
                    break;
                default:
                    Debug.Log(child.transform.name + " Not Assigned.");
                    break;
            }
        }

        // kickstart the first level
        enemyManager.numEnemies = currLevel;
        LevelBegin();
    }

    public void GameInit()
    {
        gameOver.SetActive(false);
        LevelReset();
        obstacleManager.ClearObstacles();
        currLevel = 3;
        enemyManager.numEnemies = currLevel;
        numLives = currLevel;
        LevelBegin();
    }

    // Update is called once per frame
    void Update()
    {
        // update progress UI
        progressDisplay.SetProgressPercent((ballManager.ballCoverage / worldBounds.Area()) / levelCriteria);

        if (ballManager.ballCoverage / worldBounds.Area() >= levelCriteria)
        {
            // End Level Lifecycle
            ballManager.Disable();
            obstacleManager.ClearObstacles();
            worldBounds.DisableCollider();
            transitioningLevel = true;
        }

        if (transitioningLevel && EveryBallHasClearedWorldbounds())
        {
            LevelReset();

            LevelIncrease(1);

            LevelBegin();
            transitioningLevel = false;
        }

        WatchLives(numLives, lastNumLives);
        lastNumLives = numLives;
    }

    public void LevelReset()
    {
        // re-enable edge
        worldBounds.EnableCollider();
        /*obstacleManager.ClearObstacles();*/
        // reset enemy and ball manager
        ballManager.Reset();
        enemyManager.Reset();
    }

    public void LevelIncrease(int amount = 0)
    {
        // increase level
        currLevel += 1;
        // increase enemies
        enemyManager.numEnemies = currLevel;
        // increase lives
        numLives += 1;
    }

    // @todo need to implement the lvl argument
    public void LevelBegin(int lvl = 1)
    {
        lvlDisplay.SetLevel(currLevel);
        /*obstacleManager.numObstacles = (int)Mathf.Min((float)currLevel, (float)maxObstacles);*/
        obstacleManager.SpawnObstacles();
        enemyManager.Launch();
        ballManager.Enable();
    }

    // if lives change, updates the lifeDisplay
    private void WatchLives(int newValue, int oldValue)
    {
        if (newValue != oldValue)
        {
            lifeDisplay.SetLife(newValue);
            if (newValue <= 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        ballManager.Disable();
        gameOver.SetActive(true);
    }

    public bool EveryBallHasClearedWorldbounds()
    {
        for (int i = 0; i < ballManager.releasedBalls.Count; i++)
        {
            GameObject ball = ballManager.releasedBalls[i];
            if (ball == null)
            {
                continue;
            }
           
            Vector3 ballTop = ball.transform.position;
            ballTop.y += ball.GetComponent<ReleasedBallController>().GetRadius();
           
            if (worldBounds.Contains(ballTop))
            {
                return false;
            }
        }
        return true;
    }

    public void IncrementLife(int amount = 1)
    {
        numLives += amount;
    }
}
