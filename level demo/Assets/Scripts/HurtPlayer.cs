using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour {

    private LevelManager theLevelManager;

    public int damageToGive;



	// Use this for initialization
	void Start () {

        //finds the level manager script
        theLevelManager = FindObjectOfType<LevelManager>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        //allows the player to be hurt by object
        if(other.tag == "Player")
        {
            //theLevelManager.Respawn();

            theLevelManager.hurtPlayer(damageToGive);
        }
    }

}
