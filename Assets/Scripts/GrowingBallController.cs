using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBallController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private ParticleSystem popEffect;

    public float growRate = 10.0f;
    public Vector3 minSize;

    private bool hasRoomToGrow = true;
    private bool destroyedByEnemy = false;
    private Vector3 cachedInputPosition;
    private Vector3 minScale = new Vector3(0.5f, 0.5f);

    Collider2D currentCollision;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        coll = gameObject.GetComponent<CircleCollider2D>();
        popEffect = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        gameObject.transform.localScale = minScale;
        minSize = coll.bounds.size;
    }

    // returns the area of the current circle
    public float GetArea()
    {
        return Mathf.PI * Mathf.Pow(GetRadius(), 2);
    }

    //handles an ongoing input, whether it's mouse or touch
    private void HandleInput(Vector3 inputPosition)
    {
        // set body to current mouseposition in world units
        Vector3 ipWP = Camera.main.ScreenToWorldPoint(inputPosition);
        ipWP.z = 0.0f;
        rb.position = ipWP;

        // get proposed shrink vector (if needed)
        // mouse movement difference defines shrink-rate
        float diffDist = Vector3.Distance(cachedInputPosition, rb.position);
        Vector3 shrinkVector = new Vector3(growRate * diffDist, growRate * diffDist);

        // if in free space, grow at a defined rate
        if (hasRoomToGrow)
        {
            gameObject.transform.localScale += new Vector3(growRate * Time.deltaTime, growRate * Time.deltaTime);
        }
        // if colliding with a ball or wall shrink until no longer colliding
        // only minimize if the proposed scale would make a resulting size bigger than the minimum size
        else if (diffDist > 0.0f && (gameObject.transform.localScale - shrinkVector).x * coll.bounds.size.x > minSize.x)
        {
            gameObject.transform.localScale -= shrinkVector;
        }
            

        cachedInputPosition = ipWP;
    }

    //handles an input release, whether it's mouse or touch
    private void HandleRelease()
    {
        // Destroy this ball assuming it hasn't been hit by an enemy
        if (destroyedByEnemy)
            return;
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // if isDestroyed, destroy game object after particle system is finished
        if (destroyedByEnemy)
        {
            if (!popEffect.IsAlive())
                Destroy(gameObject);
            return;
        }

        // TOUCHHOLD (Growing)
        // ball moves with mouse position or touch
        if (Input.GetButton("Fire1") || Input.touchCount > 0)
        {
            Vector3 inputPosition;
            if (Input.touchCount > 0)
                inputPosition = Input.GetTouch(0).position;
            else
                inputPosition = Input.mousePosition;

            HandleInput(inputPosition);
            return;
        }

        // TOUCHRELEASE (Destroy)
        // destroy growing ball and instantiate released ball at x,y
        if (
            Input.GetButtonUp("Fire1") ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
                HandleRelease();
    }

    public float GetRadius()
    {
        return coll.bounds.size.x / 2;
    }

    // kickstarts a ball's destruction lifecycle
    public void Pop()
    {
        // hide the sprite
        sr.enabled = false;

        // particle system radius explodes at the unreleased ball radius
        var shape = popEffect.shape;
        shape.radius = GetRadius();

        // hide the ball collider
        coll.enabled = false;
        
        // flag so the ball is Destroyed after the particle effect is finished
        destroyedByEnemy = true;

        // notify that a life is lost
        gameObject.SendMessageUpwards("IncrementLife", -1);

        // play the particle effect
        popEffect.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Pop();
            return;
        }

        currentCollision = collision;

        hasRoomToGrow = !collision.gameObject.CompareTag("WorldBounds") &&
            !collision.gameObject.CompareTag("ReleasedBall") &&
            !collision.gameObject.CompareTag("Obstacle");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentCollision = null;

        hasRoomToGrow = collision.gameObject.CompareTag("WorldBounds") ||
            collision.gameObject.CompareTag("ReleasedBall") ||
            collision.gameObject.CompareTag("Obstacle");
    }

    public bool isContainedInAForbiddenCollider()
    {
        // everything is contained in the world bounds
        if (currentCollision == null || currentCollision.gameObject.CompareTag("WorldBounds"))
            return false;

        return currentCollision.bounds.Contains(transform.position);
    }

    public bool isDestroyedByEnemy()
    {
        return destroyedByEnemy;
    }
}
