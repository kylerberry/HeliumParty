using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EnemyGenerator enemyManager;  
    EdgeCreator worldBounds;
    LevelDisplayController lvlDisplay;
    ProgressPie progressPie;
    GameObject gameOver;
    BallGenerator ballManager;
    LifeDisplay lifeDisplay;
    ObstacleGenerator obstacleManager;
    BackgroundBehavior background;
/*    FillerFeedbackScript feedback;*/

    public int currLevel = 3;

    float levelCriteria = 0.5f;

    bool transitioningLevel = false;

    public int numLives = 3;
    private int lastNumLives;

    int maxObstacles = 3;

    void Start()
    {
        worldBounds = transform.Find("WorldBounds").GetChild(0).GetComponent<EdgeCreator>();
        enemyManager = transform.Find("EnemyManager").GetComponent<EnemyGenerator>();
        ballManager = transform.Find("BallManager").GetComponent<BallGenerator>();
        lvlDisplay = transform.Find("GameStatsBar").Find("LevelDisplay").GetComponent<LevelDisplayController>();
        lifeDisplay = transform.Find("GameStatsBar").Find("LifeDisplay").GetComponent<LifeDisplay>();
        gameOver = transform.Find("GameOver").gameObject;
        obstacleManager = transform.Find("ObstacleManager").GetComponent<ObstacleGenerator>();
        background = transform.Find("Background").GetComponent<BackgroundBehavior>();
        /*feedback = transform.Find("FilledFeedback").GetComponent<FillerFeedbackScript>();*/
        progressPie = transform.Find("GameStatsBar").Find("ProgressPie").GetComponent<ProgressPie>();


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
       /* progressPie.SetProgress((ballManager.ballCoverage / worldBounds.Area()) / levelCriteria);*/

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

    public void UpdateCoverage(float ballCoverage)
    {
        if (ballCoverage / worldBounds.Area() >= levelCriteria)
        {
            // End Level Lifecycle
            ballManager.Disable();
            obstacleManager.ClearObstacles();
            worldBounds.DisableCollider();
            transitioningLevel = true;
        }
       /* Debug.Log(ballManager.ballCoverage);
        Debug.Log((ballManager.ballCoverage / (worldBounds.Area() * levelCriteria)));
        Debug.Log((ballManager.ballCoverage / worldBounds.Area()) / levelCriteria);*/
        progressPie.SetProgress((ballManager.ballCoverage / worldBounds.Area()) / levelCriteria);
    }

    public void LevelReset()
    {
        // re-enable edge
        worldBounds.EnableCollider();
        /*obstacleManager.ClearObstacles();*/
        // reset enemy and ball manager
        ballManager.Reset();
        enemyManager.Reset();
        progressPie.SetProgress(0);
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
