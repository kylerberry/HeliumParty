using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleasedBallController : MonoBehaviour
{

    private Rigidbody2D rb;
    private CircleCollider2D coll;

    public Vector3 minSize;

    // only play inertia effect if object has enough inertia (mass * acceleration)
    // negative because gravity pulls down (may need to change on gravity flip)
    private float minDustInertia = -10.0f;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<CircleCollider2D>();
        minSize = coll.bounds.size;
    }

    void PlayDust(Vector3 position)
    {
        // new position is relative to this ball transform
        // subtract the 2 world positions to get the correct vector from 0,0
        Vector2 line = position - transform.position;
        ParticleSystem dust = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule dustShape = dust.shape;

        // Get the angle of the tangent line to determine rotation of the dust cloud
        float impactAngle = Vector2.SignedAngle(Vector3.up, line);
        Vector3 dustRotation = new Vector3(0.0f, 0.0f, impactAngle);
        dustShape.rotation = dustRotation;
        dustShape.position = line;
        dust.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // hack to make any balls that fall out of the world destroy themselves so they can free up memory
        if (transform.position.y <= -50)
        {
            Destroy(gameObject);
        }

        // make balls shrink over time
        if (coll.bounds.size.x > minSize.x)
        {
            gameObject.transform.localScale -= new Vector3(0.15f * Time.deltaTime, 0.15f * Time.deltaTime);
        }
    }

    public float GetRadius()
    {
        return coll.bounds.size.x / 2;
    }

    // returns the area of the current circle
    public float GetArea()
    {
        return Mathf.PI * Mathf.Pow(coll.bounds.size.x / 2, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // only play dust if a certain inertia
        if (rb.velocity.y * rb.mass <= minDustInertia &&
            (collision.gameObject.CompareTag("WorldBounds") || collision.gameObject.CompareTag("ReleasedBall") || collision.gameObject.CompareTag("Obstacle")))
        {
            
            PlayDust(collision.ClosestPoint(transform.position));
            return;
        }
    }
}
