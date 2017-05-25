using UnityEngine;
using System.Collections;

public class jumpStateScript : StateMachineBehaviour {

	GameObject go;
	//GameObject rotationDummy;
	private GameObject fishParent;
	Rigidbody gorb;
	private float jumpforce = 500.0f;
	private Vector3 jumpDirection;
	Quaternion targetRotation;
	private Vector3 targetOffset;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		go = animator.gameObject;
		gorb = go.GetComponent<Rigidbody>();
		fishParent = GameObject.FindGameObjectWithTag ("FishParent");

		//rotationDummy = new GameObject();
		//rotationDummy.transform.eulerAngles = new Vector3(-30.0f, go.transform.eulerAngles.y, 0.0f);
//		jumpDirection = rotationDummy.transform.forward;

		jumpDirection = fishParent.transform.forward;

		targetRotation = Quaternion.LookRotation (jumpDirection);

		GameObject[] downwaters = GameObject.FindGameObjectsWithTag("WaterDown");
		foreach (GameObject downwater in downwaters) {
			downwater.GetComponent<Collider>().enabled = false;
		}
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

			Vector3 forceVector = 2f * jumpforce * jumpDirection;
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
		animator.enabled = true;
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
