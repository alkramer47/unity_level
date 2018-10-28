using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // GameObect is anything in the world 
    public GameObject target;
    public float followAhead;

    private Vector3 targetPosition;

    // allows the camera to run smoothly when following and respawning
    public float smoothing;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // this is the player's position
        targetPosition = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

        // this moves the target of the camera ahead of the player
        if (target.transform.localScale.x > 0f)
        {
            targetPosition = new Vector3(targetPosition.x + followAhead, targetPosition.y, targetPosition.z);
        }
        else
        {
            targetPosition = new Vector3(targetPosition.x - followAhead, targetPosition.y, targetPosition.z);
        }

        //transform.position = targetPosition;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

	}
}
