using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stats stat;
    public List<string> damageSources;
    public float movementSpeed;
    public EdgeCollider2D swordCollider;


	// Use this for initialization
	void Start ()
    {
        stat.Initialize();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
