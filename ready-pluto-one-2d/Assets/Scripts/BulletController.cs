using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    // Reference to the Rigidbody2D component called "theRB"
    public Rigidbody2D theRB;
    // The direction the bullet is moving
    public Vector2 moveDir;
    //
    public GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = moveDir * bulletSpeed;
    }

    // If the object has collided with another object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (impactEffect != null)
        {   // Bullet will make impact effect
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        // Bullet will be destroyed
        Destroy(gameObject);
    }

    // If the object has left the screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
