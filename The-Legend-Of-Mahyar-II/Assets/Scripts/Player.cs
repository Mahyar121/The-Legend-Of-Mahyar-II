using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadEventHandler();

public class Player : Character
{
    public event DeadEventHandler Dead;
    // Serializable variables
    public Transform[] groundPoints;
    public LayerMask whatIsGround;
    public float jumpForce;
    public float groundRadius;
    public float immortalTime;
    public float climbSpeed;
    public float playerShredder;

    // private variables
    private GameObject[] pauseObjects;
    private Vector3 startPos;
    private SpriteRenderer spriteRenderer;
    private static Player instance;
    private bool isDead;
    private bool immortal = false;
    private IUseable useable;
    private Transform currentLocation;

    // Properties
    public Rigidbody2D MyRigidbody { get; set; }
    public bool OnLadder { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    public bool OnHeart { get; set; }

    private void Awake()
    {
        healthStat.Initialize();
    }

    // Use this for initialization
    public override void Start ()
    {
        // Calls the Character Class Start Function
        base.Start();
        OnLadder = false;
        OnHeart = false;
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidbody = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        FallingOffMapHandler();
        HandleInput();
    }

    // using FixedUpdate to deal with physics
    private void FixedUpdate()
    {
        MovingFlippingPlayer();
    }

    // creates a singleton of the player
    public static Player Instance
    {
        get
        {
            if (!instance) { instance = GameObject.FindObjectOfType<Player>(); }
            return instance;
        }
    }

    // Check to see if the player is dead
    private void OnDead()
    {
        if (Dead != null) { Dead(); }
    }

    // If the player is dead then set his hp to 0
    public override bool IsDead
    {
        get
        {
            if (healthStat.CurrentVal <= 0) { OnDead(); }
            return healthStat.CurrentVal <= 0;
        }
    }

    // Respawns the player when he dies
    public override void Death()
    {
        MyRigidbody.velocity = Vector3.zero;
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
    }

    // handles the taking damage part of the player
    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            healthStat.CurrentVal -= 10;
            if (!IsDead)
            {
                MyAnimator.SetTrigger("takingDamage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                MyAnimator.SetTrigger("dead");
                Death();
            }
        }
    }

    // Blinks the player on and off
    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    // the action button for the player
    private void Use()
    {
        if (useable != null) { useable.Use(); }
    }

    // if the player falls off the map, he respawns
    private void FallingOffMapHandler()
    {
        if (!TakingDamage && !IsDead)
        {
            if (transform.position.y <= playerShredder) { Death(); }
        }
        HandleInput();
    }

    // handles player input
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !OnLadder) { MyAnimator.SetTrigger("jump"); }
        if (Input.GetKeyDown(KeyCode.Z)) { MyAnimator.SetTrigger("attack"); }
        if (Input.GetKeyDown(KeyCode.C)) { Use(); }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    private void HandleMovement(float horizontal, float vertical)
    {
        if (MyRigidbody.velocity.y < 0) { MyAnimator.SetBool("land", true); }
        if (!Attack) { MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y); }
        if (Jump && MyRigidbody.velocity.y == 0 && !OnLadder) { MyRigidbody.AddForce(new Vector2(0, jumpForce)); }
        if (OnLadder)
        {
            MyAnimator.speed = vertical != 0 ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
            MyRigidbody.velocity = new Vector2(horizontal * climbSpeed, vertical * climbSpeed);
        }
        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    // flips the direction of the player
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) { ChangeDirection(); }
    }

    // Checking if player is touching the ground
    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Useable") { useable = collider.GetComponent<IUseable>(); }
        // Player gets the checkpoint of the heart and heals up
        if (collider.tag == "Heart")
        {
            healthStat.CurrentVal += 100;
            startPos = collider.gameObject.transform.position;
        }
        // checks for damage sources
        base.OnTriggerEnter2D(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Useable") { useable = null; }
    }

    private void showPaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    private void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    private void MovingFlippingPlayer()
    {
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            OnGround = IsGrounded();
            HandleMovement(horizontal, vertical);
            Flip(horizontal);

        }
    }
}
