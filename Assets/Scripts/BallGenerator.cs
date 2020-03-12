using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{

    public GameObject growingBallPrefab;
    public GameObject releasedBallPrefab;
    public List<GameObject> releasedBalls;

    GameObject currGrowingBall;
    GameObject currReleasedBall;
    GameObject obstacleManager;
    bool clickEnabled = false;

    public float ballCoverage = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        obstacleManager = GameObject.Find("ObstacleManager");
        releasedBalls = new List<GameObject>();
    }

    // enable click to create ball
    public void Enable()
    {
        clickEnabled = true;
    }

    // Disable click to create ball
    public void Disable()
    {
        clickEnabled = false;
    }

    void UpdateBallCoverage()
    {
        float coverage = 0.0f;
        foreach (GameObject ball in releasedBalls)
        {
            if (ball == null)
                continue;

            coverage += ball.GetComponent<ReleasedBallController>().GetArea();
        }
        ballCoverage = coverage;
    }

    public void Reset()
    {
        Disable();
        foreach (GameObject ball in releasedBalls)
        {
            if (ball != null)
                // @todo an explosion animation would be cool here
                Destroy(ball);
        }
        releasedBalls = new List<GameObject>();
        ballCoverage = 0.0f;
    }

    // Does a point intersect a given collider
    private bool IsPointInsideSphere(Vector3 point, CircleCollider2D sphereColl)
    {
        float rad = sphereColl.bounds.size.x / 2;
        Vector3 center = sphereColl.transform.position;
        float diff = Vector3.Distance(center, point);
        return diff <= rad;
    }

    // Does a point intersect any releasedBalls
    private bool IsPointOverlapBalls(Vector3 point)
    {
        // see if mousepoint is on a released ball
        foreach (GameObject existing in releasedBalls)
        {
            if (existing == null)
                continue;

            if (IsPointInsideSphere(point, existing.GetComponent<CircleCollider2D>()))
                return true;
        }
        return false;
    }

    // Create a GrowingBall
    private void HandleInput(Vector3 inputPosition)
    {
        Vector3 mPosInWorld = Camera.main.ScreenToWorldPoint(inputPosition);
        mPosInWorld.z = 0.0f;

        // can't create ball inside another ball or obstacle
        if (IsPointOverlapBalls(mPosInWorld) ||
            obstacleManager.GetComponent<ObstacleGenerator>().PointIntersectsObstacles(mPosInWorld))
        {
            return;
        }
        currGrowingBall = (GameObject)Instantiate(growingBallPrefab, mPosInWorld, Quaternion.identity);
        currGrowingBall.transform.parent = gameObject.transform;
    }

    // Create a ReleasedBall
    private void HandleRelease()
    {
        // the growing ball was destroyed
        if (currGrowingBall == null || currGrowingBall.GetComponent<GrowingBallController>().isDestroyedByEnemy())
            return;

        // can't release ball inside another ball, worldbounds or obstacle
        if (currGrowingBall.GetComponent<GrowingBallController>().isContainedInAForbiddenCollider())
            return;

        // limit total balls to 64 by destroying the oldest
        // @todo maybe add a nice effect here?
        if (releasedBalls.Count == 64)
        {
            Destroy(releasedBalls[0]);
            releasedBalls.RemoveAt(0);
        }

        currReleasedBall = (GameObject)Instantiate(releasedBallPrefab, currGrowingBall.transform.position, Quaternion.identity);
        currReleasedBall.transform.localScale = currGrowingBall.transform.localScale;
        currReleasedBall.transform.parent = gameObject.transform;
        releasedBalls.Add(currReleasedBall);
    }

    // Update is called once per frame
    void Update()
    {
        if (!clickEnabled)
            return;

        // input click > instantiate ball
        // created at mouse position
        if (Input.GetButtonDown("Fire1") || Input.touchCount > 0)
        {
            Vector3 inputPosition;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                inputPosition = Input.GetTouch(0).position;
            else
                inputPosition = Input.mousePosition;

            HandleInput(inputPosition);
            return;
        }


        if (
            Input.GetButtonUp("Fire1") ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            HandleRelease();
            return;
        }

        if (releasedBalls.Count > 0)
        {
            UpdateBallCoverage();
        }
    }
}
