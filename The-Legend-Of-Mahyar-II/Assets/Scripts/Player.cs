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

    // private variables
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
    

    // Use this for initialization
    public override void Start ()
    {
        // Calls the Character Class Start Function
        base.Start();
	}
	
	// Update is called once per frame
	private void Update ()
    {
		
	}

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
    public override bool IsDead
    {
        get
        {
            if (healthStat.CurrentVal <= 0) { OnDead(); }
            return healthStat.CurrentVal <= 0;
        }
    }


    private void OnDead()
    {
        if (Dead != null) { Dead(); } 
    }

}
