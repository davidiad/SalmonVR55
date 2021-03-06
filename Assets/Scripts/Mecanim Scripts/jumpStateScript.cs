﻿using UnityEngine;
using System.Collections;

public class jumpStateScript : StateMachineBehaviour {

	GameObject go;
	//GameObject rotationDummy;
	private GameObject fishParent;
	Rigidbody gorb;
	private FishAni fishManager;
	private float jumpforce = 700.0f;
	private Vector3 jumpDirection;
	//private Vector3 modifiedJumpDirection;
	Quaternion targetRotation;
	private Vector3 targetOffset;
	public int counter;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		counter = 0;
		go = animator.gameObject;
		gorb = go.GetComponent<Rigidbody>();
		fishParent = GameObject.FindGameObjectWithTag ("FishParent");


		//rotationDummy = new GameObject();
		//rotationDummy.transform.eulerAngles = new Vector3(-30.0f, go.transform.eulerAngles.y, 0.0f);
//		jumpDirection = rotationDummy.transform.forward;

		jumpDirection = fishParent.transform.forward;
		//modifiedJumpDirection = new Vector3 (jumpDirection.x + 1.0f, jumpDirection.y, jumpDirection.z);
		targetRotation = Quaternion.LookRotation (jumpDirection) * Quaternion.AngleAxis(30.0f, Vector3.right);

		GameObject[] downwaters = GameObject.FindGameObjectsWithTag("WaterDown");
		foreach (GameObject downwater in downwaters) {
			counter += 1; // counter is just for testing, no need to keep
			downwater.GetComponent<Collider>().enabled = false;
			Debug.Log ("DISABLED WATER COLLIDER: " + counter);
		}

		fishManager = go.GetComponent<FishAni> ();
		//TODO:- convert to a function that accepts a time var, so can reuse in other states, like bump state
		fishManager.nonKinematicTime = 2.7f;
		fishManager.MinimizeTrigger ();
	} 

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		gorb.transform.rotation = Quaternion.Slerp(gorb.transform.rotation, targetRotation, 3.5f * Time.deltaTime);

		// Get the vector from the fish position to the fishParent position
		targetOffset = go.transform.position - go.transform.parent.position;
		go.GetComponent<FishAni> ().targetOffset = targetOffset;

		//if ((Vector3.Dot (go.transform.forward, jumpDirection)) > 0.98) {
		gorb.transform.parent = null;
		gorb.isKinematic = false;
		setRagdollState(true);
		animator.enabled = false;

		//Vector3 jumpDirectionModified = jumpDirection * Quaternion.AngleAxis(30, Vector3.right);

		Vector3 forceVector = jumpforce * jumpDirection;
		Debug.Log ("JUMPFORCE: " + jumpforce);
			gorb.AddForce(forceVector);
		//}
		if(Input.GetMouseButtonDown(0))
		{
			//
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//animator.enabled = true;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// Ragdoll functions repeated from fishAni script. Consolidate in one place
	public void setRagdollState(bool state) {
		// set the parent game object collision detection to opposite of "state"
		// set parent and children isKinematic to opposite of "state"
		
		// define the opposite boolean and set
		bool oppositeBoolean  = true;
		if (state) { oppositeBoolean = false; }
		
		go.GetComponent<Rigidbody>().detectCollisions = oppositeBoolean;
		turnOffChildCollidersPhysics(oppositeBoolean);
	}
	
	
	public void turnOffChildCollidersPhysics (bool turnOff) {
		//haveTurnedOffPhysics = turnOff;
		foreach (Transform child in go.transform) {
			turnOffPhysics(child, turnOff);
		}
	}
	
	public void turnOffPhysics(Transform obj, bool turnOff) {
		if(obj.GetComponent<Animation>()) {
			obj.GetComponent<Animation>().Stop();
		}
		if (obj.GetComponent<Rigidbody>()) 
		{
			obj.GetComponent<Rigidbody>().isKinematic = turnOff;
			obj.GetComponent<Rigidbody>().transform.localPosition = new Vector3(obj.GetComponent<Rigidbody>().transform.localPosition.x, 0.0f, 0.0f);
		}
		// recursively check children (bones)
		foreach (Transform trans in obj)  {
			turnOffPhysics(trans, turnOff);  
		}  
	}
}
