using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{

    // Text from the button that will be modified
    public Text Up, Down, Left, Right, Jump, Climb, Attack, Menu;

	// Use this for initialization
	void Start ()
    {
        Up.text = "Up Arrow";
        Down.text = "Down Arrow";
        Left.text = "Left Arrow";
        Right.text = "Right Arrow";
        Jump.text = "SpaceBar";
        Climb.text = "C";
        Attack.text = "Z";
        Menu.text = "Escape";
	}
	
	
}
