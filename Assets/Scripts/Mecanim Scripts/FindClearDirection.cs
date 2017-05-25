using UnityEngine;
using System.Collections;

public class FindClearDirection : StateMachineBehaviour {

	private GameObject fish;
	private GameObject fishParent;
	private Vector3 direction;
	private FishAni fishManager;
	private GameObject mainCam;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
		fish = animator.gameObject;
		fishParent = GameObject.FindGameObjectWithTag("FishParent");
		fishManager = fish.GetComponent<FishAni> ();
		fish.transform.position += fish.transform.forward * fishManager.moveSpeed; // 1 last fish movement, so that camera matches up

		mainCam = GameObject.FindGameObjectWithTag("MainCamera");
		direction = fish.transform.forward;


	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		// slow down the fish, until the user rotates to a clear direction
		if (fishManager.moveSpeed > 0.03f) {
			fishManager.moveSpeed -= .005f;
		}

		RaycastHit hit = new RaycastHit();
		float hitLength = 13.0f; // make hitLength larger than Avoid hitLength, so that it's not sent back to Swim state unless it's definitely clear
		int layermask = (1<<11) | (1<<4); // layer 13 is the fish trigger, don't want the ray to detect that
		Debug.DrawRay (fish.transform.position, mainCam.transform.forward, Color.red, 6.0f);
		if (!Physics.Raycast (fish.transform.position, mainCam.transform.forward, out hit, hitLength, layermask)) {
			//fishManager.moveSpeed = 1.4f;
			animator.SetBool ("foundClearDirection", true);
		}

		fishParent.transform.position += fish.transform.forward * fishManager.moveSpeed;
		//if (Vector3.Dot (fish.transform.forward, direction) > 0.9f) {

		Vector3 chdir = fishManager.chdir;

			direction = (fish.transform.forward + 0.2f * chdir).normalized;
			Debug.DrawRay (fish.transform.position, direction, Color.black, 15.0f);
			Debug.DrawRay (fish.transform.position, chdir, Color.blue, 15.0f);
			Debug.DrawRay ((fish.transform.position + direction).normalized, (fish.transform.position + chdir).normalized, Color.red, 15.0f);
		//}

		/* Turn off rotation update for now, as moveSpeed has been set to 0
		Quaternion rot = Quaternion.LookRotation(direction);
		fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, rot, 3.0f * Time.deltaTime);
		*/

//		if (Vector3.Dot (fish.transform.forward, direction) > 0.9f) {
//			animator.SetBool ("foundClearDirection", true);
//		}

		/* Not using for VR -- rotation is set by user
		RaycastHit hit = new RaycastHit();
		float hitLength = 9.0f;
		int layermask = (1<<11) | (1<<4); // layer 13 is the fish trigger, don't want the ray to detect that
*/
		//Debug.DrawRay (fish.transform.position, fish.GetComponent<Rigidbody> ().transform.forward * 2.5f, Color.white, 2.0f);

		// If the obstacle is very close, move to the ObstacleIsClose state (but only once the current Slerp has finished, or at least close to finished)
		//if (Vector3.Dot (fish.transform.forward, direction) > 0.8f) {
			if (Physics.Raycast (fish.transform.position, fish.transform.forward, out hit, 3.5f, layermask)) {
				animator.SetBool ("foundClearDirection", false);
				animator.SetBool ("obstacleIsClose", true);
				fishManager.moveSpeed = 0.0f; // stop the fish so it doesn't hit the obstacle. Until the user turns to a clear direction
			} else if (Physics.Raycast (fish.transform.position, fish.transform.forward, out hit, hitLength, layermask)) {
				//if (Physics.Raycast (fish.transform.position, fish.transform.forward, out hit, 0.5f, layermask)) {
				Debug.DrawRay (fish.transform.position, fish.GetComponent<Rigidbody> ().transform.forward * 3.5f, Color.magenta, 1.0f);
				//}
			} else {
				fishManager.moveSpeed = 0.13f;
				animator.SetBool ("foundClearDirection", true);
			}
		//}

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

//	findDirection() {
//		fish = GameObject.FindGameObjectWithTag("Fishy");
//		Animator animator = fish.GetComponent<Animator> ();
//		Vector3 direction = new Vector3();
//		direction = fish.transform.forward + fish.transform.right;
//
//		Quaternion rot = Quaternion.LookRotation(direction.normalized);
//		fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, rot, Time.fixedDeltaTime);
//	}
}
