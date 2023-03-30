using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference to the Rigidbody2D component called "theRB"
    public Rigidbody2D theRB;
    // Move speed of the player
    public float moveSpeed;
    // Jump force of the player
    public float jumpForce;
    // Reference to the Transform component called "groundPoint"
    public Transform groundPoint;
    // We are going to use this to check if the player is on the ground
    private bool isOnGround;
    // A way to check what is considered ground
    public LayerMask whatIsGround;
    // Reference to the Animator component called "theAnim"
    public Animator theAnim;
    // Reference to the BulletController component called "shotToFire"
    public BulletController shotToFire;
    // Reference to the Transform component called "shotPoint"
    public Transform shotPoint;
    // Double Jump
    private bool canDoubleJump;

    // Dash
    public float dashSpeed, dashTime;
    // Dash Counter
    private float dashCounter;
    // Render the after image
    public SpriteRenderer theSR, afterImage;
    // Value to control the after image
    public float afterImageLifetime, timeBetweenAfterImages;
    // Keeping track of how long we wait between after images
    private float afterImageCounter;
    // After Image Color
    public Color afterImageColor;
    // How long we wait after dashing
    public float waitAfterDashing;
    // So the player can't continuously dash
    private float dashRechargeCounter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else
        {

            if (Input.GetButtonDown("Fire2"))
            {
                dashCounter = dashTime;
                ShowAfterImage();
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, theRB.velocity.y);

            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
            {
                ShowAfterImage();
            }

            dashRechargeCounter = waitAfterDashing;
        }
        else
        {
            // Move the player left and right
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);

            // Handle Direction Change 
            if (theRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (theRB.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }

        // Checking if the player is on the ground
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);

        // Jump
        if (Input.GetButtonDown("Jump") && (isOnGround || canDoubleJump))
        {
            if (isOnGround)
            {
                // Since player is in the air, they can double jump
                canDoubleJump = true;
            }
            else
            {
                canDoubleJump = false;
                // Set the animation
                theAnim.SetTrigger("doubleJump");
            }
            // Add force to the player
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }

        // Shoot
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);

            theAnim.SetTrigger("shotFired");
        }

        // Set the animation
        theAnim.SetBool("isOnGround", isOnGround);
        theAnim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }

    // Render the after image
    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }
}
