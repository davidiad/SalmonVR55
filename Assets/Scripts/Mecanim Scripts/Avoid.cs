﻿using UnityEngine;
using System.Collections;

public class Avoid : StateMachineBehaviour {

	private GameObject fish;
	private GameObject movingObject;
	FishAni fishManager;
	//private float turningMoveSpeed;
	public Vector3 directionChange;
	private float waterlevel;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {


		fish = animator.gameObject;
		movingObject = GameObject.FindGameObjectWithTag ("MainCamera");
		fishManager = fish.GetComponent<FishAni> ();
		waterlevel = fishManager.waterlevel;
		//		waterlevel = fish.GetComponent<FishAni> ().waterlevel;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//TODO:- refactor as case-switch

		Vector3 direction = new Vector3 ();
		direction = movingObject.transform.forward;
		RaycastHit hit = new RaycastHit ();
		float hitLength = 14.0f;

		//TODO: adjust amount of compensation to the distance till the hit, to reduce to constant up and down
		directionChange = new Vector3 (0.0f, 0.0f, 0.0f);
		int layermask = (1 << 11) | (1 << 4); // layer 13 is the fish trigger, don't want the ray to detect that

		// If fish is above surface
		if (fish.transform.position.y > (waterlevel - 0.2f)) {
			float amountToRotateDown = (fish.transform.position.y - (waterlevel - 0.2f)) * 4.0f;
			if (amountToRotateDown > 10.0f) {
				amountToRotateDown = 10.0f;
			} else if (amountToRotateDown < 1.0f) {
				amountToRotateDown = 1.0f;
			}
			directionChange = new Vector3 (amountToRotateDown, 0.0f, 0.0f);
			fish.transform.Rotate (directionChange);
		}
		Debug.Log("Avoid1");
		if (Physics.Raycast (movingObject.transform.position, movingObject.transform.forward, out hit, hitLength, layermask)) {
			Debug.Log("Avoid2");
			Debug.DrawRay (movingObject.transform.position, movingObject.transform.forward, Color.yellow, hitLength);
			// If the obstacle is the water surface, then point the fish back down, and stay in swim state
			//if (!Input.anyKey) { // don't do automatic turning if the user is trying to control turning (mouse, or any key, down)
			if (hit.collider.gameObject.tag == "WaterDown") {
				// Adjust direction down
				directionChange = new Vector3 (1.0f, 0.0f, 0.0f);
				fish.transform.Rotate (directionChange);
			} else {
				// Otherwise, change the state to SawObstacle
				animator.SetBool ("sawObstacle", true);
			}
			Vector3 hitNormal = hit.normal.normalized; // is it necc. to normalize or has it already been normalized?

			// Test whether hitNormal is close to aligned to forward
			if ((Mathf.Abs (Vector3.Dot (fish.transform.forward, hitNormal)) < 0.6f)) {
				fish.GetComponent<FishAni> ().chdir = hitNormal.normalized;
				// hitNormal is too close to aligned with forward to use to change directions (in other words, pointed too directly at the obstacle)
				// so push to the side (right in this case)
			} else { 
				fish.GetComponent<FishAni> ().chdir = fish.transform.right;
			}
			//} else {
			// The mouse (or touch) is down, so instead of turning, which would take control away from the user, reduce the speed
			//TODO: how to put moveSpeed to where it has an effect
			//	fishManager.turningMoveSpeed = fishManager.turningMoveSpeed * (hit.distance / hitLength);
			//}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	//	public void avoidObstacles() 
	//	{
	//		//fish = GameObject.FindGameObjectWithTag("Fishy");
	//		Animator animator = fish.GetComponent<Animator> ();
	//		Vector3 direction = new Vector3();
	//		direction = fish.transform.forward;
	//		RaycastHit hit = new RaycastHit();
	//		float hitLength = 15.0f;
	//		directionChange = new Vector3(0.0f, 0.0f, 0.0f);
	//		int layermask = (1<<11) | (1<<4); // layer 13 is the fish trigger, don't want the ray to detect that
	//		
	//		if (Physics.Raycast(fish.transform.position, fish.transform.forward, out hit, hitLength, layermask))
	//		{
	//			Debug.Log ("Hit This: " + hit.collider.gameObject.tag);
	//			// If the obstacle is the water surface, then point the fish back down, and stay in swim state
	//			 if (hit.collider.gameObject.tag == "WaterDown") {
	//				// Adjust direction down
	//				Debug.Log ("I see the Water!");
	//			} else {
	//			// Otherwise, change the state to SawObstacle
	//				animator.SetBool("sawObstacle", true);
	//			}
	//			//Debug.DrawRay(fish.transform.position, fish.GetComponent<Rigidbody>().transform.forward, Color.blue, 1.0f);
	////			if (Physics.Raycast(fish.transform.position, fish.transform.forward, out hit, 0.5f, layermask))
	////			{
	////				Debug.DrawRay(fish.transform.position, fish.GetComponent<Rigidbody>().transform.forward * 0.5f, Color.magenta, 1.0f);
	////				moveSpeed = 0.0f;
	////				//SetTransition(Transition.SawObstacle);
	////			}
	////			else if (Physics.Raycast(fish.transform.position, fish.transform.forward, out hit, 1.5f, layermask))
	////			{
	////				Debug.Log("Green Line");
	////				Debug.DrawRay(fish.transform.position, fish.GetComponent<Rigidbody>().transform.forward * 1.5f, Color.green, 1.5f);
	////				moveSpeed = 0.04f;
	////			}
	////			else
	////			{
	////				Debug.Log("Yellow Line");
	////				Debug.DrawRay(fish.transform.position, fish.GetComponent<Rigidbody>().transform.forward * hitLength, Color.yellow, hitLength);
	////				//moveSpeed = speedSlider.value;
	////			}
	//			Vector3 hitNormal = hit.normal;
	//
	//			// Test whether hitNormal is close to aligned to forward
	//			Debug.Log ("DOT: " + Vector3.Dot (fish.transform.forward, hitNormal));
	//			if ( (Mathf.Abs(Vector3.Dot(fish.transform.forward, hitNormal)) < 0.8f)   )
	//			{
	//				fish.GetComponent<FishAni>().chdir = 0.05f * hitNormal;
	//			// hitNormal is too close to aligned with forward to use to change directions (in other words, pointed too directly at the obstacle)
	//			// so push to the side (right in this case)
	//			} else { 
	//				fish.GetComponent<FishAni>().chdir = 0.125f * fish.transform.right;
	//			}
	//			//animator.GetComponent<SwimStateScript>().directionChange = hitNormal;
	//			//direction = direction + hitNormal;
	//			//direction.Normalize();
	//			//direction = rightToForward(direction).normalized;
	//			//Vector3 newDirection = direction;
	//			//foundClearDirection = true;
	//		}
	//		// split into separate state, swimming near surface??
	////		if (fish.GetComponent<Rigidbody>().transform.position.y > 14.4f) // replace with waterlevel variable
	////		{
	////			Debug.Log ("near the surface");
	////			//Why zero out the Y????
	////			// Why not zero out the x???? We are trying to make it go flat, right?
	////			//Vector3 atSurfaceDirection = new Vector3(direction.x, 0.0f, direction.z);
	////			Vector3 atSurfaceDirection = new Vector3(direction.x, direction.y, 0.0f);
	////			direction = atSurfaceDirection;
	////			direction.Normalize();
	////			//newDirection = rightToForward(direction).normalized;
	////			Quaternion rot = Quaternion.LookRotation(direction);
	////			fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, rot, Time.fixedDeltaTime);
	////		}
	////		
	////		if ( Mathf.Abs(fish.transform.rotation.eulerAngles.z) > 10)
	////		{
	////			Debug.Log("TILTED");
	////			Vector3 uprightDirection = new Vector3(direction.x, direction.y, 0.0f);
	////			direction = uprightDirection;
	////			direction.Normalize();
	////			//newDirection = rightToForward(direction).normalized;
	////			Quaternion rot = Quaternion.LookRotation(direction);
	////			fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, rot, Time.fixedDeltaTime);
	////		}
	//	}

}
