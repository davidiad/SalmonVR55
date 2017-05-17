using UnityEngine;
using System.Collections;

public class TurnRScript : StateMachineBehaviour {

	private GameObject fish;
	private float moveSpeed = 0.05f;
	private float _cumulativeDragAmount;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		fish = animator.gameObject;
		_cumulativeDragAmount = 0.0f;
		//fish.GetComponent<FishAni> ().cumulativeDragAmount = 0.0f;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (animator.GetBool ("turnR")) {
			_cumulativeDragAmount = fish.GetComponent<FishAni> ().cumulativeDragAmount;
			
			animator.Play ("turnR", 0, _cumulativeDragAmount / 50.0f);
			//Debug.Log ("CDA: " + _cumulativeDragAmount / 50.0f);

//			float turnAmount = animator.GetFloat ("turnAmount");
//			//Debug.Log ("turnAmount: " + turnAmount);
//			Vector3 moveDirection = new Vector3 ();
//			moveDirection = fish.transform.forward + 0.5f * fish.transform.right;
//			//Vector3 rot = new Vector3(0.0f, fish.transform.localRotation.y, fish.transform.localRotation.z);
//			moveDirection.Normalize ();
//			Vector3 movement = new Vector3 ();
//			movement = moveDirection * moveSpeed;
//			fish.transform.localPosition += movement;
//			// Update fish rotation
//			fish.GetComponent<Rigidbody> ().transform.Rotate (0.0f, turnAmount, 0.0f);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//animator.Play ("turning", 0, 0.0f);
	}

//	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
//	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
//	{
//
//	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
