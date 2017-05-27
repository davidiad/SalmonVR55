using UnityEngine;
using System.Collections;

public class FishAni : MonoBehaviour 
{
	public Vector3 chdir;
	public float waterlevel = 24.7f;
	public float cumulativeDragAmount;
	public float moveSpeed;
	public float turningMoveSpeed; // speed of forward motion while turning under user control
	Animator anim;
	private GameObject mainCam;

	private GameObject fish;
	private GameObject fishParent;
	public Vector3 targetOffset = new Vector3 (-0.58f, -0.67f, 1.62f);

	private GameObject dummyFish;
	private GameObject dummyParent;

	int tapHash = Animator.StringToHash("tap");
	public float kinematicTimer;
	public float nonKinematicTime;
	bool goneAboveWater;
	private SphereCollider triggerCollider;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().isKinematic = true;
		anim = GetComponent<Animator>();

		chdir = new Vector3 (0.0f, 0.0f, 0.0f);
		kinematicTimer = 0.0f;
		nonKinematicTime = 3.0f;
		goneAboveWater = false;
		GameObject bumpTrigger = GameObject.FindGameObjectWithTag("fishtrig3");
		triggerCollider = bumpTrigger.GetComponent<SphereCollider> ();

		fish = GameObject.FindGameObjectWithTag("Fishy");
		fishParent = GameObject.FindGameObjectWithTag("FishParent");
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
		dummyFish = GameObject.FindGameObjectWithTag ("DummyFish");
		dummyParent = GameObject.FindGameObjectWithTag ("DummyParent");


	}

	// alternate (and more accurate) way to relalign camera to fish
	public void alignCamToFish() {
		dummyFish.transform.position = fish.transform.position;
		dummyFish.transform.rotation = mainCam.transform.rotation;
		fishParent.transform.position = dummyParent.transform.position;
		//fishParent.transform.position = Vector3.Lerp(fishParent.transform.position, dummyParent.transform.position, .05f);
		fish.transform.SetParent(fishParent.transform);
	}

	// Update is called once per frame
	void Update () {

////		// if the fish is jumping, or otherwise non-kinematic, keeping the fishParent position matching to the fish
//		if (fish.transform.parent == null) {
//			alignCamToFish ();
//			//fishParent.transform.position = Vector3.Lerp(fishParent.transform.position, gameObject.transform.position - targetOffset, .11f);
//		}




		// Adjust waterlevel for waterfall
//		if (transform.position.z > 166.5f) {
//			waterlevel = 19.7f;
//		} else {
//			waterlevel = 14.7f;
//		}

		// If fish is above the water, and is Kinematic, go to a new animator state to be defined



		/*** NON-KINEMATIC CASES ***/
		if (!(GetComponent<Rigidbody> ().isKinematic)) {
			Debug.Log ("/*** NON-KINEMATIC CASES ***/");

			if (fish.transform.position.y > (waterlevel + 0.8f)) {
				goneAboveWater = true;
				Debug.Log ("Gone above water");
			} else {
				Debug.Log ("Under the water");
			}

			// mimic the force of the fish falling into the water. Should vary with velocity.
			if (goneAboveWater && (fish.transform.position.y < (waterlevel + 0.8f)) && (fishParent.transform.position.y > (waterlevel + 0.5f))) {
				float speed = 25.0f * GetComponent<Rigidbody> ().velocity.y;
				GetComponent<Rigidbody> ().AddForce (Vector3.up * speed); 
			}
				

//			// simplified returnToKinematic conditions
//			if (kinematicTimer < nonKinematicTime) {
//				kinematicTimer += Time.deltaTime;
//			} else {
//				returnToKinematic ();
//			}


//			// start timer
//			if (fish.transform.position.y < (waterlevel + 0.3f)) {
//				if (kinematicTimer < 0.5f) {
//					kinematicTimer += Time.deltaTime;
//					// if > .5s, and hasn't gone above water, return to kinematic
//				} else if (!goneAboveWater) { 
//					returnToKinematic ();
//					// if has gone above water, return to kin. in 2 or 3 s
//				} else if (kinematicTimer < nonKinematicTime) {
//					kinematicTimer += Time.deltaTime;
//				} else {
//					returnToKinematic ();
//				}
//			}
		



			//if (goneAboveWater) {
				if (fish.transform.position.y < 24.0f) {
					if (kinematicTimer < nonKinematicTime) {
						kinematicTimer += Time.deltaTime;
					} else {
						//kinematicTimer = 0.0f;
						goneAboveWater = false;
						GetComponent<Rigidbody> ().isKinematic = true;
						// Turn the animator state machine back on, now that we are done with using physics engine
						anim.enabled = true;
						// set the state to swim, otherwise it picks up where it left off with jump
						anim.CrossFade ("swim", 0.0f);
						GameObject downwater = GameObject.FindWithTag ("WaterDown");
						//downwater.GetComponent<Collider>().enabled = true;
						GameObject[] downwaters = GameObject.FindGameObjectsWithTag ("WaterDown");
						foreach (GameObject watertile in downwaters) {
							watertile.GetComponent<Collider> ().enabled = true;
						}
						// need to turn kinematic back on
						setRagdollState (false);
					}
				}
			//}
		}


		// trigger to jump state
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(Input.GetMouseButtonDown(0))
		{
			GameObject fish = GameObject.FindGameObjectWithTag("Fishy");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				if ( GameObject.Find(hit.collider.gameObject.name) == fish)
				{
					anim.SetTrigger (tapHash);
				}
			}
		}



	}

	public void MinimizeTrigger() {

		if (triggerCollider != null) {
			triggerCollider.radius = 0.01f;
			triggerCollider.center = new Vector3 (0, 0, 0);
		}
	}

	public void MaximizeTrigger() {

		if (triggerCollider != null) {
			triggerCollider.radius = 0.15f;
			triggerCollider.center = new Vector3 (0f, 0.1f, 1.0f);
		}
	}

	public void returnToKinematic() {

		// reset the fishParent position to be close to the fish
		//fishParent.transform.position = gameObject.transform.position - targetOffset;


//		// match the fishParent rotation to the rotation of the camera
//		fishParent.transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;

		alignCamToFish ();
		// then reestablish the parent-child relationship
		//fish.transform.SetParent(fishParent.transform);
		//gameObject.transform.SetParent(fishParent.transform, false);

		kinematicTimer = 0.0f;
		goneAboveWater = false;
		GetComponent<Rigidbody>().isKinematic = true;
		// Turn the animator state machine back on, now that we are done with using physics engine
		anim.enabled = true;
		// set the state to swim, otherwise it picks up where it left off with in jumping state
		anim.CrossFade("swim", 0.0f);

		GameObject[] downwaters = GameObject.FindGameObjectsWithTag("WaterDown");
		foreach (GameObject downwater in downwaters) {
			downwater.GetComponent<Collider>().enabled = true;
		}
		// need to turn kinematic back on
		setRagdollState(false);
		MaximizeTrigger ();


	}

	public void setRagdollState(bool state) {
		// set the parent game object collision detection to opposite of "state"
		// set parent and children isKinematic to opposite of "state"

		// define the opposite boolean and set
		bool oppositeBoolean  = true;
		if (state) { oppositeBoolean = false; }

		GetComponent<Rigidbody>().detectCollisions = oppositeBoolean;
		turnOffChildCollidersPhysics(oppositeBoolean);
	}


	public void turnOffChildCollidersPhysics (bool turnOff) {
		//haveTurnedOffPhysics = turnOff;
		foreach (Transform child in transform) {
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

	// VR Gesture
	public void fishTapped() {
		//		animator = GameObject.FindGameObjectWithTag ("Fishy").GetComponent<Animator> ();
		anim.SetBool ("tap", true);
	}
}
