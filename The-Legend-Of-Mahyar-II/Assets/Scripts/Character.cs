using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    // Serializable variables
    public Stats healthStat;
    public EdgeCollider2D swordCollider; 
    public List<string> damageSources;
    public float movementSpeed;

    // Properties
    public Animator MyAnimator { get; set; }
    public EdgeCollider2D SwordCollider { get { return swordCollider; } }
    public bool Attack { get; set; }
    public bool TakingDamage { get; set; }

    // Protected variables
    protected bool facingRight;

    // abstract means that the classes that inherit this, needs to implement their own function
    public abstract IEnumerator TakeDamage();
    public abstract void Death();
    public abstract bool IsDead { get; }
    
    // Initialization of the characters basic stuff
    public virtual void Start ()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
        

	}

    // Handles the direction they change by multiplying the X scale by -1
    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // Uses the collider tag to differentiate who to take damage from
    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (damageSources.Contains(collider.tag)) { StartCoroutine(TakeDamage()); }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }

	
	
}
