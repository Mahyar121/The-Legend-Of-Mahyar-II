using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// forces unity to give it a rigidbody2d
[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour {

    // Serializable variables
    public float speed;

    // private variables
    private Rigidbody2D myRigidBody;
    private Vector3 direction;


	// Use this for initialization
	private void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {
        myRigidBody.velocity = direction * speed;
    }

    public void InitializeArrowDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
