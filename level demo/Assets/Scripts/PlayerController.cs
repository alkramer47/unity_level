using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    private Rigidbody2D myRigidBody;
    private Animator myAnim;

    public float jumpSpeed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    public Vector3 respawnPosition;

    public LevelManager theLevelManager;

    public GameObject stompBox;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;

    public float invincibilityLength;
    private float invincibilityCounter;

     

	// Use this for initialization
	void Start ()
    {
        //gives the player rigidbody allowing them to move as well as setting animations
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        respawnPosition = transform.position;

        //allows the level manager script to be called
        theLevelManager = FindObjectOfType<LevelManager>();

		
	}
	
	// Update is called once per frame
	void Update ()
    {

        // checks if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        //takes movement values and allows player to be knocked back after being damaged
        if (knockbackCounter <= 0)
        {
            //"horizontal" is inscripted into unity allowing us to refer to it directly in the script
            if (Input.GetAxisRaw("Horizontal") > 0f)
            {
                //moves the player forwards
                myRigidBody.velocity = new Vector3(moveSpeed, myRigidBody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                //moves the player backwards
                myRigidBody.velocity = new Vector3(-moveSpeed, myRigidBody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                myRigidBody.velocity = new Vector3(0f, myRigidBody.velocity.y, 0f);
            }

            //allows the player to jump as long as they are on the ground
            //also makes it so the player HAS to be grounded to jump again so no infinite jumping
            if (Input.GetButtonDown("Jump") && isGrounded)
            {

                myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, jumpSpeed, 0f);
            }

            

        }

        //settings for being knocked back with accordance to time
        if(knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;

            if (transform.localScale.x > 0)
            {
                myRigidBody.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
            }
            else
            {
                myRigidBody.velocity = new Vector3(knockbackForce, knockbackForce, 0f);
            }
        }

        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }
        if(invincibilityCounter <= 0)
        {
            theLevelManager.invincible = false;
        }
        
        //makes it so speed is always positive so when player changes direction the animation functions properly
        myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
        myAnim.SetBool("Grounded", isGrounded);

        if(myRigidBody.velocity.y < 0)
        {
            stompBox.SetActive(true);
        }
        else
        {
            stompBox.SetActive(false);
        }
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLength;
        invincibilityCounter = invincibilityLength;
        theLevelManager.invincible = true;

    }

    //activates triggers 
    void OnTriggerEnter2D(Collider2D other)
    {
        //kills the player when falling off the screen
        if (other.tag == "KillPlane")
        {
            //  gameObject.SetActive(false);
            transform.position = respawnPosition;
            theLevelManager.Respawn();
        }

        //allows the checkpoint to create new spawn
        if(other.tag == "Checkpoint")
        {
            respawnPosition = other.transform.position;
        }

    }

    //makes the moving platform the parent so the player isnt left behind while the platform moves
    void OnCollisionEnter2D(Collision2D other)
    {
        
        if(other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }

    }

    //exits the moving platform making the player independent again
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

}
