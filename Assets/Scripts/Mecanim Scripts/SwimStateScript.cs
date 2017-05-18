using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwimStateScript : StateMachineBehaviour {

	private GameObject fish;
	private GameObject fishParent;
	private FishAni fishManager;
	private float moveSpeed = 1.1f;
	//public Slider speedSlider;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		fish = animator.gameObject;
		fishParent = GameObject.FindGameObjectWithTag ("FishParent");
		fish.GetComponent<Rigidbody> ().isKinematic = true;
		fishManager = fish.GetComponent<FishAni> ();

		//reset moveSpeed
		moveSpeed = 1.1f;
		fishManager.moveSpeed = moveSpeed;
	}

	/*
 	 //OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		Vector3 moveDirection = new Vector3();
		moveDirection = fish.transform.forward + 0.06f * fish.GetComponent<FishAni> ().chdir;
		moveDirection.Normalize();

		Quaternion rot = Quaternion.LookRotation(moveDirection);

		Vector3 movement = new Vector3();
		movement = fish.transform.forward * moveSpeed;
		fish.transform.localPosition += movement; // should be using movePosition() so it's not "teleporting", and the physics engine is aware of it.


	}
	*/

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		updateMoveSpeed ();
		fishParent.transform.position += fish.transform.forward * moveSpeed;
	}


	public void updateMoveSpeed()
	{
		moveSpeed = fishManager.moveSpeed;
		//Slider moveSpeedSlider = GetComponent<Slider>();
		//moveSpeedSlider = GameObject.Find("MoveSpeedSlider");
		//moveSpeed = speedSlider.value;
	}
}
