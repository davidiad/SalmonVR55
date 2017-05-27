using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimAboveWaterState: StateMachineBehaviour {

	private GameObject GO;
	private Rigidbody GORB;
	private FishAni fishManager;


	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GO = animator.gameObject;
		GORB = GO.GetComponent<Rigidbody>();
		fishManager = GO.GetComponent<FishAni> ();
		fishManager.kinematicTimer = 0.3f;
		fishManager.MinimizeTrigger ();

	}


	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (GO.GetComponent<Animation>()) {
			GO.GetComponent<Animation>().Stop();
		}

		GORB.transform.parent = null;
		GORB.isKinematic = false;
		fishManager.setRagdollState(true);
		animator.enabled = false;

	
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
}
