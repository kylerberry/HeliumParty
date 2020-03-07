using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	Rigidbody2D rb;

	public float thrust = 4.0f;
	private float angle;
	private Vector3 direction;

	private void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		angle = Random.Range(0.0f, 360.0f);
		// get a point on a circle (x,y) = (r*sin(angle), r*cos(angle))
		// used to get a random direction for the enemy to travel multiplied by thrust
		direction = new Vector3(thrust * Mathf.Sin(angle), thrust * Mathf.Cos(angle));
	}

	public void Launch()
	{
		rb.AddForce(direction);
	}

	public void ChangeVelocity(Vector2 velocity)
	{
		rb.velocity = velocity;
	}

	private void Update()
	{
		if (
			transform.position.y <= -50 ||
			transform.position.y >= 50 ||
			transform.position.x <= -50 ||
			transform.position.y >= 50
		)
		{
			Destroy(gameObject);
		}
		// make sure the speed of the enemy is alway constant
		rb.velocity = 4.0f * rb.velocity.normalized;
	}

	void PlayDust()
	{
		// @todo set rotation of dust
		ParticleSystem dust = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
		/*ParticleSystem.ShapeModule dustShape = dust.shape;
		dustShape.position = position;*/
		dust.Play();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		
		if (collision.gameObject.CompareTag("WorldBounds"))
		{
			PlayDust();
			// bounce direction
			ChangeVelocity(
				new Vector2(
					rb.velocity.x + collision.attachedRigidbody.velocity.x, 
					rb.velocity.y + collision.attachedRigidbody.velocity.y
				)
			);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
	}

	// fixed ball sticking to wall by lowering the velocity threshold for the project settings. 
	// It could still happen if the velocity is < .1 but this should be pretty rare
}
